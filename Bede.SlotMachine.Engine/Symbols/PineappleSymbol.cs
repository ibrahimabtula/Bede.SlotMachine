using Bede.SlotMachine.Engine.Enums;

namespace Bede.SlotMachine.Engine.Symbols
{
    public class PineappleSymbol : ISlotSymbol
    {
        public SymbolTypes Type => SymbolTypes.Pineapple;

        public double Probability => 0.15;

        public double Coefficient => 0.8;
    }
}
