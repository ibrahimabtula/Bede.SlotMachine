using AutoMapper;
using Bede.SlotMachine.Engine.Symbols;
using System;

namespace Bede.SlotMachine.Engine.MappingProfiles
{
    public class SlotSymbolProfile : Profile
    {
        public SlotSymbolProfile()
        {
            CreateMap<AppleSymbol, SlotSymbolDto>();
            CreateMap<BananaSymbol, SlotSymbolDto>();
            CreateMap<PineappleSymbol, SlotSymbolDto>();
            CreateMap<WildcardSymbol, SlotSymbolDto>();

            CreateMap<ISlotSymbol, SlotSymbolDto>()
                        .ConstructUsing((ISlotSymbol slot) =>
                        {
                            if (slot is AppleSymbol) { return Mapper.Map<SlotSymbolDto>(slot); }
                            else if (slot is BananaSymbol) { return Mapper.Map<SlotSymbolDto>(slot); }
                            else if (slot is PineappleSymbol) { return Mapper.Map<SlotSymbolDto>(slot); }
                            else if (slot is WildcardSymbol) { return Mapper.Map<SlotSymbolDto>(slot); }

                            throw new InvalidOperationException("Unknown person type: " + slot.GetType());
                        })
                        ;
        }
    }
}
