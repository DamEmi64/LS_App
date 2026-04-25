using AutoMapper;
using Automation.Domain.Entities;

namespace Automation.Application.Dtos
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<AutomationDto, Automat>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                   .ForMember(dest => dest.InsDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                   .ForMember(dest => dest.UpdDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                   .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks))
                   .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                   .ForMember(dest => dest.Triggers, opt => opt.MapFrom(src => src.Triggers));

            CreateMap<TaskDto, Automation.Domain.Entities.Task>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.InsDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.UpdDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow));

            CreateMap<TriggerDto, Trigger>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.InsDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.UpdDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.Cron, opt => opt.MapFrom(src => src.Cron ?? string.Empty));
        }
    }
}
