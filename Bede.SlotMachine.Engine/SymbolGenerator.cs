using Bede.SlotMachine.Engine.Enums;
using Bede.SlotMachine.Engine.Symbols;
using Bede.SlotMachine.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bede.SlotMachine.Engine
{
    internal class SymbolGenerator
    {
        private IList<ISlotSymbol> _symbols;

        public SymbolGenerator()
        {
            _symbols = new List<ISlotSymbol>
            {
                new WildcardSymbol(),
                new PineappleSymbol(),
                new BananaSymbol(),
                new AppleSymbol()
            };
        }

        /// <summary>
        /// Generates a random symbol according to their probability
        /// </summary>
        /// <returns></returns>
        internal ISlotSymbol Next()
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
}
