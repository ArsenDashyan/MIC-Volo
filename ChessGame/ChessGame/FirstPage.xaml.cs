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
                MainWindow.currentGameStatus = 1;
                var mainWindow = new MainWindow();
                mainWindow.ShowKingGamePanel();
                mainWindow.Show();
                this.Close();
            }
            if (CheckKnight.IsChecked == true)
            {
                MainWindow.currentGameStatus = 2;
                var mainWindow = new MainWindow();
                mainWindow.ShowKnightGamePanel();
                mainWindow.Show();
                this.Close();
            }
            if (StandardGame.IsChecked == true)
            {
                MainWindow.currentGameStatus = 3;
                var mainWindow = new MainWindow();
                mainWindow.ShowStandardGamePanel();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
