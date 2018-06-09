using Bede.SlotMachine.Engine.Symbols;
using System.Collections.Generic;

namespace Bede.SlotMachine.Engine
{
    public class SpinRowResult
    {
        //public decimal Stake { get; set; }

        //public decimal Balance { get; set; }

        public IList<ISlotSymbol> Symbols { get; set; }
    }
}
