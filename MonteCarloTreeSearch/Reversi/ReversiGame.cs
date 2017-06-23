using System.Collections.Generic;
using System.Linq;
using MonteCarloTreeSearch.Game;

namespace MonteCarloTreeSearch.Reversi
{
    public class ReversiGame : IBoardGame<ReversiGameState>
    {
        private readonly List<ReversiGameState> gameStates;

        public ReversiGame(ReversiGameState gameState) { gameStates = new List<ReversiGameState> {gameState}; }

        public ReversiGameState CurrentState => gameStates[gameStates.Count - 1];

        public bool TryMakeMove(IMove<ReversiGameState> move)
        {
            var newState = move.Make(CurrentState);
            if (newState == null)
                return false;
            gameStates.Add(newState);
            if (!GetPossibleMoves().Any())
                gameStates.Add(new ReversiGameState(newState.Board, CurrentPlayerId ^ 1));
            return true;
        }

        IBoardGame<ReversiGameState> IBoardGame<ReversiGameState>.GetSnapshot() => new ReversiGame(CurrentState);

        public IMove<ReversiGameState>[] GetPossibleMoves() => CurrentState.GetPossibleMoves();

        public int CurrentPlayerId => CurrentState.CurrentPlayerId;
    }
}