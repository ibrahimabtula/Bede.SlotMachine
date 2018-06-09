using Bede.SlotMachine.Engine.Enums;

namespace Bede.SlotMachine.Engine.Symbols
{
    public class WildcardSymbol : ISlotSymbol
    {
        public SymbolTypes Type => SymbolTypes.Wildcard;

        public double Probability => 0.05;

        public double Coefficient => 0.0;
    }
}
