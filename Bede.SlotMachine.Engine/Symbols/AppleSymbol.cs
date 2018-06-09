using Bede.SlotMachine.Engine.Enums;

namespace Bede.SlotMachine.Engine.Symbols
{
    public class AppleSymbol : ISlotSymbol
    {
        public SymbolTypes Type => SymbolTypes.Apple;
               
        public double Probability => 0.45;
               
        public double Coefficient => 0.4;
    }
}
