using Bede.SlotMachine.Engine.Interfaces;
using Bede.SlotMachine.Engine.Symbols;
using System.Collections.Generic;

namespace Bede.SlotMachine.Engine.Entities
{
    public class SpinRowResult : ISpinRowResult
    {
        public bool IsWin { get; set; }

        public IList<ISlotSymbol> Symbols { get; set; }
    }
}
