using Bede.SlotMachine.Engine.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bede.SlotMachine.Engine
{
    public class SlotEngine
    {
        private int _dimensionCount = 5;
        private int _symbolCount = 4;
        private double _balance;
        private double _stake;
        private double _currentWin = 0;
        private bool _staked = false;

        private IList<SpinResult> _spinHistory;
        
        public SlotEngine()
        {
            _spinHistory = new List<SpinResult>();
        }

        public IEnumerable<SpinResult> SpinHistory
        {
            get { return _spinHistory; }
        }

        public double Balance
        {
            get { return _balance; }
        }

        public void EnterDeposit(double value)
        {
            this._balance = value;
        }

        public void EnterStake(double value)
        {
            this._stake = value;
            MarkAsStaked();
        }

        public double GetCurrentWin()
        {
            return _currentWin;
        }

        private void MarkAsStaked()
        {
            this._staked = true;
        }

        private void MarkAsDestaked()
        {
            this._staked = false;
        }

        private bool IsSpinable()
        {
            return _balance - _stake > 0;
        }

        private void AddToHistory(SpinResult spin)
        {
            this._spinHistory.Add(spin);
        }

        private SpinRowResult GenerateRow()
        {
            var selector = new SymbolSelector();

            var spinRowResult = new SpinRowResult()
            {
                Symbols = new List<ISlotSymbol>(_symbolCount)
            };

            for(int i = 0; i < _symbolCount; i++)
            {
                spinRowResult.Symbols.Add(selector.GetSymbol()); 
            }

            return spinRowResult;
        }

        public SpinResult GetSpinResult()
        {
            SpinResult spin = null;

            if (!IsSpinable())
            {
                return spin;
            }

            spin = new SpinResult()
            {
                Date = DateTime.Now,
                Dimensions = new List<SpinRowResult>(_dimensionCount)
            };

            for (int i = 0; i < _dimensionCount; i++)
            {
                spin.Dimensions.Add(GenerateRow());
            }


            AddToHistory(spin);
            return spin;
        }

        public IEnumerable<SpinRowResult> GetWinningDimensions()
        {

            var spin = GetSpinResult();

            var result = spin.Dimensions.Where(dimension =>
            {
                var first = dimension.Symbols.First();

                var query = from symbol in dimension.Symbols
                            where symbol.Type == first.Type || symbol.Type == Enums.SymbolTypes.Wildcard
                            select symbol;

                return query.Count() == dimension.Symbols.Count();
            });

            return result;
        }

        public void Spin()
        {
            var winninDimensions = GetWinningDimensions().ToList();

            _currentWin = Math.Round(winninDimensions.Sum(d => d.Symbols.Sum(s => s.Coefficient)) * _stake, 2, MidpointRounding.AwayFromZero);
            _balance = _balance - _stake + _currentWin;
        }
    }
}
