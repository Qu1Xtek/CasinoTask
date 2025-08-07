using CasinoBetty.Models;

namespace CasinoBetty.Commands.Interfaces
{
    public interface ICasinoCommand
    {
        CasinoResult Execute(decimal param, decimal currentBalance);
    }
}
