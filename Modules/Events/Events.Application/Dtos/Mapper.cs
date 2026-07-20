using AutoMapper;
using Base;
using Events.Domain.Entities;
namespace Events.Application.Dtos
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Event, EventDto>()
                .ForMember(desc => desc.Participates, opt => opt.MapFrom(src => src.Participates))
                .ForMember(desc => desc.Category, opt => opt.MapFrom(src => src.CategoryId))
                .ReverseMap()
                .ForMember(desc => desc.CategoryId, opt => opt.MapFrom(src => src.Category));

            CreateMap<EventUser, UserDto>().ReverseMap();
            CreateMap<UserData, EventUser>().ReverseMap()
                .ForMember(desc => desc.Permissions, opt => opt.Ignore());

            CreateMap<UserData, UserDto>().ReverseMap();
        }
    }
}
