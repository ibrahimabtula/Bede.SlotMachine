using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.SlotMachine.Engine
{
    public class SpinResult
    {
        public DateTime Date { get; set; }
        public IList<SpinRowResult> Dimensions { get; set; }
    }
}
