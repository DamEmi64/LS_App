using AutoMapper;
using CommunicationBase.Interfaces;

namespace Communication.Application.Dtos
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<IFluidFunction, FluidDto>()
                .ForMember(x => x.Id, o => o.MapFrom(s => GetFluidId(s.TitleKey)))
                .ForMember(x => x.Title, o => o.MapFrom(s => s.TitleKey))
                .ForMember(x => x.Description, o => o.MapFrom(s => s.DescriptionKey))
                .ForMember(x => x.Invoker, o => o.MapFrom(s => s.Invoker));
        }

        private int GetFluidId(string titleKey)
        {
            if (int.TryParse(titleKey, out int key))
            {
                return key;
            }
            return Random.Shared.Next();
        }
    }
}
