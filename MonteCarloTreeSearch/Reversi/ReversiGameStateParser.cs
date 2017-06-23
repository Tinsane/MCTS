using System;

namespace MonteCarloTreeSearch.Reversi
{
    public static class ReversiGameStateParser
    {
        public static ReversiGameState Parse(string[] board, int currentPlayerId)
        {
            var gameBoard = new Cell[board.Length, board[0].Length];
            for(var x = 0; x < board.Length; ++x)
                for(var y = 0; y < board[0].Length; ++y)
                    switch (board[x][y])
                    {
                        case '.':
                            gameBoard[x, y] = Cell.Empty;
                            break;
                        case 'W':
                            gameBoard[x, y] = Cell.White;
                            break;
                        case 'B':
                            gameBoard[x, y] = Cell.Black;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(board) + $"[{x}][{y}]");
                    }
            return new ReversiGameState(gameBoard, currentPlayerId);
        }
    }
}