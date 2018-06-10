using Bede.SlotMachine.Engine.Enums;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
namespace Bede.SlotMachine.Engine.Symbols
{
    public class AppleSymbol : ISlotSymbol
    {
        public SymbolTypes Type => SymbolTypes.Apple;
               
        public double Probability => 0.45;
               
        public double Coefficient => 0.4;

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
}
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
