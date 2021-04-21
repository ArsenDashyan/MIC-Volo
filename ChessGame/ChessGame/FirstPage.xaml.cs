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
                MainWindow mainWindow = new MainWindow();
                mainWindow.Width = 1000;
                mainWindow.Show();
                mainWindow.gameManager = true;
                this.Close();
            }
            if (CheckKnight.IsChecked == true)
            {
                MainWindow mainWindow = new MainWindow();
                Thickness thickness = new Thickness(0);
                mainWindow.KnightPage.Margin = thickness;
                mainWindow.Width = 1000;
                mainWindow.Show();
                this.Close();
            }
            if (StandardGame.IsChecked == true)
            {
                var mainWindow = new MainWindow();
                mainWindow.Width = 620;
                mainWindow.Show();
                //Standard standard = new Standard(mainWindow.currentFigureColor);
                //mainWindow.models = standard.figures;
                //mainWindow.SetAllFigures(mainWindow.models);
                this.Close();
            }
        }
    }
}
