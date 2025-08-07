using CasinoBetty.Commands;
using CasinoBetty.Commands.Interfaces;
using Moq;

namespace CasinoBetty.Tests.Commands
{
    public class BetCommandTests
    {
        [Theory]
        [InlineData(0.5)] // Below minimum
        [InlineData(11)]  // Above maximum
        [InlineData(15)]
        public void Execute_ShouldReturnErrorResult_WhenBetOutOfRange(decimal betAmount)
        {
            var mockRng = new Mock<ICasinoRNG>();
            var command = new BetCommand(mockRng.Object);

            var result = command.Execute(betAmount, 100);

            Assert.Equal(0, result.BalanceUpdateValue);
            Assert.Contains("between", result.Details);
        }

        [Theory]
        [InlineData(5, 3)]
        public void Execute_ShouldReturnErrorResult_WhenInsufficientFunds(decimal betAmount, decimal currentBalance)
        {
            var mockRng = new Mock<ICasinoRNG>();
            var command = new BetCommand(mockRng.Object);
         
            var result = command.Execute(betAmount, currentBalance);
            
            Assert.Equal(0, result.BalanceUpdateValue);
            Assert.Contains("Insufficient funds", result.Details);
        }

        [Theory]
        [InlineData(1)]
        public void Execute_ShouldReturnLossResult_WhenRngReturnsLoss(decimal betAmount)
        {            
            var mockRng = new Mock<ICasinoRNG>();
            mockRng.Setup(x => x.RollOnBet()).Returns(false); // Not winning 
            var command = new BetCommand(mockRng.Object);

            var result = command.Execute(betAmount, 100);

            Assert.Equal(-betAmount, result.BalanceUpdateValue);
            Assert.Contains("No luck", result.Details);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void Execute_ShouldReturnRegularWinResult_WhenRngReturnsRegularWin(decimal betAmount)
        {
            var mockRng = new Mock<ICasinoRNG>();
            mockRng.Setup(x => x.RollOnBet()).Returns(true);
            mockRng.Setup(x => x.RollForMaxBet()).Returns(false); // make it only possible for regular win (2x)
            var command = new BetCommand(mockRng.Object);

            var result = command.Execute(betAmount, 100);

            var expectedWin = betAmount * 2;
            var expectedBalanceUpd = -betAmount + expectedWin; 

            Assert.Equal(expectedBalanceUpd, result.BalanceUpdateValue);
            Assert.Contains("Congrats", result.Details);
            Assert.Contains($"{expectedWin}", result.Details);
        }

        [Theory]
        [InlineData(5, 3)]
        [InlineData(2, 7)]
        [InlineData(10, 5)]
        public void Execute_ShouldReturnMaxWinResult_WhenRngReturnsMaxWin(decimal betAmount, int multiplier)
        {
            var mockRng = new Mock<ICasinoRNG>();

            mockRng.Setup(x => x.RollOnBet()).Returns(true);
            mockRng.Setup(x => x.RollForMaxBet()).Returns(true); // make it possible to win x(2-10)
            mockRng.Setup(x => x.RollMaxBetMultiplier()).Returns(multiplier);

            var command = new BetCommand(mockRng.Object);

            var result = command.Execute(betAmount, 100);
            
            var expectedWin = betAmount * multiplier;
            var expectedBalance = -betAmount + expectedWin;

            Assert.Equal(expectedBalance, result.BalanceUpdateValue);
            Assert.Contains("Congrats", result.Details);
            Assert.Contains($"{expectedWin}", result.Details);
        }
    }
}