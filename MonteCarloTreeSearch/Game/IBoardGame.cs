using System.Collections.Generic;

namespace MonteCarloTreeSearch.Game
{
    public interface IBoardGame<TGameState> where TGameState : IGameState
    {
        TGameState CurrentState { get; }
        int CurrentPlayerId { get; }
        bool TryMakeMove(IMove<TGameState> move);
        IBoardGame<TGameState> GetSnapshot();
        IMove<TGameState>[] GetPossibleMoves();
    }
}