using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.SlotMachine.Engine.Utils
{
    internal static class ThreadSafeRandom
    {
        private static Random random = new Random();

        internal static double Next()
        {
            lock (random)
            {
                return random.NextDouble();
            }
        }
    }
}
