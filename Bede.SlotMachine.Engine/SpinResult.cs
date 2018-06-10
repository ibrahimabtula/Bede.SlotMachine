using System;
using System.Collections.Generic;

namespace Bede.SlotMachine.Engine
{
    public class SpinResult : ISpinResult
    {
        public DateTime Date { get; set; }

        public double Win { get; set; }

        public double Stake { get; set; }

        public double Balance { get; set; }

        public IEnumerable<SpinRowResult> Dimensions { get; set; }
    }
}
