using AutoMapper;
using Automation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Automation.Application.Dtos
{
    public class Mapper: Profile
    {
        public Mapper()
        {
            CreateMap<AutomatonDto, Automat>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                   .ForMember(dest => dest.InsDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                   .ForMember(dest => dest.UpdDate, opt => opt.MapFrom(_ => DateTimeOffset.UtcNow))
                   .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks))
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
