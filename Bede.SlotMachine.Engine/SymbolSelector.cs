using Bede.SlotMachine.Engine.Enums;
using Bede.SlotMachine.Engine.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.SlotMachine.Engine
{
    internal class SymbolSelector
    {
        private IList<ISlotSymbol> _symbols;

        public SymbolSelector()
        {
            _symbols = new List<ISlotSymbol>
            {
                new WildcardSymbol(),
                new PineappleSymbol(),
                new BananaSymbol(),
                new AppleSymbol()
            };
        }

        public ISlotSymbol GetSymbol()
        {
            double diceRoll = ThreadSafeRandom.Next();

            double cumulative = 0.0;
            for (int i = 0; i < _symbols.Count; i++)
            {
                cumulative += _symbols[i].Probability;
                if (diceRoll < cumulative)
                {
                    ISlotSymbol selectedElement = _symbols[i];
                    return selectedElement;
                }
            }

            return default(ISlotSymbol);
        }

    }

    class ThreadSafeRandom
    {
        private static Random random = new Random();

        public static double Next()
        {
            lock (random)
            {
                return random.NextDouble();
            }
        }
    }
}
