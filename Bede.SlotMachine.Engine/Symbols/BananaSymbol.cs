using Bede.SlotMachine.Engine.Enums;

namespace Bede.SlotMachine.Engine.Symbols
{
    public class BananaSymbol : ISlotSymbol
    {
        public SymbolTypes Type => SymbolTypes.Banana;

        public double Probability => 0.35;

        public double Coefficient => 0.6;
    }
}
