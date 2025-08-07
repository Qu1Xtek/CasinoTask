using CasinoBetty.Commands;

namespace CasinoBetty.Tests.Commands
{
    public class DepositCommandTests
    {
        [Theory]
        [InlineData(100, 50)]
        [InlineData(25.75, 0)]
        [InlineData(1, 999)]
        public void Execute_ShouldReturnSuccessResult_WhenValidAmount(decimal depositAmount, decimal currentBalance)
        {
            var command = new DepositCommand();

            var result = command.Execute(depositAmount, currentBalance);

            Assert.Equal(depositAmount, result.BalanceUpdateValue);
            Assert.Contains("successful", result.Details);
            Assert.Contains($"${depositAmount}", result.Details);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Execute_ShouldReturnErrorResult_WhenInvalidAmount(decimal invalidAmount)
        {
            var command = new DepositCommand();

            var result = command.Execute(invalidAmount, 100);

            Assert.Equal(0, result.BalanceUpdateValue);
            Assert.Contains("more than $0", result.Details);
        }
    }
}