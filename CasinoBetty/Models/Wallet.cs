namespace CasinoBetty.Models
{
    public class Wallet
    {
        public decimal Balance { get; private set; }

        public void AddBalance(decimal amount)
        {
            Balance += amount;
        }
    }
}
