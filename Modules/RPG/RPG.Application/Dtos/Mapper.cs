namespace RPG.Application.Dtos
{
    using AutoMapper;
    using Domain.Entities;
    using Newtonsoft.Json;

    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<SessionDto, Session>().ReverseMap();

            CreateMap<LinkDto, Link>()
                .ForMember(d => d.Id, o => o.Ignore()); // let DB handle

            CreateMap<Link, LinkDto>()
                .ForMember(d => d.Id, o => o.MapFrom(d => Random.Shared.Next())); // don't use GetHashCode ❗

            CreateMap<PlaceDto, Place>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id ?? Guid.NewGuid()))
                .ForMember(d => d.Chapter, o => o.MapFrom(s =>
                    s.Chapter.HasValue ? new Chapter { Id = s.Chapter.Value } : null))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.ImageId));

            CreateMap<Place, PlaceDto>()
                .ForMember(d => d.Chapter, o => o.MapFrom(s => s.Chapter != null ? s.Chapter.Id : (Guid?)null))
                .ForMember(d => d.ImageId, o => o.MapFrom(s => s.Image));

            CreateMap<HeroDto, Hero>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id ?? Guid.NewGuid()))
                .ForMember(d => d.Chapter, o => o.MapFrom(s =>
                    s.Chapter.HasValue ? new Chapter { Id = s.Chapter.Value } : null))
                .ForMember(d => d.PlayerData, o => o.MapFrom(s =>
                    (s.Skills != null && s.Skills.Count > 0) || !string.IsNullOrWhiteSpace(s.PlayerData)
                        ? new PlayerData
                        {
                            Skills = s.Skills ?? new List<Skill>(),
                            Content = s.PlayerData
                        }
                        : null))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.ImageId));

            CreateMap<Hero, HeroDto>()
                .ForMember(d => d.Chapter, o => o.MapFrom(s => s.Chapter != null ? s.Chapter.Id : (Guid?)null))
                .ForMember(d => d.PlayerData, o => o.MapFrom(s => s.PlayerData != null ? s.PlayerData.Content : null))
                .ForMember(d => d.Skills, o => o.MapFrom(s => s.PlayerData != null ? s.PlayerData.Skills : null))
                .ForMember(d => d.ImageId, o => o.MapFrom(s => s.Image));

            CreateMap<ChapterDto, Chapter>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id ?? Guid.NewGuid()))
                .ForMember(d => d.Story, o => o.MapFrom(s =>
                    s.Story.HasValue ? new Story { Id = s.Story.Value } : null))
                .ForMember(d => d.Heroes, o => o.MapFrom(s => s.Heroes ?? new()))
                .ForMember(d => d.Places, o => o.MapFrom(s => s.Places ?? new()))
                .ForMember(d => d.Sessions, o => o.MapFrom(s => s.Sessions ?? new()))
                .ForMember(d => d.Links, o => o.MapFrom(s => s.Links ?? new()));

            CreateMap<Chapter, ChapterDto>()
                .ForMember(d => d.Flow, o => o.MapFrom(s => JsonConvert.DeserializeObject<FlowDto>(s.FlowJson ?? string.Empty)))
                .ForMember(d => d.Story, o => o.MapFrom(s => s.Story != null ? s.Story.Id : (Guid?)null))

                // optimized (no double OrderBy)
                .ForMember(d => d.StartDate, o => o.MapFrom(s =>
                    s.Sessions != null && s.Sessions.Any()
                        ? s.Sessions.Min(x => x.Start)
                        : (DateTimeOffset?)null))

                .ForMember(d => d.EndDate, o => o.MapFrom(s =>
                    s.Sessions != null && s.Sessions.Any()
                        ? s.Sessions.Max(x => x.End)
                        : null))

                .ForMember(d => d.Heroes, o => o.MapFrom(s => s.Heroes ?? new()))
                .ForMember(d => d.Places, o => o.MapFrom(s => s.Places ?? new()))
                .ForMember(d => d.Sessions, o => o.MapFrom(s => s.Sessions ?? new()))
                .ForMember(d => d.Links, o => o.MapFrom(s => s.Links ?? new()))
                .MaxDepth(3);


            // --------------------
            // Story
            // --------------------
            CreateMap<StoryDto, Story>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id ?? Guid.NewGuid()))
                .ForMember(d => d.Summary, o => o.Ignore())
                .ForMember(d => d.Chapters, o => o.MapFrom(s => s.Chapters ?? new()));

            CreateMap<Story, StoryDto>()
                .ForMember(d => d.Chapters, o => o.MapFrom(s => s.Chapters ?? new()))
                .MaxDepth(3);

            CreateMap<SkillDto, Skill>().ReverseMap();
            CreateMap<RPGFile, RpgFileDto>().ReverseMap();
        }
    }
}
