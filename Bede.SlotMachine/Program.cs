using Bede.SlotMachine.Engine;
using System;

namespace Bede.SlotMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var engine = new SlotEngine();
            engine.EnterDeposit(200);
            engine.EnterStake(20);
            engine.Spin();
        }
    }
}
