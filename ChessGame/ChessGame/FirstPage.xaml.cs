using System.Windows;

namespace ChessGame
{
    /// <summary>
    /// Interaction logic for FirstPage.xaml
    /// </summary>
    public partial class FirstPage : Window
    {
        public FirstPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckKing.IsChecked == true)
            {
                var mainWindow = new MainWindow();
                mainWindow.ShowKingGamePanel();
                mainWindow.Show();
                MainWindow.currentGameStatus = 1;
                this.Close();
            }
            if (CheckKnight.IsChecked == true)
            {
                var mainWindow = new MainWindow();
                mainWindow.ShowKnightGamePanel();
                mainWindow.Show();
                MainWindow.currentGameStatus = 2;
                this.Close();
            }
            if (StandardGame.IsChecked == true)
            {
                var mainWindow = new MainWindow();
                mainWindow.ShowStandardGamePanel();
                mainWindow.Show();
                mainWindow.SetAllFigures();
                MainWindow.currentGameStatus = 3;
                this.Close();
            }
        }
    }
}
