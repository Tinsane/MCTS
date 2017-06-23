namespace MonteCarloTreeSearch.Game
{
    public interface IPlayer<TGameState> where TGameState : IGameState
    {
        IMove<TGameState> GetNextMove();
    }
}