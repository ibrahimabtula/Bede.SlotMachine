using System.Collections.Generic;

namespace Bede.SlotMachine.Engine
{
    interface ISlotEngine
    {
        IEnumerable<ISpinResult> SpinHistory { get; }

        double Stake { get; set; }

        void EnterDeposit(double value);

        double GetBalance();

        double GetCurrentWin();

        (ISpinResult spin, bool success, string message) Spin();

    }
}
