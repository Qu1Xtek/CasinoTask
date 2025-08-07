using CasinoBetty.Commands;
using CasinoBetty.Commands.Interfaces;
using CasinoBetty.Services;
using Moq;

namespace CasinoBetty.Tests.Services
{
    public class CasinoServiceTests
    {
        [Fact]
        public void CasinoService_ShouldStartWithZeroBalance()
        {
            var service = new CasinoService(
                new DepositCommand(),
                new BetCommand(new CasinoRNGCommand()),
                new WithdrawalCommand()
            );

            Assert.Equal(0, service.CheckBalance()); // Should start with $0
            
            service.Interact("deposit 100");

            Assert.Equal(100, service.CheckBalance());
        }

        [Theory]
        [InlineData("deposit 50")]
        [InlineData("DEPoSIT 50")]
        public void Interact_ShouldBeCaseInsensitive(string command)
        {
            var service = new CasinoService(
                new DepositCommand(),
                new BetCommand(new CasinoRNGCommand()),
                new WithdrawalCommand()
            );

            var result = service.Interact(command);

            Assert.Contains("successful", result);
            Assert.Equal(50, service.CheckBalance());
        }

        [Theory]
        [InlineData("unknown 50")]
        [InlineData("asdstgre dfsdf")]
        [InlineData("")]
        public void Interact_ShouldReturnError_WhenUnrecognizedCommand(string command)
        {            
            var service = new CasinoService(
                new DepositCommand(),
                new BetCommand(new CasinoRNGCommand()),
                new WithdrawalCommand()
            );

            var result = service.Interact(command);

            Assert.Contains("Unrecognized command", result);
        }

        [Theory]
        [InlineData("deposit")]
        [InlineData("bet")]
        [InlineData("withdraw")]
        [InlineData("deposit abc")]
        [InlineData("bet xyz")]
        public void Interact_ShouldReturnError_WhenInvalidArgument(string command)
        {
            var service = new CasinoService(
                new DepositCommand(),
                new BetCommand(new CasinoRNGCommand()),
                new WithdrawalCommand()
            );
         
            var result = service.Interact(command);
            
            Assert.Contains("Invalid argument", result);
        }

        [Fact]
        public void Interact_ShouldMaintainBalance_ThroughMultipleOperations()
        {
            var mockRng = new Mock<ICasinoRNG>();

            mockRng.Setup(x => x.RollOnBet()).Returns(false);
            
            var service = new CasinoService(
                new DepositCommand(),
                new BetCommand(mockRng.Object),
                new WithdrawalCommand()
            );
           
            Assert.Equal(0, service.CheckBalance()); 

            service.Interact("deposit 100");
            Assert.Equal(100, service.CheckBalance());

            service.Interact("bet 10"); 
            Assert.Equal(90, service.CheckBalance());

            service.Interact("withdraw 20");
            Assert.Equal(70, service.CheckBalance());
        }

        [Fact]
        public void Interact_ShouldHandleBettingWinScenario()
        {
            var mockRng = new Mock<ICasinoRNG>();

            //Always win x2
            mockRng.Setup(x => x.RollOnBet()).Returns(true);
            mockRng.Setup(x => x.RollForMaxBet()).Returns(false); 
            
            var service = new CasinoService(
                new DepositCommand(),
                new BetCommand(mockRng.Object),
                new WithdrawalCommand()
            );

            service.Interact("deposit 100"); // Start with 100 $
            Assert.Equal(100, service.CheckBalance());
            
            var result = service.Interact("bet 5"); // Bet 5, win 10, net +5

            Assert.Contains("Congrats", result);
            Assert.Equal(105, service.CheckBalance()); // 100 - 5 + 10 = 105
        }
    }
}