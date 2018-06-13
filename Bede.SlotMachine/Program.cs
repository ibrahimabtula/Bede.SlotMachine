using Bede.SlotMachine.Engine;
using System;

namespace Bede.SlotMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // Just simple test
            var engine = new SlotEngine();
            engine.EnterDeposit(200);
            engine.Stake = 20;
            var res = engine.Spin();

            if(res.success)
            {
                if (200 - 20 + res.spin.Win != engine.GetBalance())
                    throw new ApplicationException();

            }
        }
    }
}
