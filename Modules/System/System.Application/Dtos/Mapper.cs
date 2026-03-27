using AutoMapper;
using System;
using System.Collections.Generic;
using System.Domain.Entities;
using System.Text;

namespace System.Application.Dtos
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<JobDto, Job>()
                .ForMember(d => d.Id, o => o.Ignore()) // let DB or existing entity handle it
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate ?? DateTimeOffset.UtcNow))
                .ForMember(d => d.OperationId, o => o.MapFrom(s => s.Operation))
                .ForMember(d => d.Process, o => o.MapFrom(s =>
                    s.Process == Guid.Empty ? null : new Process { Id = s.Process, Title = s.Name}))
                .ForMember(d => d.Parent, o => o.MapFrom(s =>
                    s.Parent.HasValue ? new Job { Id = s.Parent.Value, Name = s.Name } : null))
                .ForMember(d => d.Children, o => o.MapFrom(s => s.Children ?? new List<JobDto>()));

            CreateMap<Job, JobDto>()
                .ForMember(d => d.Process, o => o.MapFrom(s => s.Process != null ? s.Process.Id : Guid.Empty))
                .ForMember(d => d.Parent, o => o.MapFrom(s => s.Parent != null ? s.Parent.Id : (Guid?)null))
                .ForMember(d => d.Operation, o => o.MapFrom(s => s.OperationId))
                .ForMember(d => d.Children, o => o.MapFrom(s => s.Children ?? new List<Job>()))
                .MaxDepth(3);
            CreateMap<ProcessDto, Process>()
                .ForMember(d => d.Id, o => o.Condition(src => src.Id != Guid.Empty)) // preserve existing
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate ?? DateTimeOffset.UtcNow))
                .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate ?? DateTimeOffset.UtcNow))
                .ForMember(d => d.UpdDate, o => o.MapFrom(_ => DateTimeOffset.UtcNow))

                .ForMember(d => d.InsDate, o => o.Condition((src, dest) => dest.Id == Guid.Empty))
                .ForMember(d => d.InsDate, o => o.MapFrom(_ => DateTimeOffset.UtcNow))
                .ForMember(d => d.Jobs, o => o.MapFrom(s => s.Jobs ?? new List<JobDto>()));

            CreateMap<Process, ProcessDto>()
                .ForMember(d => d.UpDate, o => o.MapFrom(s => s.UpdDate))
                .ForMember(d => d.Jobs, o => o.MapFrom(s => s.Jobs ?? new List<Job>()))
                .MaxDepth(3);
        }
    }
}
