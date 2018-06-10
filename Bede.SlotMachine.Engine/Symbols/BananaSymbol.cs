using Bede.SlotMachine.Engine.Enums;

namespace Bede.SlotMachine.Engine.Symbols
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class BananaSymbol : ISlotSymbol
    {
        public SymbolTypes Type => SymbolTypes.Banana;

        public double Probability => 0.35;

        public double Coefficient => 0.6;

        public override bool Equals(object obj)
        {
            var other = obj as ISlotSymbol;
            if (other == null)
            {
                return false;
            }

            return this.Type == other.Type || other.Type == SymbolTypes.Wildcard;
        }
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}
