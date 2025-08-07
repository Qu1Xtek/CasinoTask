using CasinoBetty.Commands.Interfaces;

namespace CasinoBetty.Commands
{
    public class CasinoRNGCommand : ICasinoRNG
    {
        private readonly Random _rng;

        public CasinoRNGCommand()
        {
            _rng = new Random();
        }

        public bool RollForMaxBet()
        {
            return _rng.Next(0, 100) < 20;
        }

        public int RollMaxBetMultiplier()
        {
            return _rng.Next(2, 11);
        }

        public bool RollOnBet()
        {
            return _rng.Next(0, 100) < 50;
        }
    }
}
