using Bede.SlotMachine.Engine.Entities;
using System;
using System.Collections.Generic;

namespace Bede.SlotMachine.Engine
{
    public class SpinResultDto
    {
        public DateTime Date { get; set; }

        public double Win { get; set; }

        public double Stake { get; set; }

        public double Balance { get; set; }

        public IEnumerable<SpinRowResultDto> Dimensions { get; set; }
        public string Message { get; set; }
    }
}
