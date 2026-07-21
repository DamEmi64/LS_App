using Base;
using Files.Application.Dtos;
using Files.Application.Filters;
using Files.Domain.Dictionaries;
using Files.Domain.Repositories;
using Files.Infrastructure.Services.DownloadService;
using Files.Infrastructure.Services.ManagmentService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Files.Application.Controllers
{
    [AuthPermission("files")]
    public class FilesController : BaseController
    {
        private readonly IFileRepository _fileRepository;
        private readonly IImportService _importService;
        private readonly IManagmentService _managmentService;
        private readonly IMediaProvider _mediaProvider;

        public FilesController(
            IControllerService controllerService,
            IFileRepository fileRepository,
            IImportService importService,
            IManagmentService managmentService,
            IMediaProviderFactory mediaProviderFactory)
            : base(controllerService)
        {
            _fileRepository = fileRepository;
            _importService = importService;
            _managmentService = managmentService;
            _mediaProvider = mediaProviderFactory.Create(AppConfiguration.GetValue<string>("DefaultStorage"));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Domain.Entities.File), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _fileRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ResponseList<Domain.Entities.File>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListData([FromQuery] FileFilter filter)
        {
            var files = filter.Filter(_fileRepository.GetAll(), out var count);

            var dtos = files
                .Select(FileDto.ToDto)
                .ToList();

            if (filter.IncludeImages)
            {
                var imageIds = dtos
                                .Where(x => x.ImageId != null)
                                .Select(x => x.ImageId!.Value)
                                .Distinct()
                                .ToList();

                var media = await _mediaProvider.LoadMany(imageIds).ToListAsync();

                foreach (var item in dtos)
                {
                    if (item.ImageId != null)
                    {
                        var mediaItem = media.FirstOrDefault(m => m?.Id == item.ImageId);
                        item.ImageData = mediaItem?.ContentStr;
                    }
                }
            }

            return Json(dtos, count);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] FileDto dto)
        {
            var file = dto.ToEntity();

            if (dto.ImageData is not null)
            {
                file.Image = await _mediaProvider.Save(dto.ImageData, dto.ImageId);
            }

            if (dto.Content is not null)
            {
                file.Content = await _mediaProvider.Save(Convert.FromBase64String(dto.Content), null);
            }

            file.UpdDate = DateTimeOffset.Now;
            await _fileRepository.Add(file);
            await Notifier.Success(FileNotifyTypes.FileSave, dto.Title);
            return Ok();
        }

        [HttpPut("{id}/import")]
        public async Task<IActionResult> Download(Guid id, [FromQuery] string? newLocaction)
        {
            var file = await _fileRepository.Get(id);
            ArgumentNullException.ThrowIfNull(file);

            var title = await _importService.ImportFile(file, CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });

            await Notifier.Info(NotifyTypes.ProcessQueued, title);

            return Ok();
        }

        [HttpGet("{id}/export")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportFile(Guid id, [FromQuery] string? newLocaction)
        {
            var file = await _fileRepository.Get(id);
            ArgumentNullException.ThrowIfNull(file);

            if (file.Content is null)
            {
                return BadRequest("File has no content to export.");
            }

            var media = await _mediaProvider.Load(file.Content ?? Guid.Empty, true);

            if (media is null)
            {
                return NotFound();
            }

            return File(media.Content ?? Array.Empty<byte>(), media.Extension.ToContentType(), file.Title + "." + media.Extension);
        }

        [HttpPut("{id}/copy")]
        public async Task<IActionResult> Copy(Guid id, [FromQuery] string newLocaction)
        {
            var file = await _fileRepository.Get(id);

            if (file is null)
            {
                return NotFound();
            }

            var title = await _managmentService.CopyFile(file, newLocaction, CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });

            await Notifier.Info(NotifyTypes.ProcessQueued, title);

            return Ok();
        }

        [HttpPut("{id}/move")]
        public async Task<IActionResult> Move(Guid id, [FromQuery] string newLocaction)
        {
            var file = await _fileRepository.Get(id);

            if (file is null || string.IsNullOrEmpty(newLocaction))
            {
                return NotFound();
            }

            var title = await _managmentService.MoveFile(file, newLocaction, CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });

            await Notifier.Info(NotifyTypes.ProcessQueued, title);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] FileDto dto)
        {
            var entity = await _fileRepository.Get(id);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Title = dto.Title;

            if (entity.Locaction != dto.Locaction && entity.Locaction is not null && dto.Locaction is not null)
            {
                await _managmentService.MoveFile(entity, dto.Locaction, CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
            }

            entity.Locaction = dto.Locaction;

            if (dto.ImageData is not null)
            {
                entity.Image = await _mediaProvider.Save(dto.ImageData, entity.Image);
            }
            else
            {
                await _mediaProvider.Delete(entity.Image);
                entity.Image = null;
            }

            if (entity.AdditionalData is not null)
            {
                entity.AdditionalData.Subject = dto.Subject;
                entity.AdditionalData.Year = dto.Year;
                entity.AdditionalData.Semester = dto.Semester;
                entity.AdditionalData.GameGenre = dto.GameGenre;
            }

            await _fileRepository.Update(entity);
            await Notifier.Success(FileNotifyTypes.FileUpdated, dto.Title);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] bool pernament)
        {
            if (pernament)
            {
                var file = await _fileRepository.Get(id);

                if (file is not null)
                {
                    await _managmentService.DeleteFile(file, CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
                }
            }

            await _fileRepository.Remove(id);

            await Notifier.Success(FileNotifyTypes.FileDeleted, id.ToString());
            return Ok();
        }
    }
}