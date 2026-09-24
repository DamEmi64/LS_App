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
    [AuthPermission("directoriesV2")]
    public class DirectoriesController : BaseController
    {
        private IFolderRepository _directoryRepository;

        public DirectoriesController(IControllerService controllerService, IFolderRepository directoryRepository)
            : base(controllerService)
        {
            _directoryRepository = directoryRepository;
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
                if (request.ParentId == id) return BadRequest("A directory cannot be its own parent.");
                var parent = await _directoryRepository.Get(request.ParentId.Value);
                if (parent is null) return BadRequest("Parent directory not found.");
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
                return BadRequest("Directory is not empty.");

            await _directoryRepository.Remove(id);
            return NoContent();
        }

        // GET /api/v2/files/{id}/users
        [HttpGet("{id:guid}/users")]
        public async Task<ActionResult<List<FileUserDto>>> ListFileUsers(Guid id)
        {
            var dir = await _directoryRepository.Get(id);
            if (dir is null) return NotFound();
            if (CurrentUser is null || !FolderRepository.HasReadAccess(dir, CurrentUser.UserId)) return Forbid();

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
