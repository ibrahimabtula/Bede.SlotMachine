using AutoMapper;
using Bede.SlotMachine.Engine.Entities;

namespace Bede.SlotMachine.Engine.MappingProfiles
{
    public class SpinResultProfile : Profile
    {
        public SpinResultProfile()
        {
            CreateMap<SpinResult, SpinResultDto>()
                .ReverseMap();
        }
    }
}
