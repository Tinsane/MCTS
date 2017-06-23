namespace MonteCarloTreeSearch.MCTS
{
    public struct StatisticsRecord
    {
        public StatisticsRecord(int playsCount, int winsCount)
        {
            PlaysCount = playsCount;
            WinsCount = winsCount;
        }

        public readonly int PlaysCount;
        public readonly int WinsCount;

        public double WinnnigRate => (double) WinsCount / PlaysCount;
        public StatisticsRecord WithLoose => new StatisticsRecord(PlaysCount + 1, WinsCount);
        public StatisticsRecord WithWin => new StatisticsRecord(PlaysCount + 1, WinsCount + 1);

        public override string ToString() => $"({PlaysCount}, {WinsCount})";
    }
}