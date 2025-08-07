using CasinoBetty.Models;

namespace CasinoBetty.Services.Interfaces
{
    public interface ICasinoService
    {
        public string Interact(string command);

        public decimal CheckBalance();
    }
}
