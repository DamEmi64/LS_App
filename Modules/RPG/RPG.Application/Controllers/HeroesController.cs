using AutoMapper;
using Base;
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
    public class HeroesController : BaseController
    {
        private readonly IHeroRepository _heroRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IMediaProvider _mediaProvider;
        private readonly IMapper _mapper;

        public HeroesController(IControllerService controllerService,
            IHeroRepository heroRepository,
            IChapterRepository chapterRepository,
            IMediaProvider mediaProvider,
            IMapper mapper)
            : base(controllerService)
        {
            _heroRepository = heroRepository;
            _chapterRepository = chapterRepository;
            _mediaProvider = mediaProvider;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _heroRepository.Get(id);

            if (result is null)
            {
                return NotFound();
            }

            return Json(_mapper.Map<HeroDto>(result));
        }

        [HttpGet("")]
        public IActionResult ListData([FromQuery] HeroFilter filter)
        {
            return Json(filter.Filter(_heroRepository.GetAll()));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] HeroDto dto)
        {
            var chapter = await _chapterRepository.GetOnlyChapter(dto.Chapter ?? Guid.Empty);
            var hero = new Hero
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Player = dto.Player,
                Chapter = chapter ?? throw new ArgumentNullException(nameof(chapter))
            };

            if (dto.Image is not null)
            {
                hero.Image = await _mediaProvider.Save(dto.Image, hero.Image);
            }
            else
            {
                hero.Image = null;
            }

            await _heroRepository.Add(hero);
            await Notifier.Success(SessionNotifyTypes.HeroSaved, $"{dto.FirstName} {dto.LastName}");
            return Ok();
        }

        [HttpPut("{id}/playerData")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] PlayerDataDto dto)
        {
            var hero = await _heroRepository.Get(id);

            if (hero is null)
            {
                return NotFound();
            }

            if (hero.PlayerData is null)
            {
                hero.PlayerData = new PlayerData
                {
                    Skills = dto.Skills.Select(x => new Skill
                    {
                        CategoryId = 0,
                        Title = x.Title,
                        Value = x.Value.HasValue ? x.Value.Value : 0m,
                        Id = Guid.Empty

                    }).ToList(),
                    Content = dto.PlayerData,
                    Id = Guid.Empty,
                    InsDate = DateTimeOffset.Now,
                    UpdDate = DateTimeOffset.Now
                };
            }

            else
            {
                hero.PlayerData.UpdDate = DateTimeOffset.Now;
                hero.PlayerData.Content = dto.PlayerData;

                foreach (var skill in dto.Skills)
                {
                    var dbSkill = hero.PlayerData.Skills.FirstOrDefault(x => x.Id == skill.SkillId);
                    if (dbSkill is null)
                    {
                        hero.PlayerData.Skills.Add(new Skill
                        {
                            CategoryId = 0,
                            Title = skill.Title,
                            Value = skill.Value.HasValue ? skill.Value.Value : 0m,
                        });
                    }
                    else
                    {
                        dbSkill.CategoryId = 0;
                        dbSkill.Value = skill.Value.HasValue ? skill.Value.Value : 0m;
                        dbSkill.Title = skill.Title;
                    }
                }
            }
            await _heroRepository.Update(hero);
            await Notifier.Success(SessionNotifyTypes.HeroUpdated, id);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] HeroDto dto)
        {
            var hero = await _heroRepository.Get(id);

            var chapter = await _chapterRepository.GetOnlyChapter(dto.Chapter ?? Guid.Empty);

            if (hero is null)
            {
                await Notifier.Error(SessionNotifyTypes.HeroNotFound, id);
                return NotFound();
            }

            if (chapter is null)
            {
                await Notifier.Error(SessionNotifyTypes.ChapterNotFound, dto.Chapter ?? Guid.Empty);
                return NotFound();
            }

            hero.Description = dto.Description;
            hero.Chapter = chapter;
            hero.FirstName = dto.FirstName;
            hero.LastName = dto.LastName;
            hero.Player = dto.Player;
            hero.UpdDate = DateTimeOffset.Now;

            if (dto.Image is not null)
            {
                hero.Image = await _mediaProvider.Save(dto.Image, hero.Image);
            }
            else
            {
                hero.Image = null;
            }

            if (dto.Skills?.Count > 0 || !string.IsNullOrWhiteSpace(dto.PlayerData))
            {
                if (hero.PlayerData is null)
                {
                    hero.PlayerData = new PlayerData { Skills = dto.Skills ?? new List<Skill>(), Content = dto.PlayerData };
                }
                else
                {
                    hero.PlayerData.UpdDate = DateTimeOffset.Now;
                    hero.PlayerData.Content = dto.PlayerData;

                    foreach (var skill in dto.Skills ?? new List<Skill>())
                    {
                        var dbSkill = hero.PlayerData.Skills.FirstOrDefault(x => x.Id == id);
                        if (dbSkill is null)
                        {
                            hero.PlayerData.Skills.Add(skill);
                        }
                        else
                        {
                            dbSkill.CategoryId = skill.CategoryId;
                            dbSkill.Value = skill.Value;
                            dbSkill.Title = skill.Title;
                        }
                    }
                }
            }

            await _heroRepository.Update(hero);
            await Notifier.Success(SessionNotifyTypes.HeroUpdated, $"{dto.FirstName} {dto.LastName}");
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _heroRepository.Remove(id);
            await Notifier.Success(SessionNotifyTypes.HeroDeleted, id);
            return Ok();
        }
    }
}