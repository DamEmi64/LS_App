using AutoMapper;
using Base;
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
    [Route("[controller]")]
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
            IMediaProvider mediaProvider,
            IMapper mapper,
            IImportService importService)
            : base(controllerService)
        {
            _storyRepository = storyRepository;
            _summaryService = summaryService;
            _mediaProvider = mediaProvider;
            _mapper = mapper;
            _importService = importService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _storyRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(_mapper.Map<StoryDto>(result));
        }

        [HttpGet("")]
        public async Task<IActionResult> ListData([FromQuery] StoryFilter filter)
        {
            var stories = _storyRepository.GetAll();
            return Json(filter.Filter(stories).Select(x => _mapper.Map<StoryDto>(x)));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] StoryDto dto)
        {
            var entity = _mapper.Map<Story>(dto);

            await _storyRepository.Add(entity);
            await Notifier.Success(SessionNotifyTypes.SessionSaved, dto.Title);

            return Ok();
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import([FromBody] ImportDto dto)
        {
            var fileName = dto.File.FileName;

            var job = _importService.ImportFromFile(dto.File, dto.ConverterType, await GetCurrentUser() ?? new UserData { UserId = Guid.Empty.ToString() });

            await Notifier.Success(NotifyTypes.ProcessQueued, job);

            return Ok();
        }


        [HttpPut("{id}")]
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
            var title = await _summaryService.QueueGenerateSummaryJob(id, dto, await GetCurrentUser() ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() }, dto.IsPdf);
            await Notifier.Success(NotifyTypes.ProcessQueued, title);
            return Ok();
        }

        [HttpPut("{id}/firebase")]
        public async Task<IActionResult> SendToFirebase(Guid id, [FromBody] SummaryModel dto)
        {
            var title = await _summaryService.QueueSendToFirebaseJob(id, dto, await GetCurrentUser() ?? new UserData() { Id = 0, UserId = Guid.Empty.ToString() });
            await Notifier.Success(NotifyTypes.ProcessQueued, title);
            return Ok();
        }

        [HttpGet("{id}/summary")]
        public async Task<IActionResult> DownloadSummary(Guid id)
        {
            var story = await _storyRepository.Get(id);
            if (story != null)
            {
                var file = await _mediaProvider.Load(story.Summary ?? Guid.Empty);

                if (file is null || file.Content is null)
                    return NotFound();

                return File(file.Content, file.Extension.ToContentType(), story.Title + "_Summary." + file.Extension);
            }

            return NotFound();
        }
    }
}