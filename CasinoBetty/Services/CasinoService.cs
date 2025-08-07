using CasinoBetty.Commands.Interfaces;
using CasinoBetty.Models;
using CasinoBetty.Services.Interfaces;

namespace CasinoBetty.Services
{
    public class CasinoService : ICasinoService
    {
        private Wallet _wallet;
        private Dictionary<string, ICasinoCommand> _commands;

        public CasinoService(
            ICasinoCommand depositCommand,
            ICasinoCommand betCommand,
            ICasinoCommand withdrawCommand)
        {
            _wallet = new Wallet();

            _commands = new Dictionary<string, ICasinoCommand>()
            {
                { "deposit", depositCommand },
                { "bet", betCommand },
                { "withdraw", withdrawCommand },
            };
        }

        public decimal CheckBalance()
        {
            return _wallet.Balance;
        }

        public string Interact(string param)
        {
            var actions = param.Split(" ", 2);

            var commandName = actions[0].Trim().ToLower();

            if (_commands.ContainsKey(commandName)) 
            {
                if (actions.Length > 1 && decimal.TryParse(actions[1].Trim(), out decimal amount))
                {
                    var result = _commands[commandName].Execute(amount, _wallet.Balance);

                    return ProcessResult(result);
                }
                else
                {
                    return ("Invalid argument");
                }
            }
            else
            {
                return "Unrecognized command";
            }
        }

        private string ProcessResult(CasinoResult result)
        {
            if (result.BalanceUpdateValue != 0)
            {
                _wallet.AddBalance(result.BalanceUpdateValue);
                result.Details += $" Your new balance is ${_wallet.Balance}";
            }

            return result.Details;
        }
    }
}
