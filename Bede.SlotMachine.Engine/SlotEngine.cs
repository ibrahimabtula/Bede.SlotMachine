using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bede.SlotMachine.Engine.Entities;
using Bede.SlotMachine.Engine.MappingProfiles;
using Bede.SlotMachine.Engine.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bede.SlotMachine.Engine
{
    public class SlotEngine : ISlotEngine
    {
        private int _dimensionCount = 4;
        private int _symbolCount = 3;
        private double _balance;
        private double _stake;
        private double _currentWin = 0;
        private bool _staked = false;

        private IMapper mapper;

        private IList<SpinResultDto> _spinHistory;
        
        public SlotEngine()
        {
            _spinHistory = new List<SpinResultDto>();

            InitMapper();
        }

        private void InitMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SpinResultProfile());
                cfg.AddProfile(new SpinRowResultProfile());
                cfg.AddProfile(new SlotSymbolProfile());
            });
            mapper = mapperConfiguration.CreateMapper();
        }

        public IEnumerable<SpinResultDto> SpinHistory
        {
            get { return _spinHistory; }
        }

        public double Stake
        {
            get { return _stake; }
            set { _stake = value; }
        }

        public void EnterDeposit(double value)
        {
            this._balance += value;
        }

        public double GetBalance()
        {
            return _balance;
        }

        public double GetCurrentWin()
        {
            return _currentWin;
        }

        private (bool valid, string message) IsSpinable()
        {
            if(_balance == 0)
            {
                return (false, "Not enough balance, please deposit!");
            }
            else if(_stake == 0)
            {
                return (false, "Stake is 0, please set stake size!");
            }
            else if(_balance - _stake < 0)
            {
                return (false, "Not enough balance, please deposit!");
            }

            return (true, null);
        }

        private void AddToHistory(SpinResultDto spin)
        {
            this._spinHistory.Add(spin);
        }

        private SpinRowResult GenerateRow()
        {
            var symbolGenerator = new SymbolGenerator();

            var spinRowResult = new SpinRowResult()
            {
                Symbols = new List<ISlotSymbol>(_symbolCount)
            };

            for (int i = 0; i < _symbolCount; i++)
            {
                spinRowResult.Symbols.Add(symbolGenerator.Next());
            }

            return spinRowResult;
        }

        private IEnumerable<SpinRowResult> GenerateDimensions()
        {          
            var dimensions = new List<SpinRowResult>(_dimensionCount);

            for (int i = 0; i < _dimensionCount; i++)
            {
                dimensions.Add(GenerateRow());
            }
            
            return dimensions;
        }

        private IEnumerable<SpinRowResult> GetWinningDimensions(IEnumerable<SpinRowResult> Dimensions)
        {
            // Check each dimension for win and return winning ones if any
            return Dimensions.Where(dimension =>
            {
                bool isWin = dimension.Symbols.All(a => a.Equals(new AppleSymbol()));

                if(!isWin)
                {
                    isWin = dimension.Symbols.All(a => a.Equals(new BananaSymbol()));
                }

                if (!isWin)
                {
                    isWin = dimension.Symbols.All(a => a.Equals(new PineappleSymbol()));
                }

                dimension.IsWin = isWin;

                return isWin;
            });
        }

        public (SpinResultDto spin, bool success, string message) Spin()
        {
            var spinable = IsSpinable();
            if (!spinable.valid)
            {
                return (null, spinable.valid, spinable.message);
            }

            var dimmensions = GenerateDimensions();
            var winninDimensions = GetWinningDimensions(dimmensions);

            _currentWin = Math.Round(winninDimensions.Sum(d => d.Symbols.Sum(s => s.Coefficient)) * _stake, 2, MidpointRounding.AwayFromZero);
            _balance = Math.Round(_balance - _stake + _currentWin, 2 , MidpointRounding.AwayFromZero);

            var dto = new SpinResultDto()
            {
                Date = DateTime.Now,
                Balance = _balance,
                Stake = _stake,
                Win = _currentWin,
                Dimensions = dimmensions.AsQueryable().ProjectTo<SpinRowResultDto>()
            };

            //var dto = mapper.Map<SpinResultDto>(spin);
            AddToHistory(dto);
            return (dto, true, "Spinned");
        }

        public void Reset()
        {
            _balance = 0;
            _stake = 0;
            _currentWin = 0;
            _spinHistory = new List<SpinResultDto>();
        }
    }
}
