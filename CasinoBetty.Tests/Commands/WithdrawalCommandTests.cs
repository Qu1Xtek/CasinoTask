using CasinoBetty.Commands;

namespace CasinoBetty.Tests.Commands
{
    public class WithdrawalCommandTests
    {
        [Theory]
        [InlineData(50, 100)]
        [InlineData(100, 100)]
        [InlineData(25.75, 50)]
        public void Execute_ShouldReturnSuccessResult_WhenValidAmount(decimal withdrawAmount, decimal currentBalance)
        {
            var command = new WithdrawalCommand();

            var result = command.Execute(withdrawAmount, currentBalance);

            Assert.Equal(-withdrawAmount, result.BalanceUpdateValue);
            Assert.Contains("successful", result.Details);
            Assert.Contains($"${withdrawAmount}", result.Details);
        }

        [Theory]
        [InlineData(150, 100)]
        [InlineData(50, 25)]
        public void Execute_ShouldReturnErrorResult_WhenInsufficientFunds(decimal withdrawAmount, decimal currentBalance)
        {
            var command = new WithdrawalCommand();

            var result = command.Execute(withdrawAmount, currentBalance);

            Assert.Equal(0, result.BalanceUpdateValue);
            Assert.Contains("can't withdraw more", result.Details);
            Assert.Contains($"${currentBalance}", result.Details);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(-50, 100)]
        public void Execute_ShouldReturnErrorResult_WhenInvalidAmount(decimal invalidAmount, decimal currentBalance)
        {
            var command = new WithdrawalCommand();

            var result = command.Execute(invalidAmount, currentBalance);

            Assert.Equal(0, result.BalanceUpdateValue);
            Assert.Contains("bigger than $0", result.Details);
        }
    }
}