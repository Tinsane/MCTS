using System;
using MonteCarloTreeSearch.Game;
using MonteCarloTreeSearch.Reversi;

namespace MonteCarloTreeSearch.CUI
{
    public class ReversiView
    {
        public ReversiView(ReversiGame game, IPlayer<ReversiGameState>[] players)
        {
            Game = game;
            Players = players;
        }

        private ReversiGame Game { get; }

        private IPlayer<ReversiGameState>[] Players { get; }

        public void RunGame()
        {
            PrintCurrentState();
            while (!Game.IsEnded())
            {
                Game.TryMakeMove(Players[Game.CurrentPlayerId].GetNextMove());
                PrintCurrentState();
            }
        }

        private void PrintCurrentState()
        {
            //Console.Clear();
            var repr = ReversiRepresentor.GetGameRepresentation(Game);
            for (var x = 0; x < repr.Length; ++x)
                Console.WriteLine($"{repr.Length - x}  {repr[x]}");
            Console.WriteLine();
            Console.Write("   ");
            for (var a = 0; a < repr[0].Length; ++a)
                Console.Write((char)('a' + a));
            Console.WriteLine();
        }
    }
}