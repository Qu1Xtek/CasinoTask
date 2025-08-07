using CasinoBetty.Models;

namespace CasinoBetty.Tests.Models
{
    public class WalletTests
    {
        [Fact]
        public void Wallet_ShouldStartWithZeroBalance()
        {
            var wallet = new Wallet();
            Assert.Equal(0, wallet.Balance);
        }

        [Theory]
        [InlineData(100)]
        public void AddBalance_ShouldIncreaseBalance_WhenPositiveAmount(decimal amount)
        {
            var wallet = new Wallet();
            wallet.AddBalance(amount);
            Assert.Equal(amount, wallet.Balance);
        }

        [Fact]
        public void AddBalance_ShouldAccumulateBalance_WhenCalledMultipleTimes()
        {
            var wallet = new Wallet();

            wallet.AddBalance(100);
            wallet.AddBalance(50);
            wallet.AddBalance(-25);

            Assert.Equal(125, wallet.Balance);
        }
    }
}