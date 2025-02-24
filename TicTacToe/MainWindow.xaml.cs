using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;

namespace TicTacToe
{
    public sealed partial class MainWindow : Window
    {
        private string currentPlayer = "X";
        private string[,] board = new string[3, 3];
        private bool gameEnded = false;

        public MainWindow()
        {
            this.InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = "";
                }
            }

            var buttons = GameBoard.Children.Cast<Button>();
            foreach (Button button in buttons)
            {
                button.Content = "";
                button.IsEnabled = true;
            }

            currentPlayer = "X";
            StatusText.Text = "Player X's Turn";
            gameEnded = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameEnded) return;

            Button button = (Button)sender;
            string buttonName = button.Name;
            int row = int.Parse(buttonName.Split('_')[1]);
            int col = int.Parse(buttonName.Split('_')[2]);

            if (string.IsNullOrEmpty(board[row, col]))
            {
                board[row, col] = currentPlayer;
                button.Content = currentPlayer;
                button.IsEnabled = false;

                if (CheckWinner(row, col))
                {
                    StatusText.Text = $"Player {currentPlayer} Wins!";
                    gameEnded = true;
                    DisableAllButtons();
                }
                else if (IsBoardFull())
                {
                    StatusText.Text = "It's a Draw!";
                    gameEnded = true;
                }
                else
                {
                    currentPlayer = currentPlayer == "X" ? "O" : "X";
                    StatusText.Text = $"Player {currentPlayer}'s Turn";
                }
            }
        }

        private bool CheckWinner(int row, int col)
        {
            // Check row
            if (board[row, 0] == currentPlayer &&
                board[row, 1] == currentPlayer &&
                board[row, 2] == currentPlayer)
                return true;

            // Check column
            if (board[0, col] == currentPlayer &&
                board[1, col] == currentPlayer &&
                board[2, col] == currentPlayer)
                return true;

            // Check diagonals
            if (row == col || row + col == 2)
            {
                // Main diagonal
                if (board[0, 0] == currentPlayer &&
                    board[1, 1] == currentPlayer &&
                    board[2, 2] == currentPlayer)
                    return true;

                // Anti-diagonal
                if (board[0, 2] == currentPlayer &&
                    board[1, 1] == currentPlayer &&
                    board[2, 0] == currentPlayer)
                    return true;
            }

            return false;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (string.IsNullOrEmpty(board[i, j]))
                        return false;
                }
            }
            return true;
        }

        private void DisableAllButtons()
        {
            var buttons = GameBoard.Children.Cast<Button>();
            foreach (Button button in buttons)
            {
                button.IsEnabled = false;
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            InitializeBoard();
        }
    }
}