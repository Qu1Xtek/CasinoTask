using CasinoBetty.Commands.Interfaces;
using CasinoBetty.Models;

namespace CasinoBetty.Commands
{
    public class BetCommand : ICasinoCommand
    {
        private ICasinoRNG _casinoRng;

        public BetCommand(ICasinoRNG rng)
        {
            _casinoRng = rng;
        }

        public CasinoResult Execute(decimal param, decimal currentBalance)
        {                            
            return MakeABet(param, currentBalance);            
        }

        private CasinoResult PerformBet(decimal param, decimal currentBalance)
        {
            var result = new CasinoResult();

            result.BalanceUpdateValue = -param;

            if (_casinoRng.RollOnBet())
            {
                decimal winAmount = 0;
                string message = "Congrats - you won $amount!";
                if (_casinoRng.RollForMaxBet())
                {
                    var multiplier = _casinoRng.RollMaxBetMultiplier();
                    winAmount += param * multiplier;
                }
                else
                {
                    winAmount += param * 2;
                }

                result.BalanceUpdateValue += winAmount;
                result.Details = message.Replace("amount", winAmount.ToString());
            }
            else
            {
                result.Details = "No luck this time!";
            }

            return result;
        }

        private CasinoResult MakeABet(decimal betAmount, decimal currentBalance)
        {
            var result = new CasinoResult();

            if (betAmount < 1 || betAmount > 10)
            {
                result.Details = "Bet amount needs to be between $1 and $10.";
                return result;
            }

            if (currentBalance < betAmount)
            {
                result.Details = "Insufficient funds, lower your bet amount or make another deposit";
                return result;
            }

            return PerformBet(betAmount, currentBalance);
        }
    }
}
