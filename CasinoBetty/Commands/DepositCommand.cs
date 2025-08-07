using CasinoBetty.Commands.Interfaces;
using CasinoBetty.Models;
using System.Runtime.InteropServices;

namespace CasinoBetty.Commands
{
    public class DepositCommand : ICasinoCommand
    {
        public CasinoResult Execute(decimal param, decimal currentBalance)
        {
            var result = new CasinoResult();

            if (param <= 0)
            {
                result.Details = "You need to deposit more than $0";
                return result;
            }

            result.BalanceUpdateValue = param;

            result.Details = $"Your deposit of ${param} was successful.";

            return result;
        }
    }
}
