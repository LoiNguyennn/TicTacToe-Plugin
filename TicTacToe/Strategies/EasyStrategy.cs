using System;

namespace TicTacToe
{
    public class EasyStrategy : IComputerStrategy
    {
        private Random random = new Random();

        public (int row, int col) GetNextMove(string[,] board, string computerPlayer)
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