using Base;
using FilesV2.Application.Dtos;
using FilesV2.Domain.Entities;
using FilesV2.Domain.Dictionaries;
using FilesV2.Domain.Enums;
using FilesV2.Domain.Repositories;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;

namespace FilesV2.Application.Controllers
{
    [ApiController]
    [AuthPermission("directoriesV2")]
    public class DirectoriesController : BaseController
    {
        private readonly IFolderRepository _directoryRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMediaProvider _mediaProvider;

        public DirectoriesController(IControllerService controllerService, IFolderRepository directoryRepository,
            IFileRepository fileRepository, IMediaProviderFactory mediaProviderFactory)
            : base(controllerService)
        {
            _directoryRepository = directoryRepository;
            _fileRepository = fileRepository;
            _mediaProvider = mediaProviderFactory.Create();
        }

        // GET /api/v2/directories?parentId=
        // Omit parentId to list root-level directories.
        [HttpGet]
        public async Task<ActionResult<List<DirectoryDto>>> ListDirectoriesAsync([FromQuery] Guid? parentId)
        {
            var directories = (await _directoryRepository.GetDirectoriesByUser(CurrentUser?.UserId ?? string.Empty))
                .Where(d => (parentId is null && d.Parent is null) || d.Parent?.Id == parentId)
                .Select(ToDto)
                .ToList();

            return Ok(directories);
        }

        // GET /api/v2/directories/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DirectoryDto>> GetDirectory(Guid id)
        {
            var directory = await _directoryRepository.Get(id);
            if (directory is null) return NotFound();
            return Ok(ToDto(directory));
        }

        // POST /api/v2/directories
        [HttpPost]
        public async Task<ActionResult<DirectoryDto>> CreateDirectory([FromBody] CreateDirectoryDto request)
        {
            Domain.Entities.Directory? parent = null;
            if (request.ParentId is not null)
            {
                parent = await _directoryRepository.Get(request.ParentId.Value);
            }

            var directory = new Domain.Entities.Directory
            {
                Title = request.Title,
                Parent = parent,
                Public = false,
                Owner = new CatalogUser
                {
                    UserId = CurrentUser?.UserId ?? throw new ArgumentNullException(),
                    Login = CurrentUser.Login ?? string.Empty,
                    Privilage = Privilage.Owner
                }
            };

            await _directoryRepository.Add(directory);
            var uploadedDirectory = await _directoryRepository.Get(directory.Id) ?? directory;
            return CreatedAtAction(nameof(GetDirectory), new { id = directory.Id }, ToDto(uploadedDirectory));
        }

        [HttpPost("upload")]
        public async Task<ActionResult<DirectoryDto>> Upload([FromForm] UploadDirectoryDto request)
        {
            if (CurrentUser is null) return Unauthorized();
            if (request.Zip is null || request.Zip.Length == 0) return BadRequest(FileV2Logs.ZipRequired);

            Domain.Entities.Directory? parent = null;
            if (request.ParentId is not null)
            {
                parent = await _directoryRepository.Get(request.ParentId.Value);
                if (parent is null) return NotFound("Parent directory not found.");
                if (parent.Owner.UserId != CurrentUser.UserId &&
                    !parent.Users.Any(user => user.UserId == CurrentUser.UserId && user.Privilage == Privilage.Write))
                    return Forbid();
            }

            var owner = new CatalogUser
            {
                UserId = CurrentUser.UserId,
                Login = CurrentUser.Login ?? string.Empty,
                Privilage = Privilage.Owner
            };
            var directory = new Domain.Entities.Directory
            {
                Title = request.Title,
                Parent = parent,
                Public = false,
                Owner = owner
            };
            try
            {
                using var zipStream = request.Zip.OpenReadStream();
                using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
                await _directoryRepository.Add(directory);
                var directories = new Dictionary<string, Domain.Entities.Directory>(StringComparer.Ordinal)
                {
                    [string.Empty] = directory
                };

                foreach (var entry in archive.Entries)
                {
                    var normalizedPath = entry.FullName.Replace('\\', '/').Trim('/');
                    if (string.IsNullOrWhiteSpace(normalizedPath)) continue;
                    var segments = normalizedPath.Split('/');
                    if (segments.Any(segment => segment is ".." or "." || segment.Contains(':')))
                        return BadRequest(FileV2Logs.ZipInvalidPath);

                    var isDirectory = entry.FullName.EndsWith('/') || entry.FullName.EndsWith('\\') || string.IsNullOrEmpty(entry.Name);
                    var directorySegmentCount = isDirectory ? segments.Length : segments.Length - 1;
                    var currentPath = string.Empty;
                    for (var i = 0; i < directorySegmentCount; i++)
                    {
                        var segment = segments[i];
                        currentPath = currentPath.Length == 0 ? segment : $"{currentPath}/{segment}";
                        if (directories.ContainsKey(currentPath)) continue;

                        var child = new Domain.Entities.Directory
                        {
                            Title = segment,
                            Parent = directories[currentPath[..Math.Max(0, currentPath.LastIndexOf('/'))]],
                            Owner = new CatalogUser { UserId = owner.UserId, Login = owner.Login, Privilage = Privilage.Owner }
                        };
                        await _directoryRepository.Add(child);
                        directories[currentPath] = child;
                    }

                    if (isDirectory) continue;
                    var fileName = segments[^1];
                    await using var contentStream = entry.Open();
                    using var memory = new MemoryStream();
                    await contentStream.CopyToAsync(memory);
                    var contentId = await _mediaProvider.Save(memory.ToArray(), null, Path.GetExtension(fileName), owner: owner.UserId);
                    await _fileRepository.Add(new Domain.Entities.File
                    {
                        Title = fileName,
                        Content = contentId,
                        Folder = directories[currentPath],
                        Owner = new CatalogUser { UserId = owner.UserId, Login = owner.Login, Privilage = Privilage.Owner }
                    });
                }
            }
            catch (InvalidDataException)
            {
                return BadRequest(FileV2Logs.ZipInvalid);
            }

            return CreatedAtAction(nameof(GetDirectory), new { id = directory.Id }, ToDto(directory));
        }

