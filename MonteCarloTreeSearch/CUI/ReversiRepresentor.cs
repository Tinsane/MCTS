using System;
using MonteCarloTreeSearch.Reversi;

namespace MonteCarloTreeSearch.CUI
{
    public static class ReversiRepresentor
    {
        public static string[] GetGameRepresentation(ReversiGame game)
        {
            var board = game.CurrentState.Board;
            var representation = new string[board.GetLength(0)];
            for (var row = 0; row < board.GetLength(0); ++row)
            {
                representation[row] = "";
                for (var column = 0; column < board.GetLength(1); ++column)
                    switch (board[row, column])
                    {
                        case Cell.Black:
                            representation[row] += 'B';
                            break;
                        case Cell.White:
                            representation[row] += 'W';
                            break;
                        case Cell.Empty:
                            representation[row] += '.';
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
            return representation;
        }
    }
}