using AutoMapper;
using RPG.Domain.Entities;
using RPG.Infrastructure.External.FileConverters.Json;

namespace RPG.Infrastructure.External.FileConverters
{
    public class FileDataMapper : Profile
    {
        public FileDataMapper()
        {
            // Story ↔ StoryDto
            CreateMap<Story, StoryDto>()
                .ReverseMap();

            CreateMap<Chapter, ChapterDto>()
                .ForMember(dest => dest.Links,
                    opt => opt.MapFrom(src => src.Links.ToDictionary(
                        l => l.Title,
                        l => l.Url)))
                .ReverseMap()
                .ForMember(dest => dest.Links,
                    opt => opt.MapFrom(src => src.Links.Select(kvp => new Link
                    {
                        Title = kvp.Key,
                        Url = kvp.Value
                    })));

            CreateMap<Hero, HeroDto>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Chapter, opt => opt.Ignore()); // avoid circular dependency

            CreateMap<Place, PlaceDto>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.Chapter, opt => opt.Ignore()); // avoid circular dependency
        }
    }
}
