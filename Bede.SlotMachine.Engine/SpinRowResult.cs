using Bede.SlotMachine.Engine.Symbols;
using System.Collections.Generic;

namespace Bede.SlotMachine.Engine
{
    public class SpinRowResult
    {
        public bool IsWin { get; set; }

        public IList<ISlotSymbol> Symbols { get; set; }
    }
}
