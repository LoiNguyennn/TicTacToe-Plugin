using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;
using System.Diagnostics;

namespace TicTacToe
{
    public sealed partial class MainWindow : Window
    {
        private string currentPlayer = "X";
        private string computerPlayer = "O";
        private string[,] board = new string[3, 3];
        private bool gameEnded = false;
        private bool isVsComputer = true; // Mặc định chơi với máy
        private IComputerStrategy computerStrategy;

        public MainWindow()
        {
            Debug.WriteLine("Starting MainWindow constructor");
            this.InitializeComponent();
            computerStrategy = new EasyStrategy(); // Khởi tạo mặc định
            Debug.WriteLine("MainWindow constructor finished");
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Grid Loaded");
            InitializeBoard();
            Debug.WriteLine("InitializeBoard finished");
        }

        private void InitializeBoard()
        {
            Debug.WriteLine("Initializing board");
            if (GameBoard == null)
            {
                Debug.WriteLine("GameBoard is still null!");
                return;
            }

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

            // Nếu máy đi trước (O), gọi ComputerMove ngay
            if (isVsComputer && currentPlayer == computerPlayer)
            {
                ComputerMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Button clicked");
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

                    if (isVsComputer && currentPlayer == computerPlayer && !gameEnded)
                    {
                        ComputerMove();
                    }
                }
            }
        }

        private void ComputerMove()
        {
            Debug.WriteLine("Computer move started");
            var (row, col) = computerStrategy.GetNextMove(board, computerPlayer);
            board[row, col] = computerPlayer;
            var button = (Button)GameBoard.Children.Cast<UIElement>()
                .First(b => b is Button btn && btn.Name == $"Button_{row}_{col}");
            button.Content = computerPlayer;
            button.IsEnabled = false;

            if (CheckWinner(row, col))
            {
                StatusText.Text = $"Player {computerPlayer} Wins!";
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
                currentPlayer = "X";
                StatusText.Text = "Player X's Turn";
            }
            Debug.WriteLine("Computer move finished");
        }

        private bool CheckWinner(int row, int col)
        {
            if (board[row, 0] == currentPlayer && board[row, 1] == currentPlayer && board[row, 2] == currentPlayer) return true;
            if (board[0, col] == currentPlayer && board[1, col] == currentPlayer && board[2, col] == currentPlayer) return true;
            if (row == col && board[0, 0] == currentPlayer && board[1, 1] == currentPlayer && board[2, 2] == currentPlayer) return true;
            if (row + col == 2 && board[0, 2] == currentPlayer && board[1, 1] == currentPlayer && board[2, 0] == currentPlayer) return true;
            return false;
        }

        private bool IsBoardFull()
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
            Debug.WriteLine("New Game clicked");
            isVsComputer = true; // Đảm bảo luôn chơi với máy
            InitializeBoard();
        }

        private void Difficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Difficulty changed");
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
            {
                switch (selectedItem.Content.ToString().ToLower())
                {
                    case "easy":
                        computerStrategy = new EasyStrategy();
                        break;
                    case "medium":
                        computerStrategy = new MediumStrategy();
                        break;
                    case "hard":
                        computerStrategy = new HardStrategy();
                        break;
                }
                InitializeBoard(); // Reset bảng khi đổi độ khó
            }
        }
    }
}