using Bede.SlotMachine.Engine.Enums;

namespace Bede.SlotMachine.Engine.Symbols
{
    public interface ISlotSymbol
    {
        SymbolTypes Type { get;}
        double Probability { get; }
        double Coefficient { get; }
    }
}