        // PUT /api/v2/directories/{id}
        // Rename and/or move to a new parent.
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DirectoryDto>> UpdateDirectory(Guid id, [FromBody] UpdateDirectoryDto request)
        {
            var directory = await _directoryRepository.Get(id);
            if (directory is null) return NotFound();

            if (request.Title is not null) directory.Title = request.Title;

            if (request.Public is not null) directory.Public = request.Public ?? false;

            if (request.ParentId is not null)
            {
                if (request.ParentId == id) return BadRequest(FileV2Logs.DirectorySelfParent);
                var parent = await _directoryRepository.Get(request.ParentId.Value);
                if (parent is null) return BadRequest(FileV2Logs.ParentNotFound);
                directory.Parent = parent;
            }

            await _directoryRepository.Update(directory);
            return Ok(ToDto(directory));
        }

        // DELETE /api/v2/directories/{id}
        // Rejects deletion of non-empty directories; caller should move/delete contents first.
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDirectory(Guid id)
        {
            var directory = await _directoryRepository.Get(id);
            if (directory is null) return NotFound();

            if (directory.Owner.UserId != CurrentUser?.UserId)
                return Forbid();

            if (!_directoryRepository.IsEmpty(id))
                return BadRequest(FileV2Logs.DirectoryNotEmpty);

            await _directoryRepository.Remove(id);
            return NoContent();
        }

        // GET /api/v2/files/{id}/users
        [HttpGet("{id:guid}/users")]
        public async Task<ActionResult<List<FileUserDto>>> ListFileUsers(Guid id)
        {
            var dir = await _directoryRepository.Get(id);
            if (dir is null) return NotFound();
            if (CurrentUser is null || !(dir.Owner.UserId == CurrentUser.UserId)) return Forbid();

            var users = dir.Users
                .Select(u => new FileUserDto { UserId = u.UserId, Login = u.Login, Privilage = u.Privilage })
                .ToList();

            return Ok(users);
        }

        // POST /api/v2/files/{id}/users
        // Grants a user access to the dir with a given privilege.
        [HttpPost("{id:guid}/users")]
        public async Task<ActionResult<FileUserDto>> GrantAccess(Guid id, [FromBody] GrantAccessDto request)
        {
            var dir = await _directoryRepository.Get(id);
            if (dir is null) return NotFound();
            if (CurrentUser is null || dir.Owner.UserId != CurrentUser.UserId) return Forbid();

            var existing = dir.Users.FirstOrDefault(u => u.Login == request.Login);
            if (existing is not null)
            {
                existing.Privilage = request.Privilage;
            }
            else
            {
                dir.Users.Add(new CatalogUser
                {
                    UserId = request.UserId,
                    Login = request.Login,
                    Privilage = request.Privilage
                });
            }

            await _directoryRepository.Update(dir);
            var updated = dir.Users.First(u => u.UserId == request.UserId);
            return Ok(new FileUserDto { UserId = updated.UserId, Login = updated.Login, Privilage = updated.Privilage });
        }

        // DELETE /api/v2/files/{id}/users/{userId}
        [HttpDelete("{id:guid}/users/{userId}")]
        public async Task<IActionResult> RevokeAccess(Guid id, string userId)
        {
            var dir = await _directoryRepository.Get(id);
            if (dir is null) return NotFound();
            if (CurrentUser is null || dir.Owner.UserId != CurrentUser.UserId) return Forbid();

            var target = dir.Users.FirstOrDefault(u => u.UserId == userId);
            if (target is null) return NotFound();

            dir.Users.Remove(target);
            await _directoryRepository.Update(dir);
            return NoContent();
        }

        private static DirectoryDto ToDto(Domain.Entities.Directory directory) => new()
        {
            Id = directory.Id,
            Title = directory.Title,
            ParentId = directory.Parent?.Id,
            ChildDirectoryCount = directory.Children.Count,
            FileCount = directory.Files.Count,
            Owner = directory.Owner.UserId,
            Public = directory.Public,
            FileUsers = directory.Users.Select(x => new FileUserDto
            {
                UserId = x.UserId,
                Login = x.Login,
                Privilage = x.Privilage
            }).ToList()
        };
    }
}

