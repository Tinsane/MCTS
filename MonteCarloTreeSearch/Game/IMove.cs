namespace MonteCarloTreeSearch.Game
{
    public interface IMove<TGameState> where TGameState : IGameState
    {
        TGameState Make(TGameState gameState);
    }
}