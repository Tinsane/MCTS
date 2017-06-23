namespace MonteCarloTreeSearch.Game
{
    public static class BoardGameExtensions
    {
        public static bool IsEnded<TGameState>(this IBoardGame<TGameState> game)
            where TGameState : IGameState
            => game.CurrentState.IsTerminal();

        public static int GetWinnerId<TGameState>(this IBoardGame<TGameState> game)
            where TGameState : IGameState
            => game.CurrentState.GetWinnerId();
    }
}