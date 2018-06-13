using Bede.SlotMachine.Engine.Enums;

namespace Bede.SlotMachine.Engine.Symbols
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode() 
    public class WildcardSymbol : ISlotSymbol
    {
        public SymbolTypes Type => SymbolTypes.Wildcard;

        public double Probability => 0.05;

        public double Coefficient => 0.0;

        public override bool Equals(object obj)
        {
            var other = obj as ISlotSymbol;
            if (other == null)
            {
                return false;
            }

            return true;
        }
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
}
