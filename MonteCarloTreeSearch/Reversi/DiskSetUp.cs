using System;
using System.Collections.Generic;
using MonteCarloTreeSearch.Game;

namespace MonteCarloTreeSearch.Reversi
{
    internal class DiskSetUp : IMove<ReversiGameState>
    {
        public static readonly List<Point2D> FlipDirections = new List<Point2D>
        {
            new Point2D(1, 1),
            new Point2D(1, 0),
            new Point2D(1, -1),
            new Point2D(0, -1),
            new Point2D(-1, -1),
            new Point2D(-1, 0),
            new Point2D(-1, 1),
            new Point2D(0, 1)
        };

        private readonly int x;
        private readonly int y;

        public DiskSetUp(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public DiskSetUp(Point2D position) : this(position.X, position.Y) { }

        public ReversiGameState Make(ReversiGameState reversiGameState)
        {
            if (!Check(reversiGameState))
                return null;
            var resultGameState = Apply(reversiGameState);
            return !IsMoveCorrect(reversiGameState, resultGameState) ? null : resultGameState;
        }

        private bool IsMoveCorrect(ReversiGameState oldState, ReversiGameState newState)
        {
            foreach (var direction in FlipDirections)
            {
                var newX = x + direction.X;
                var newY = y + direction.Y;
                if (oldState.InBoard(newX, newY) && oldState.Board[newX, newY] != newState.Board[newX, newY])
                    return true;
            }
            return false;
        }

        public IEnumerable<Point2D> GetFlippedCells(ReversiGameState reversiGameState)
        {
            foreach (var direction in FlipDirections)
            {
                var endPosition = new Point2D(x, y) + direction;
                while (reversiGameState.InBoard(endPosition) &&
                       reversiGameState.Board[endPosition.X, endPosition.Y] == reversiGameState.AnotherPlayerCell)
                    endPosition += direction;
                if (!reversiGameState.InBoard(endPosition) ||
                    reversiGameState.Board[endPosition.X, endPosition.Y] == Cell.Empty)
                    continue;
                for (var position = new Point2D(x, y) + direction; !position.Equals(endPosition); position += direction)
                    yield return position;
            }
        }

        private bool Check(ReversiGameState reversiGameState)
        {
            var board = reversiGameState.Board;
            return reversiGameState.InBoard(x, y) &&
                   board[x, y] == Cell.Empty;
        }

        private ReversiGameState Apply(ReversiGameState reversiGameState)
        {
            var board = new Cell[reversiGameState.Board.GetLength(0), reversiGameState.Board.GetLength(1)];
            Array.Copy(reversiGameState.Board, board, reversiGameState.Board.Length);
            board[x, y] = reversiGameState.CurrentPlayerCell;
            foreach (var position in GetFlippedCells(reversiGameState))
                board[position.X, position.Y] = reversiGameState.CurrentPlayerCell;
            var resultGameState = new ReversiGameState(board, reversiGameState.CurrentPlayerId ^ 1);
            return resultGameState;
            //return !resultGameState.GetPossibleMoves().Any()
            //    ? new ReversiGameState(board, reversiGameState.CurrentPlayerId)
            //    : resultGameState;
        }

        public override string ToString() => $"DiskSetUp({x}, {y})";
    }
}