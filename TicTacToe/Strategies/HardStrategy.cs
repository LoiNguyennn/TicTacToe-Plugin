namespace TicTacToe
{
    public class HardStrategy : IComputerStrategy
    {
        public (int row, int col) GetNextMove(string[,] board, string computerPlayer)
        {
            int bestScore = int.MinValue;
            (int row, int col) bestMove = (-1, -1);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (string.IsNullOrEmpty(board[i, j]))
                    {
                        board[i, j] = computerPlayer;
                        int score = MiniMax(board, 0, false, computerPlayer);
                        board[i, j] = "";
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = (i, j);
                        }
                    }
                }
            }
            return bestMove;
        }

        private int MiniMax(string[,] board, int depth, bool isMaximizing, string computerPlayer)
        {
            string opponent = computerPlayer == "X" ? "O" : "X";
            if (CheckWin(board, computerPlayer)) return 10 - depth;
            if (CheckWin(board, opponent)) return depth - 10;
            if (IsBoardFull(board)) return 0;

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (string.IsNullOrEmpty(board[i, j]))
                        {
                            board[i, j] = computerPlayer;
                            int score = MiniMax(board, depth + 1, false, computerPlayer);
                            board[i, j] = "";
                            bestScore = System.Math.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (string.IsNullOrEmpty(board[i, j]))
                        {
                            board[i, j] = opponent;
                            int score = MiniMax(board, depth + 1, true, computerPlayer);
                            board[i, j] = "";
                            bestScore = System.Math.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
        }

        private bool CheckWin(string[,] board, string player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) return true;
                if (board[0, i] == player && board[1, i] == player && board[2, i] == player) return true;
            }
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) return true;
            return false;
        }

        private bool IsBoardFull(string[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (string.IsNullOrEmpty(board[i, j])) return false;
                }
            }
            return true;
        }
    }
}