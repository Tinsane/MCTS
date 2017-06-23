namespace MonteCarloTreeSearch.Reversi
{
    public static class Utils
    {
        public const int BlackPlayerId = 0;
        public const int WhitePlayerId = 1;
        public const int BoardSize = 8;

        public static bool InHalfInterval(int x, int lBound, int rBound) => lBound <= x && x < rBound;

        public static Cell GetCellByPlayerId(int playerId) => playerId == BlackPlayerId ? Cell.Black : Cell.White;
    }
}