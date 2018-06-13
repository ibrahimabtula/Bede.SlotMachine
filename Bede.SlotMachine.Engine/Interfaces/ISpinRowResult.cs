using Bede.SlotMachine.Engine.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.SlotMachine.Engine.Interfaces
{
    public interface ISpinRowResult
    {
        bool IsWin { get; set; }

        IList<ISlotSymbol> Symbols { get; set; }
    }
}
