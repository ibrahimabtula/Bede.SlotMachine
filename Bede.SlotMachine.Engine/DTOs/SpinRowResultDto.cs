using Bede.SlotMachine.Engine.Interfaces;
using Bede.SlotMachine.Engine.Symbols;
using System.Collections.Generic;

namespace Bede.SlotMachine.Engine
{
    public class SpinRowResultDto
    {
        public bool IsWin { get; set; }

        public IList<SlotSymbolDto> Symbols { get; set; }
    }
}
