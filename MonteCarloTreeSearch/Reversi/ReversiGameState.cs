using System;
using System.Collections.Generic;
using System.Linq;
using MonteCarloTreeSearch.Game;

namespace MonteCarloTreeSearch.Reversi
{
    public class ReversiGameState : IGameState
    {
        private static readonly Dictionary<ReversiGameState, IMove<ReversiGameState>[]> PossibleMovesCache =
            new Dictionary<ReversiGameState, IMove<ReversiGameState>[]>();

        public readonly Cell[,] Board;
        private readonly ulong cellColorHash;

        private readonly ulong cellExistanceHash;

        private readonly int hashCode;

        public ReversiGameState(Cell[,] board, int currentPlayerId)
        {
            Board = board;
            CurrentPlayerId = currentPlayerId;
            cellExistanceHash = 0;
            cellColorHash = 0;
            for (var x = 0; x < board.GetLength(0); ++x)
            for (var y = 0; y < board.GetLength(1); ++y)
            {
                cellExistanceHash <<= 1;
                cellColorHash <<= 1;
                if (board[x, y] == Cell.Empty) continue;
                cellExistanceHash ^= 1;
                cellColorHash ^= (ulong) board[x, y];
            }
            unchecked
            {
                hashCode = (int) ((((cellExistanceHash * 217361) ^ cellColorHash) * 397) ^ (ulong) CurrentPlayerId);
            }
        }

        public Cell CurrentPlayerCell => Utils.GetCellByPlayerId(CurrentPlayerId);

        public Cell AnotherPlayerCell => Utils.GetCellByPlayerId(CurrentPlayerId ^ 1);

        public int CurrentPlayerId { get; }

        public bool IsTerminal() => !GetPossibleMoves().Any() &&
                                    !new ReversiGameState(Board, CurrentPlayerId ^ 1).GetPossibleMoves().Any();

        public int GetWinnerId()
        {
            switch (Math.Sign(GetDisksBalance()))
            {
                case 0:
                    return -1;
                case 1:
                    return Utils.WhitePlayerId;
                case -1:
                    return Utils.BlackPlayerId;
            }
            throw new InvalidOperationException("Invalid sign.");
        }

        public int GetDisksBalance()
        {
            var whiteDisksCnt = 0;
            var blackDisksCnt = 0;
            foreach (var cell in Board)
                if (cell == Cell.White)
                    ++whiteDisksCnt;
                else if (cell == Cell.Black)
                    ++blackDisksCnt;
            return whiteDisksCnt - blackDisksCnt;
        }

        public bool InBoard(int x, int y) => Utils.InHalfInterval(x, 0, Board.GetLength(0)) &&
                                             Utils.InHalfInterval(y, 0, Board.GetLength(1));

        internal bool InBoard(Point2D position) => InBoard(position.X, position.Y);

        public IMove<ReversiGameState>[] GetPossibleMoves()
        {
            if (!PossibleMovesCache.ContainsKey(this))
                PossibleMovesCache[this] = GeneratePossibleMoves();
            return PossibleMovesCache[this];
        }

        private IMove<ReversiGameState>[] GeneratePossibleMoves()
        {
            var possibleMoves = new List<IMove<ReversiGameState>>();
            for (var x = 0; x < Board.GetLength(0); ++x)
            for (var y = 0; y < Board.GetLength(1); ++y)
            {
                if (Board[x, y] != Cell.Empty)
                    continue;
                foreach (var direction in DiskSetUp.FlipDirections)
                {
                    var inititalPosition = new Point2D(x, y) + direction;
                    var currentPosition = inititalPosition;
                    while (InBoard(currentPosition) && Board[currentPosition.X, currentPosition.Y] == AnotherPlayerCell)
                        currentPosition += direction;
                    if (!InBoard(currentPosition) || Board[currentPosition.X, currentPosition.Y] != CurrentPlayerCell ||
                        currentPosition.Equals(inititalPosition)) continue;
                    possibleMoves.Add(new DiskSetUp(x, y));
                    break;
                }
            }
            return possibleMoves.ToArray();
        }

        public bool Equals(ReversiGameState other)
            => CurrentPlayerId == other.CurrentPlayerId &&
               cellExistanceHash == other.cellExistanceHash &&
               cellColorHash == other.cellColorHash;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ReversiGameState) obj);
        }

        public override int GetHashCode() => hashCode;
    }
}