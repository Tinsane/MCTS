using System;
using System.Collections.Generic;
using System.Linq;
using MonteCarloTreeSearch.Game;

namespace MonteCarloTreeSearch.MCTS
{
    // ReSharper disable once InconsistentNaming
    public class MCTSPlayer<TGameState> : IPlayer<TGameState> where TGameState : IGameState
    {
        private readonly Random random = new Random();

        public MCTSPlayer(IBoardGame<TGameState> game, double confidenceCoef, TimeSpan simulationTimeLimit)
        {
            Game = game;
            ConfidenceCoef = confidenceCoef;
            SimulationTimeLimit = simulationTimeLimit;
            Statistics = new Dictionary<TGameState, StatisticsRecord>();
        }

        private double ConfidenceCoef { get; }

        private TimeSpan SimulationTimeLimit { get; }

        private Dictionary<TGameState, StatisticsRecord> Statistics { get; }

        private IBoardGame<TGameState> Game { get; }

        public IMove<TGameState> GetNextMove()
        {
            if (Game.IsEnded())
                throw new InvalidOperationException("Game has already ended.");
            var simulationBegin = DateTime.Now;
            var runsCount = 0;
            while (DateTime.Now - simulationBegin < SimulationTimeLimit)
            {
                RunSimulation();
                ++runsCount;
            }
            Console.WriteLine(runsCount);
            return SelectBestMove(Game.GetPossibleMoves(), move => GetWinningRate(move.Make(Game.CurrentState)), true);
        }

        private double GetWinningRate(TGameState state)
            => !Statistics.ContainsKey(state) ? -1 : Statistics[state].WinnnigRate;

        private IMove<TGameState> SelectBestMove(IEnumerable<IMove<TGameState>> possibleMoves,
            Func<IMove<TGameState>, double> estimate, bool verbose = false) // TODO
        {
            var bestEstimation = double.MinValue;
            IMove<TGameState> bestMove = null;
            foreach (var move in possibleMoves)
            {
                var estimation = estimate(move);
                if (estimation < bestEstimation) continue;
                bestEstimation = estimation;
                bestMove = move;
            }
            if (verbose)
                foreach (var move in possibleMoves.OrderByDescending(estimate))
                {
                    var stat = Statistics[move.Make(Game.CurrentState)];
                    Console.WriteLine($"{stat} : {stat.WinnnigRate}");
                }
            return bestMove;
        }

        private void UpdateStatistics(IEnumerable<TGameState> visitedStates, int winnerId)
        {
            foreach (var gameState in visitedStates.Where(Statistics.ContainsKey))
                Statistics[gameState] = gameState.CurrentPlayerId == winnerId
                    ? Statistics[gameState].WithWin
                    : Statistics[gameState].WithLoose;
        }

        // ReSharper disable once InconsistentNaming
        private double CalcMCTSEstimation(int totalPlaysCount, TGameState newState)
            => Statistics[newState].WinnnigRate + ConfidenceCoef *
               Math.Sqrt(Math.Log(totalPlaysCount) / Statistics[newState].PlaysCount);

        private IMove<TGameState> SelectMove(IMove<TGameState>[] possibleMoves, TGameState currentState)
        {
            var totalPlaysCount = 0;
            foreach (var state in possibleMoves.Select(move => move.Make(currentState)))
            {
                if (!Statistics.ContainsKey(state))
                    return possibleMoves[random.Next(possibleMoves.Length)];
                totalPlaysCount += Statistics[state].PlaysCount;
            }
            return SelectBestMove(possibleMoves, move => CalcMCTSEstimation(totalPlaysCount, move.Make(currentState)));
        }

        private void RunSimulation()
        {
            var snapshotGame = Game.GetSnapshot();
            var visitedStates = new List<TGameState> {snapshotGame.CurrentState};

            if (!Statistics.ContainsKey(snapshotGame.CurrentState))
                Statistics[snapshotGame.CurrentState] = new StatisticsRecord();

            var expand = true;
            while (!snapshotGame.IsEnded())
            {
                if (!snapshotGame.TryMakeMove(SelectMove(
                    snapshotGame.GetPossibleMoves(),
                    snapshotGame.CurrentState)))
                    throw new InvalidOperationException("Invalid move.");

                if (expand && !Statistics.ContainsKey(snapshotGame.CurrentState))
                {
                    expand = false;
                    Statistics[snapshotGame.CurrentState] = new StatisticsRecord();
                }

                visitedStates.Add(snapshotGame.CurrentState);
            }
            UpdateStatistics(visitedStates, snapshotGame.GetWinnerId());
        }
    }
}