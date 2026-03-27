using AutoMapper;
using Files.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Files.Application.Dtos
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            {
                // DTO -> Entity
                CreateMap<FileDto, Domain.Entities.File>()
                    .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
                    .ForMember(d => d.Image, o => o.MapFrom(s => s.ImageId))
                    .ForMember(d => d.InsDate, o => o.MapFrom(_ => DateTimeOffset.Now))
                    .ForMember(d => d.UpdDate, o => o.MapFrom(_ => DateTimeOffset.Now))
                    .ForMember(d => d.AdditionalData, o => o.MapFrom(s => s))
                    .ForMember(d => d.Sources, o => o.MapFrom(s =>
                        (s.Links ?? string.Empty)
                            .Split('\n', StringSplitOptions.RemoveEmptyEntries)))
                    .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                    .ForMember(d => d.Locaction, o => o.MapFrom(s => s.Locaction))
                    .ForMember(d => d.FileType, o => o.MapFrom(s => s.FileType));

                // DTO -> AdditionalData
                CreateMap<FileDto, AdditionalData>()
                    .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
                    .ForMember(d => d.InsDate, o => o.MapFrom(_ => DateTimeOffset.Now))
                    .ForMember(d => d.UpdDate, o => o.MapFrom(_ => DateTimeOffset.Now))
                    .ForMember(d => d.Semester, o => o.MapFrom(s => s.Semester))
                    .ForMember(d => d.Subject, o => o.MapFrom(s => s.Subject))
                    .ForMember(d => d.GameGenre, o => o.MapFrom(s => s.GameGenre))
                    .ForMember(d => d.Year, o => o.MapFrom(s => s.Year));

                // string -> SourceLink
                CreateMap<string, SourceLink>()
                    .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
                    .ForMember(d => d.SourceType, o => o.MapFrom((src, _, _, ctx) => (int)ctx.Items["SourceType"]))
                    .ForMember(d => d.Link, o => o.MapFrom(src => src))
                    .ForMember(d => d.InsDate, o => o.MapFrom(_ => DateTimeOffset.Now))
                    .ForMember(d => d.UpdDate, o => o.MapFrom(_ => DateTimeOffset.Now));

                // Entity -> DTO
                CreateMap<Domain.Entities.File, FileDto>()
                    .ForMember(d => d.ImageId, o => o.MapFrom(s => s.Image))
                    .ForMember(d => d.Semester, o => o.MapFrom(s => s.AdditionalData != null ? s.AdditionalData.Semester : null))
                    .ForMember(d => d.Subject, o => o.MapFrom(s => s.AdditionalData != null ? s.AdditionalData.Subject : null))
                    .ForMember(d => d.GameGenre, o => o.MapFrom(s => s.AdditionalData != null ? s.AdditionalData.GameGenre : null))
                    .ForMember(d => d.Year, o => o.MapFrom(s => s.AdditionalData != null ? s.AdditionalData.Year : null))
                    .ForMember(d => d.SourceType, o => o.MapFrom(s => s.Sources != null && s.Sources.Any()
                        ? s.Sources.First().SourceType
                        : 0))
                    .ForMember(d => d.Links, o => o.MapFrom(s =>
                        s.Sources != null
                            ? string.Join("\n", s.Sources.Select(x => x.Link))
                            : string.Empty))
                    .ForMember(d => d.IsInstall, o => o.MapFrom(s =>
                        s.Sources != null && s.Sources.Count > 0));
            }
        }
    }
}
