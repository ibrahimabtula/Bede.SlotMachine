using Bede.SlotMachine.Engine.Entities;
using Bede.SlotMachine.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.SlotMachine.Engine
{
    public interface ISpinResult
    {
        DateTime Date { get; set; }

        double Win { get; set; }

        double Stake { get; set; }

        double Balance { get; set; }

        string Message { get; set; }

        IEnumerable<ISpinRowResult> Dimensions { get; set; }
    }
}
