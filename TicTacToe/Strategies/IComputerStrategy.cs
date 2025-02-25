namespace TicTacToe
{
    public interface IComputerStrategy
    {
        (int row, int col) GetNextMove(string[,] board, string computerPlayer);
    }
}