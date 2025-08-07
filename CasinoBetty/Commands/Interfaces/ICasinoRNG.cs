namespace CasinoBetty.Commands.Interfaces
{
    public interface ICasinoRNG
    {
        bool RollOnBet();

        bool RollForMaxBet();

        int RollMaxBetMultiplier();
    }
}
