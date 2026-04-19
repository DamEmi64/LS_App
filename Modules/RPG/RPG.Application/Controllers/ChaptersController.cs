using AutoMapper;
using Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RPG.Application.Dtos;
using RPG.Application.Filters;
using RPG.Domain.Dictionaries;
using RPG.Domain.Entities;
using RPG.Domain.Repositories;

namespace RPG.Application.Controllers
{
    [Route("[controller]")]
    [AuthPermission("rpg")]
    public class ChaptersController : BaseController
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;

        public ChaptersController(IControllerService controllerService,
            IChapterRepository chapterRepository,
            IStoryRepository storyRepository,
            IMapper mapper)
            : base(controllerService)
        {
            _chapterRepository = chapterRepository;
            _storyRepository = storyRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _chapterRepository.GetWithPlayerData(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(_mapper.Map<ChapterDto>(result));
        }

        [HttpGet("")]
        public IActionResult ListData([FromQuery] ChapterFilter filter)
        {
            return Json(filter.Filter(_chapterRepository.GetAll()));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] ChapterDto dto)
        {
            var story = await _storyRepository.Get(dto.Story ?? Guid.Empty);

            if (story is null)
            {
                await Notifier.Error(SessionNotifyTypes.SessionNotFound, dto.Story?.ToString() ?? string.Empty);
                return BadRequest();
            }

            var chapter = new Chapter
            {
                Description = dto.Description,
                InsDate = DateTimeOffset.Now,
                UpdDate = DateTimeOffset.Now,
                Story = story,
                Title = dto.Title,
                Draft = dto.Draft,
            };

            await _chapterRepository.Add(chapter);
            await Notifier.Success(SessionNotifyTypes.ChapterSaved, dto.Title);
            return Ok();
        }

        [HttpPut("{id}/publish")]
        [AuthPermission("rpg_write")]
        public async Task<IActionResult> Publish(Guid id)
        {
            var entity = await _chapterRepository.Get(id);


            if (entity is not null)
            {
                entity.Draft = false;
                await _chapterRepository.Update(entity);
                await Notifier.Success(SessionNotifyTypes.ChapterUpdated, entity.Title);
            }
            else
            {
                await Notifier.Error(SessionNotifyTypes.ChapterNotFound, id);
            }

            return Ok();
        }

        [HttpPut("{id}/flow")]
        [AuthPermission("rpg_write")]
        public async Task<IActionResult> Flow(Guid id, [FromBody] FlowDto flow)
        {
            var entity = await _chapterRepository.Get(id);

            if (entity is not null)
            {
                entity.FlowJson = JsonConvert.SerializeObject(flow);
                await _chapterRepository.Update(entity);
                await Notifier.Success(SessionNotifyTypes.ChapterUpdated, entity.Title);
            }
            else
            {
                await Notifier.Error(SessionNotifyTypes.ChapterNotFound, id);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        [AuthPermission("rpg_write")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ChapterDto chapter)
        {
            var entity = await _chapterRepository.Get(id);

            if (entity is not null)
            {
                entity.Title = chapter.Title;
                entity.Description = chapter.Description;
                entity.Order = chapter.Order;

                var linksToRemove = entity.Links.Where(x => !chapter.Links.Any(y => y.Title == x.Title)).ToList();

                foreach (var item in linksToRemove)
                {
                    entity.Links.Remove(item);
                    await _chapterRepository.RemoveLink(item);
                }

                foreach (var link in chapter.Links)
                {
                    var entityLink = entity.Links.Find(x => x.Title == link.Title);

                    if (entityLink is not null)
                    {
                        entityLink.Url = link.Url;
                    }
                    else
                    {
                        entityLink = _mapper.Map<Link>(link);
                        await _chapterRepository.AddLink(entityLink);
                        entity.Links.Add(entityLink);
                    }
                }

                await _chapterRepository.Update(entity);
                await Notifier.Success(SessionNotifyTypes.ChapterUpdated, chapter.Title);
            }
            else
            {
                await Notifier.Error(SessionNotifyTypes.ChapterNotFound, chapter.Title);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _chapterRepository.Remove(id);
            await Notifier.Success(SessionNotifyTypes.ChapterDeleted, id);

            return Ok();
        }

        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartChapter(Guid id)
        {
            var chapter = await _chapterRepository.GetWithStoryAndSessions(id);
            if (chapter != null)
            {
                var startDate = DateTimeOffset.Now;

                var session = new Session { Id = Guid.NewGuid(), Start = startDate };

                chapter.Sessions.Add(session);

                if (chapter.Story.StartDate is null)
                {
                    chapter.Story.StartDate = startDate;
                }

                await _chapterRepository.AddSession(chapter, session);

                await _chapterRepository.Update(chapter);
            }

            return Ok();
        }

        [HttpPut("{id}/end")]
        public async Task<IActionResult> EndStory(Guid id)
        {
            var chapter = await _chapterRepository.GetWithStoryAndSessions(id);
            var endDate = DateTimeOffset.Now;
            if (chapter != null)
            {
                var lastSession = chapter.Sessions.LastOrDefault();

                if (lastSession is not null)
                {
                    lastSession.End = endDate;
                }

                if (chapter.Story.EndDate is null || chapter.Story.EndDate < endDate)
                {
                    chapter.Story.EndDate = endDate;
                }

                await _chapterRepository.Update(chapter);
            }

            return Ok();
        }
    }
}