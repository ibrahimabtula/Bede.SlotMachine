﻿using System;
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

        IEnumerable<SpinRowResult> Dimensions { get; set; }
    }
}
