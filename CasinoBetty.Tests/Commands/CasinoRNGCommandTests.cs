using CasinoBetty.Commands;

namespace CasinoBetty.Tests.Commands
{
    public class CasinoRNGCommandTests
    {

        [Fact]
        public void RollMaxBetMultiplier_ShouldReturnValueBetween2And10()
        {
            var rng = new CasinoRNGCommand();

            for (int i = 0; i < 100; i++)
            {
                var result = rng.RollMaxBetMultiplier();
                Assert.InRange(result, 2, 10);
            }
        }

        [Fact]
        public void RollOnBet_ShouldReturnBothTrueAndFalse_OverMultipleRuns()
        {
            var rng = new CasinoRNGCommand();
            var trueCount = 0;
            var falseCount = 0;
            var iterations = 1000;

            for (int i = 0; i < iterations; i++)
            {
                if (rng.RollOnBet())
                    trueCount++;
                else
                    falseCount++;
            }

            Assert.True(trueCount > 0, "RollOnBet should return true sometimes");
            Assert.True(falseCount > 0, "RollOnBet should return false sometimes");


            // This test is not mandatory, the above test is more than enough to cover teh case.
            // I did this out of curiousity to check if I can hit an edge case,
            // let me know if you ahve any questions about it
            var truePercentage = (double)trueCount / iterations;
            Assert.InRange(truePercentage, 0.4, 0.6);
        }
    }
}