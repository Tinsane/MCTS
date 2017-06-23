using System;
using MonteCarloTreeSearch.CUI;
using MonteCarloTreeSearch.Game;
using MonteCarloTreeSearch.MCTS;
using MonteCarloTreeSearch.Reversi;

namespace MonteCarloTreeSearch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var game = new ReversiGame(ReversiGameStateParser.Parse(new[]
            {
                "........",
                "........",
                "........",
                "...BW...",
                "...WB...",
                "........",
                "........",
                "........"
            }, Utils.BlackPlayerId));
            //var player1 = new ConsoleReversiPlayer(game);
            //var player2 = new ConsoleReversiPlayer(game);
            var player1 = new MCTSPlayer<ReversiGameState>(game, 1, new TimeSpan(0, 0, 10));
            var player2 = new MCTSPlayer<ReversiGameState>(game, 2, new TimeSpan(0, 0, 10));
            var view = new ReversiView(game, new IPlayer<ReversiGameState>[] {player1, player2});
            view.RunGame();
        }
    }
}