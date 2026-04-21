using AutoMapper;
using CommunicationBase.Interfaces;
using Newtonsoft.Json;

namespace Communication.Application.Dtos
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<IFluidFunction,FluidDto>()
                .ForMember(x=>x.Title, o=>o.MapFrom(s=>s.TitleKey))
                .ForMember(x=>x.Description,o => o.MapFrom(s=>s.DescriptionKey))
                .ForMember(x=>x.Invoker, o => o.MapFrom(s => s.Invoker));
        }
    }
}
