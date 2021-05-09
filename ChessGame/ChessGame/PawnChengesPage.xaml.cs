using System.Windows;

namespace ChessGame
{
    public delegate void MessageForClosed(object sender, RoutedEventArgs e);
    /// <summary>
    /// Interaction logic for PawnChengesPage.xaml
    /// </summary>
    public partial class PawnChengesPage : Window
    {
        public string result;
        public event MessageForClosed MessageCloseAndChange;
        public PawnChengesPage()
        {
            InitializeComponent();
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBishop.IsChecked == true)
            {
                result = "Bishop";
            }
            else if(CheckKnight.IsChecked == true)
            {
                result = "Knight";
            }
            else if (CheckQuuen.IsChecked == true)
            {
                result = "Queen";
            }
            else if (CheckRook.IsChecked == true)
            {
                result = "Rook";
            }
            else
            {
                result = string.Empty;
            }
            MessageCloseAndChange(this, e);
            this.Visibility = Visibility.Hidden;
        }
    }
}
