using System;
using MonteCarloTreeSearch.Game;
using MonteCarloTreeSearch.Reversi;

namespace MonteCarloTreeSearch.CUI
{
    internal class ConsoleReversiPlayer : IPlayer<ReversiGameState>
    {
        public ConsoleReversiPlayer(ReversiGame game) { Game = game; }
        private ReversiGame Game { get; }

        public IMove<ReversiGameState> GetNextMove()
        {
            while (true)
            {
                var moveRepr = Console.ReadLine();
                if (moveRepr == null || moveRepr.Length != 2)
                {
                    Console.WriteLine("Invalid move");
                    continue;
                }
                var y = moveRepr[0] - 'a';
                var x = Game.CurrentState.Board.GetLength(0) - (moveRepr[1] - '0');
                var move = new DiskSetUp(x, y);
                if (move.Make(Game.CurrentState) != null) return move;
                Console.WriteLine("Invalid move");
            }
        }
    }
}