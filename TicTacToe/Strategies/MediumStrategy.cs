using System;

namespace TicTacToe
{
    public class MediumStrategy : IComputerStrategy
    {
        private Random random = new Random();
        private HardStrategy hardStrategy = new HardStrategy();

        public (int row, int col) GetNextMove(string[,] board, string computerPlayer)
        {
            if (random.NextDouble() < 0.5)
            {
                return hardStrategy.GetNextMove(board, computerPlayer);
            }
            else
            {
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
    }
}