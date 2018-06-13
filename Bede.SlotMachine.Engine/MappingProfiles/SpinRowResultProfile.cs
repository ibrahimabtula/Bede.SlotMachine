using AutoMapper;
using Bede.SlotMachine.Engine.Entities;

namespace Bede.SlotMachine.Engine.MappingProfiles
{
    public class SpinRowResultProfile : Profile
    {
        public SpinRowResultProfile()
        {
            CreateMap<SpinRowResult, SpinRowResultDto>()
                .ReverseMap();
        }
    }
}
