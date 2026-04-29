using Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPG.Application.Dtos;
using RPG.Application.Filters;
using RPG.Domain.Dictionaries;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;

namespace RPG.Application.Controllers
{
    [Route("[controller]")]
    [AuthPermission("rpg")]
    public class PlacesController : BaseController
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IMediaProvider _mediaProvider;

        public PlacesController(
            IControllerService controllerService,
            IPlaceRepository placeRepository,
            IChapterRepository chapterRepository,
            IMediaProvider mediaProvider)
            : base(controllerService)
        {
            _placeRepository = placeRepository;
            _chapterRepository = chapterRepository;
            _mediaProvider = mediaProvider;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Place), StatusCodes.Status200OK)]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _placeRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(result);
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(ResponseList<Place>), StatusCodes.Status200OK)]
        public IActionResult ListData([FromQuery] PlaceFilter filter)
        {
            return Json(filter.Filter(_placeRepository.GetAll(), out var count), count);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] PlaceDto dto)
        {
            var chapter = await _chapterRepository.GetOnlyChapter(dto.Chapter ?? Guid.Empty);

            var place = new Place
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                Title = dto.Title,
                Chapter = chapter ?? throw new ArgumentNullException(nameof(chapter))
            };

            if (dto.Image is not null)
            {
                place.Image = await _mediaProvider.Save(dto.Image, place.Image);
            }
            else
            {
                place.Image = null;
            }

            await _placeRepository.Add(place);
            await Notifier.Success(SessionNotifyTypes.PlaceSaved, dto.Title);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] PlaceDto dto)
        {
            var place = await _placeRepository.Get(id);

            var chapter = await _chapterRepository.GetOnlyChapter(dto.Chapter ?? Guid.Empty);
            if (place is null)
            {
                await Notifier.Error(SessionNotifyTypes.PlaceNotFound, id);
                return NotFound();
            }

            if (chapter is null)
            {
                await Notifier.Error(SessionNotifyTypes.ChapterNotFound, dto.Chapter ?? Guid.Empty);
                return NotFound();
            }

            place.Description = dto.Description;
            place.Chapter = chapter;
            place.Title = dto.Title;

            if (dto.Image is not null)
            {
                place.Image = await _mediaProvider.Save(dto.Image, place.Image);
            }
            else
            {
                place.Image = null;
            }

            place.UpdDate = DateTimeOffset.Now;

            await _placeRepository.Update(place);
            await Notifier.Success(SessionNotifyTypes.PlaceUpdated, dto.Title);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _placeRepository.Remove(id);
            await Notifier.Success(SessionNotifyTypes.PlaceDeleted, id);
            return Ok();
        }
    }
}