using AutoMapper;
using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPG.Application.Dtos;
using RPG.Application.Filters;
using RPG.Domain.Dictionaries;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;
using RPG.Infrastructure.Services;
using RPG.Infrastructure.Services.SummaryService;

namespace RPG.Application.Controllers
{
    [AuthPermission("rpg")]
    public class StoriesController : BaseController
    {
        private readonly IStoryRepository _storyRepository;
        private readonly ISummaryService _summaryService;
        private readonly IMediaProvider _mediaProvider;
        private readonly IMapper _mapper;
        private readonly IImportService _importService;

        public StoriesController(
            IControllerService controllerService,
            IStoryRepository storyRepository,
            ISummaryService summaryService,
            IMediaProviderFactory mediaProviderFactory,
            IMapper mapper,
            IImportService importService)
            : base(controllerService)
        {
            _storyRepository = storyRepository;
            _summaryService = summaryService;
            _mediaProvider = mediaProviderFactory.Create();
            _mapper = mapper;
            _importService = importService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Story), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _storyRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(_mapper.Map<StoryDto>(result));
        }

        [HttpGet("{id}/draft")]
        [AuthPermission("rpg-draft")]
        [ProducesResponseType(typeof(Story), StatusCodes.Status200OK)]
        public async Task<IActionResult> DetailsDraft(Guid id)
        {
            var result = await _storyRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(_mapper.Map<StoryDto>(result));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ResponseList<Story>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListData([FromQuery] StoryFilter filter)
        {
            var stories = _storyRepository.GetAll();
            return Json(filter.Filter(stories, out var count).Select(_mapper.Map<StoryDto>), count);
        }

        [HttpGet("draft")]
        [AuthPermission("rpg-draft")]
        [ProducesResponseType(typeof(ResponseList<Story>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListDrafts([FromQuery] StoryFilter filter)
        {
            var stories = _storyRepository.GetAllDraft();
            return Json(filter.Filter(stories, out var count).Select(_mapper.Map<StoryDto>), count);
        }

        [HttpPost("")]
        [AuthPermission("rpg-write")]
        public async Task<IActionResult> Create([FromBody] StoryDto dto)
        {
            var entity = _mapper.Map<Story>(dto);

            var files = new List<RPGFile>();
            await _storyRepository.Add(entity);
            await Notifier.Success(SessionNotifyTypes.SessionSaved, dto.Title);

            return Ok();
        }

        [HttpPost("import")]
        [AuthPermission("rpg-write")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Import([FromForm] ImportDto dto)
        {
            var fileName = dto.ExternalUrl;
            if (dto.File is not null)
            {
                fileName = dto.File.FileName;
            }

            var job = await _importService.ImportFromFile(dto.File ?? null, dto.ConverterType, dto.ExternalUrl, CurrentUser ?? new UserData { UserId = Guid.Empty.ToString() });

            await Notifier.Success(NotifyTypes.ProcessQueued, job);

            return Ok();
        }

        [HttpGet("{id}/export")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Export(Guid id)
        {
            var storyName = _storyRepository.GetStoryTitle(id) ?? throw new ArgumentException("Story not found", nameof(id));

            var data = await _importService.ExportAsJson(id);

            return File(data, "application/json", $"story_{id}.json");
        }


        [HttpPut("{id}")]
        [AuthPermission("rpg-write")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] StoryDto dto)
        {
            var place = await _storyRepository.Get(id);
            ArgumentNullException.ThrowIfNull(place);

            place.Title = dto.Title;
            place.Description = dto.Description;
            place.StartDate = dto.StartDate;
            place.EndDate = dto.EndDate;

            await _storyRepository.Update(place);
            await Notifier.Success(SessionNotifyTypes.SessionUpdated, dto.Title);

            return Ok();
        }

        [HttpDelete("{id}")]
        [AuthPermission("rpg-write")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _storyRepository.Remove(id);
            await Notifier.Success(SessionNotifyTypes.SessionDeleted, id);

            return Ok();
        }

        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartStory(Guid id)
        {
            var story = await _storyRepository.Get(id);
            if (story != null)
            {
                story.StartDate = DateTimeOffset.Now;
                await _storyRepository.Update(story);
            }

            return Ok();
        }

        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndStory(Guid id)
        {
            var story = await _storyRepository.Get(id);
            if (story != null)
            {
                story.EndDate = DateTimeOffset.Now;
                await _storyRepository.Update(story);
            }

            return Ok();
        }

        [HttpPut("{id}/summary")]
        public async Task<IActionResult> GenerateSummary(Guid id, [FromBody] SummaryModel dto)
        {
            var title = await _summaryService.QueueGenerateSummaryJob(id, dto, CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() }, dto.IsPdf);
            await Notifier.Success(NotifyTypes.ProcessQueued, title);
            return Ok();
        }

        [HttpPut("{id}/firebase")]
        public async Task<IActionResult> SendToFirebase(Guid id, [FromBody] SummaryModel dto)
        {
            var title = await _summaryService.QueueSendToFirebaseJob(id, dto, CurrentUser ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
            await Notifier.Success(NotifyTypes.ProcessQueued, title);
            return Ok();
        }

        [HttpPost("{id}/files")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddRPGFile(Guid id, [FromForm] CreateRPGFileDto dto)
        {
            if (dto.File is null)
                return BadRequest();

            var story = await _storyRepository.Get(id);

            if (story is null)
                return NotFound();

            var title = Path.GetFileNameWithoutExtension(dto.File!.FileName);
            var extension = Path.GetExtension(dto.File!.FileName);

            using (var stream = new MemoryStream())
            {
                await dto.File.CopyToAsync(stream);
                var fileId = await _mediaProvider.Save(stream.ToArray(), null, extension);
                var file = new RPGFile
                {
                    Id = dto.FileId ?? Guid.NewGuid(),
                    Title = title ?? "FILE",
                    Content = fileId
                };

                await _storyRepository.AddFile(story, file);

                return Ok();
            }
        }

        [HttpDelete("{id}/files/{fileId}")]
        public async Task<IActionResult> DeleteRPGFile([FromRoute] Guid id, [FromRoute] Guid fileId)
        {
            var story = await _storyRepository.Get(id);

            if (story is null)
                return NotFound();

            story.Files.RemoveAll(x => x.Id == fileId);

            await _storyRepository.Update(story);

            return Ok();
        }
    }
}