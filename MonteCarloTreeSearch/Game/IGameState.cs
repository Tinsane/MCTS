namespace MonteCarloTreeSearch.Game
{
    public interface IGameState
    {
        int CurrentPlayerId { get; }
        bool IsTerminal();
        int GetWinnerId();
    }
}