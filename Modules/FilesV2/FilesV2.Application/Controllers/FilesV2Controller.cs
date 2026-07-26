using Base;
using FilesV2.Application.Dtos;
using FilesV2.Domain.Entities;
using FilesV2.Domain.Enums;
using FilesV2.Domain.Repositories;
using FilesV2.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FilesV2.Application.Controllers
{
    [ApiController]
    [AuthPermission("filesV2")]
    public class FilesV2Controller : BaseController
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFolderRepository _directoryRepository;
        private readonly IMediaProvider _mediaProvider;

        public FilesV2Controller(IControllerService controllerService,
            IFileRepository fileRepository,
            IFolderRepository directoryRepository,
            IMediaProviderFactory mediaProviderFactory)
            : base(controllerService)
        {
            _fileRepository = fileRepository;
            _directoryRepository = directoryRepository;
            _mediaProvider = mediaProviderFactory.Create();
        }

        // GET /api/v2/files?directoryId=&search=
        // Note: GetAll() returns entities without includes, so Owner/Users/Folder must
        // already be loaded some other way (e.g. the repository's Entity is tracked with
        // navigation properties configured as always-loaded) for this filtering to work.
        // If not, this is the first place a dedicated query method will be needed.
        [HttpGet]
        public async Task<ActionResult<List<FileV2Dto>>> ListFiles(
            [FromQuery] Guid? directoryId,
            [FromQuery] string? search) //TODO Convert to filter
        {
            var data = await _fileRepository.GetFilesByUser(CurrentUser?.UserId ?? string.Empty);
            data = data.Where(x => x.Folder?.Id == directoryId).ToList();
            var dto = data.Select(ToDto).ToList();

            return Ok(dto);
        }

        // GET /api/v2/files/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FileV2Dto>> GetFile(Guid id)
        {
            if (CurrentUser is null)
                return Unauthorized();

            var file = await _fileRepository.Get(id);
            if (file is null) return NotFound();
            if (!FileRepository.HasReadAccess(file, CurrentUser.UserId)) return Forbid();

            return Ok(ToDto(file));
        }

        // POST /api/v2/files
        // Creates the File record once content has been uploaded.
        [HttpPost]
        public async Task<ActionResult<FileV2Dto>> CreateFile([FromForm] CreateFileDto request)
        {
            if (CurrentUser == null)
                return BadRequest();

            Domain.Entities.Directory? directory = null;
            if (request.DirectoryId is not null)
            {
                directory = await _directoryRepository.Get(request.DirectoryId.Value);
                if (directory is null) return BadRequest("Directory not found.");
            }

            Guid contentId;

            using (var stream = new MemoryStream())
            {
                await request.File.CopyToAsync(stream);
                stream.Position = 0;
                contentId = await _mediaProvider.Save(stream.ToArray(), null, Path.GetExtension(request.File.FileName), owner: CurrentUser.UserId);
            }

            var file = new Domain.Entities.File
            {
                Title = Path.GetFileNameWithoutExtension(request.File.FileName),
                Description = request.Description,
                Content = contentId,
                Public = request.Public,
                Folder = directory,
                Owner = new FileUser
                {
                    UserId = CurrentUser.UserId,
                    Login = CurrentUser.Login ?? string.Empty,
                    Privilage = Privilage.Owner
                }
            };

            await _fileRepository.Add(file);
            return CreatedAtAction(nameof(GetFile), new { id = file.Id }, ToDto(file));
        }

        // GET /api/v2/files/{id}/download-url
        [HttpGet("{id:guid}/download")]
        public async Task<IActionResult> DownloadFile(Guid id)
        {
            var file = await _fileRepository.Get(id);
            if (file is null || CurrentUser is null) return NotFound();
            if (!FileRepository.HasReadAccess(file, CurrentUser.UserId)) return Forbid();

            var media = await _mediaProvider.Load(file.Content);
            if (media is null) return NotFound();

            return File(media.Content ?? Array.Empty<byte>(), media.Extension.ToContentType());
        }

        // PUT /api/v2/files/{id}
        // Rename, move to another directory, edit description, toggle public.
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FileV2Dto>> UpdateFile(Guid id, [FromBody] UpdateFileDto request)
        {
            var file = await _fileRepository.Get(id);
            if (file is null) return NotFound();
            if (CurrentUser is null || !FileRepository.HasWriteAccess(file, CurrentUser.UserId)) return Forbid();

            if (request.Title is not null) file.Title = request.Title;
            if (request.Description is not null) file.Description = request.Description;
            if (request.Public is not null) file.Public = request.Public.Value;

            if (request.DirectoryId is not null)
            {
                var directory = await _directoryRepository.Get(request.DirectoryId.Value);
                if (directory is null) return BadRequest("Directory not found.");
                file.Folder = directory;
            }

            await _fileRepository.Update(file);
            return Ok(ToDto(file));
        }

        // DELETE /api/v2/files/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            var file = await _fileRepository.Get(id);
            if (file is null) return NotFound();
            if (CurrentUser is null || file.Owner.UserId != CurrentUser.UserId) return Forbid();

            await _mediaProvider.Delete(file.Content);

            await _fileRepository.Remove(id);
            return NoContent();
        }

        // GET /api/v2/files/{id}/users
        [HttpGet("{id:guid}/users")]
        public async Task<ActionResult<List<FileUserDto>>> ListFileUsers(Guid id)
        {
            var file = await _fileRepository.Get(id);
            if (file is null) return NotFound();
            if (CurrentUser is null || !FileRepository.HasReadAccess(file, CurrentUser.UserId)) return Forbid();

            var users = file.Users
                .Select(u => new FileUserDto { UserId = u.UserId, Login = u.Login, Privilage = u.Privilage })
                .ToList();

            return Ok(users);
        }

        // POST /api/v2/files/{id}/users
        // Grants a user access to the file with a given privilege.
        [HttpPost("{id:guid}/users")]
        public async Task<ActionResult<FileUserDto>> GrantAccess(Guid id, [FromBody] GrantAccessDto request)
        {
            var file = await _fileRepository.Get(id);
            if (file is null) return NotFound();
            if (CurrentUser is null || file.Owner.UserId != CurrentUser.UserId) return Forbid();

            var existing = file.Users.FirstOrDefault(u => u.Login == request.Login);
            if (existing is not null)
            {
                existing.Privilage = request.Privilage;
            }
            else
            {
                var user = await Connect.GetUserIdByLogin(request.Login);

                if (user.IsFailed || user.Value is null) return NotFound();

                file.Users.Add(new FileUser
                {
                    UserId = user.Value.UserId,
                    Login = request.Login,
                    Privilage = request.Privilage
                });
            }

            await _fileRepository.Update(file);
            var updated = file.Users.First(u => u.Login == request.Login);
            return Ok(new FileUserDto { UserId = updated.UserId, Login = updated.Login, Privilage = updated.Privilage });
        }

        // DELETE /api/v2/files/{id}/users/{userId}
        [HttpDelete("{id:guid}/users/{Login}")]
        public async Task<IActionResult> RevokeAccess(Guid id, string login)
        {
            var file = await _fileRepository.Get(id);
            if (file is null) return NotFound();
            if (CurrentUser is null || file.Owner.UserId != CurrentUser.UserId) return Forbid();

            var target = file.Users.FirstOrDefault(u => u.Login == login);
            if (target is null) return NotFound();

            file.Users.Remove(target);
            await _fileRepository.Update(file);
            return NoContent();
        }

        private static FileV2Dto ToDto(Domain.Entities.File file) => new()
        {
            Id = file.Id,
            Title = file.Title,
            Description = file.Description,
            OwnerLogin = file.Owner.Login,
            Public = file.Public,
            DirectoryId = file.Folder?.Id,
            Path = file.Path
        };
    }
}
