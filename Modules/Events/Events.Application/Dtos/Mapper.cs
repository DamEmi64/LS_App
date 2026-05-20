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
                .ReverseMap();

            CreateMap<EventUser, UserDto>().ReverseMap();
            CreateMap<UserData, EventUser>().ReverseMap();

            CreateMap<UserData, UserDto>().ReverseMap();
        }
    }
}
