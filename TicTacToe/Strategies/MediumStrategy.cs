using System;

namespace TicTacToe
{
    public class MediumStrategy : IComputerStrategy
    {
        private Random random = new Random();
        private HardStrategy hardStrategy = new HardStrategy();
        private int totalGames = 0;
        private int playerWins = 0;

        // Call this method after each game ends to update stats
        public void RecordGameResult(bool playerWon)
        {
            totalGames++;
            if (playerWon) playerWins++;
        }

        // Calculate player's win rate
        public double GetPlayerWinRate()
        {
            if (totalGames == 0) return 0.0;
            return (double)playerWins / totalGames;
        }

        public (int row, int col) GetNextMove(string[,] board, string computerPlayer)
        {
            // If player's win rate is above 49%, use hard strategy
            if (GetPlayerWinRate() > 0.49)
            {
                return hardStrategy.GetNextMove(board, computerPlayer);
            }
            else
            {
                // Easy strategy: random move
                var emptyCells = new System.Collections.Generic.List<(int row, int col)>();
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (string.IsNullOrEmpty(board[i, j]))
                        {
                            emptyCells.Add((i, j));
                        }
                    }
                }
                return emptyCells[random.Next(emptyCells.Count)];
            }
        }

        // Optional: Reset stats for a new session
        public void ResetStats()
        {
            totalGames = 0;
            playerWins = 0;
        }
    }
}