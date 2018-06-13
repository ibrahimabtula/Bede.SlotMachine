using AutoMapper;
using Bede.SlotMachine.Engine.Symbols;
using System;

namespace Bede.SlotMachine.Engine.MappingProfiles
{
    public class SlotSymbolProfile : Profile
    {
        public SlotSymbolProfile()
        {
            CreateMap<ISlotSymbol, SlotSymbolDto>();
        }
    }
}
