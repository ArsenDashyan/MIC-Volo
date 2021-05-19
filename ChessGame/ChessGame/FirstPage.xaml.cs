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
                MainWindow.CurrentGameStatus = 1;
                var mainWindow = new MainWindow();
                mainWindow.ShowKingGamePanel();
                mainWindow.Show();
                this.Close();
            }
            if (CheckKnight.IsChecked == true)
            {
                MainWindow.CurrentGameStatus = 2;
                var mainWindow = new MainWindow();
                mainWindow.ShowKnightGamePanel();
                mainWindow.Show();
                this.Close();
            }
            if (StandardGame.IsChecked == true)
            {
                MainWindow.CurrentGameStatus = 3;
                var mainWindow = new MainWindow();
                mainWindow.ShowStandardGamePanel();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
