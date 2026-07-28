using Base;
using FilesV2.Application.Dtos;
using FilesV2.Domain.Repositories;
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
        public ActionResult<List<DirectoryDto>> ListDirectories([FromQuery] Guid? parentId)
        {
            var directories = _directoryRepository.GetAll()
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
                Parent = parent
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

            if (!_directoryRepository.IsEmpty(id))
                return BadRequest("Directory is not empty.");

            await _directoryRepository.Remove(id);
            return NoContent();
        }

        private static DirectoryDto ToDto(Domain.Entities.Directory directory) => new()
        {
            Id = directory.Id,
            Title = directory.Title,
            ParentId = directory.Parent?.Id,
            ChildDirectoryCount = directory.Children.Count,
            FileCount = directory.Files.Count
        };
    }
}
