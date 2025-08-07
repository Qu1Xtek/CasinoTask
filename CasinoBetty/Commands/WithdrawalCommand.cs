using CasinoBetty.Commands.Interfaces;
using CasinoBetty.Models;

namespace CasinoBetty.Commands
{
    public class WithdrawalCommand : ICasinoCommand
    {
        public CasinoResult Execute(decimal param, decimal currentBalance)
        {
            if (param <= 0 || currentBalance < param)
            {
                return new CasinoResult
                {
                    Details = $"You can't withdraw more than current balance and withdrawal amount needs to be bigger than $0. Current balance is ${currentBalance}"
                };
            }
            else
            {
                return new CasinoResult
                {
                    Details = $"Your withdrawal of ${param} was successful.",
                    BalanceUpdateValue = -param
                };
            }
        }
    }
}
