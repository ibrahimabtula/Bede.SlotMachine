using System.Collections.Generic;

namespace Bede.SlotMachine.Engine
{
    public interface ISlotEngine
    {
        IReadOnlyCollection<SpinResultDto> SpinHistory { get; }

        double Stake { get; set; }

        void EnterDeposit(double value);

        double GetBalance();

        double GetCurrentWin();

        void Reset();

        (SpinResultDto spin, bool success, string message) Spin();

    }
}
