using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ChessGameLibrary;

namespace ChessGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

        }


        private void PleacementB1_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GetFigure());
            bitmap.EndInit();
            string[] tempFigure = GetFigure().Split('\\');
            string color = tempFigure[tempFigure.Length - 1].Split('.')[0];
            string figure = tempFigure[tempFigure.Length - 1].Split('.')[1];
            if (GetCoordinats(out Point point))
            {
                //Model model = new Model();
                //model = (Model)figure;
                var image = GetImage(point);
                image.Source = bitmap;
            }
        }

        //private Model GetModel(string str)
        //{
        //    Model model = str switch
        //    {
        //        //"Queen" => Queen,
        //        //"King" => King,
        //        //"Bishop" => Bishop,
        //        //"Rook" => Rook,
        //        //"Knight" => Knights,
        //        //"Pawn" => null,
        //        //_ => ""
        //    };
        //    return model;
        //}
        private string GetFigure()
        {
            string str = SelectFigur.Text;
            string result = string.Empty;
            if (CheckWhite.IsChecked == true)
            {
                result = WhiteFigurePath(str);
            }
            else if (CheckBlack.IsChecked == true)
            {
                result = BlackFigurePath(str);
            }
            return result;
        }
        private string WhiteFigurePath(string str)
        {
            string result = str switch
            {
                "Queen" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\White.Queen.png",
                "King" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\White.King.png",
                "Bishop" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\White.Bishop.png",
                "Rook" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\White.Rook.png",
                "Knight" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\White.Knight.png",
                "Pawn" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\White.Pawn.png",
                _ => ""
            };
            return result;
        }
        private string BlackFigurePath(string str)
        {
            string result = str switch
            {
                "Queen" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\Black.Queen.png",
                "King" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\Black.King.png",
                "Bishop" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\Black.Bishop.png",
                "Rook" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\Black.Rook.png",
                "Knight" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\Black.Knight.png",
                "Pawn" => @"C:\Users\arsen\source\repos\VoloMic\HomeWork\ChessGame\Picturs\Black.Pawn.png",
                _ => ""
            };
            return result;
        }
        private void QueenWhiteB2_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"C:\Users\arsen\OneDrive\Desktop\Queen.png");
            bitmap.EndInit();

            Image00.Source = bitmap;
            Bord.Children.IndexOf(Image00);
        }
        private bool GetCoordinats(out Point point)
        {
            string input = InputCoordinats.Text;
            int i = CharToInt(input[0]);
            int j = Convert.ToInt32(input[1].ToString());
            point = new Point(i, j - 1);
            return true;
        }
        public static int CharToInt(char ch)
        {
            switch (Char.ToLower(ch))
            {
                case 'a':
                    return 0;
                case 'b':
                    return 1;
                case 'c':
                    return 2;
                case 'd':
                    return 3;
                case 'e':
                    return 4;
                case 'f':
                    return 5;
                case 'g':
                    return 6;
                case 'h':
                    return 7;
            }
            return 0;
        }
        public Image GetImage(Point point)
        {
            string corrent = "Image" +  point.Y.ToString()+ point.X.ToString();

            foreach (var item in Bord.Children)
            {
                Border temp = (Border)item;
                Image tempImage = (Image)temp.Child;
                if (tempImage.Name == corrent)
                {
                    return (Image)temp.Child;
                }
            }
            return null;
        }
    }
}
