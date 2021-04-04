using Coordinats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Utility;

namespace ChessGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Property and Feld
        public List<BaseFigure> models = new List<BaseFigure>();
        public List<CoordinatPoint> currentListForBabyGame = new List<CoordinatPoint>();
        private BaseFigure CurentKing => (BaseFigure)models.Where(c => c.Color == currentFigureColor && c is King).Single();
        private int whiteKingCount = 0;
        private int whiteQueenCount = 0;
        private int whiteBishopCount = 0;
        private int whiteRookCount = 0;
        private int whitePawnCount = 0;
        private int whiteKnightCount = 0;
        private int blackKingCount = 0;
        private int blackQueenCount = 0;
        private int blackBishopCount = 0;
        private int blackRookCount = 0;
        private int blackPawnCount = 0;
        private int blackKnightCount = 0;
        private string currentFigureColor;
        private Knight knightForeMoves;
        //BaseFigure dragObject = null;
        //UIElement dragObjectImage = null;
        //private bool _isMoving;
        //private Point? _buttonPosition;
        //private double deltaX;
        //private double deltaY;
        //private TranslateTransform _currentTT;
        //private CoordinatPoint startCoordinate;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }
        private void PleacementB1_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GetCurrentFigureColor(), UriKind.Relative);
            bitmap.EndInit();
            string[] tempFigure = GetCurrentFigureColor().Split('/');
            string color = tempFigure[tempFigure.Length - 1].Split('.')[0];
            string figure = tempFigure[tempFigure.Length - 1].Split('.')[1];
            if (GetCoordinatsForPleacement(out CoordinatPoint CoordinatPoint) && GetFigureBase(figure, color, out BaseFigure temp))
            {
                if (temp is King king)
                {
                    if (!DangerPosition(king).Contains(CoordinatPoint))
                    {
                        models.Add(temp);
                        temp.Bitmap = bitmap;
                        temp.SetFigurePosition(CoordinatPoint, Board);
                        //temp.FigureImage.PreviewMouseDown += GamePanel_PreviewMouseDown;
                    }
                    else
                    {
                        MessageBox.Show("King is under a check");
                        if (color == "White")
                            whiteKingCount = 0;
                        else
                            blackKingCount = 0;
                    }
                }
                else
                {
                    models.Add(temp);
                    temp.Bitmap = bitmap;
                    temp.SetFigurePosition(CoordinatPoint, Board);
                    //temp.FigureImage.PreviewMouseDown += GamePanel_PreviewMouseDown;
                }
            }
        }
        private void PlayB2_Click(object sender, RoutedEventArgs e)
        {
            if (SetCurrentColor())
            {
                if (PlayB2.IsEnabled == true)
                {
                    CheckBlack.IsChecked = false;
                    CheckWhite.IsChecked = false;
                    SelectFigur.Text = "";
                    InputCoordinatsLetter.Text = "";
                    InputCoordinatsNumber.Text = "";
                    PleacementB1.Content = "";
                    CheckBlack.IsEnabled = false;
                    CheckWhite.IsEnabled = false;
                    SelectFigur.IsEnabled = false;
                    InputCoordinatsLetter.IsEnabled = false;
                    InputCoordinatsNumber.IsEnabled = false;
                    PleacementB1.IsEnabled = false;
                    PlayColorWhite.IsEnabled = false;
                    PlayColorBlack.IsEnabled = false;
                    InputCoordinatsLetter_Corrent.IsEnabled = true;
                    InputCoordinatsNumber_Corrent.IsEnabled = true;
                    InputCoordinatsLetter_Selected.IsEnabled = true;
                    InputCoordinatsNumber_Selected.IsEnabled = true;
                    InstalB3.IsEnabled = true;
                    MessageBox.Show("Good Luck!!!");
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PlayB2.IsEnabled = false;
            if (GetCurrentFigure())
            {
                Manager manager = new(currentListForBabyGame, CurentKing, models, currentFigureColor, Board, MovesTextBox, MessageHandle);
                manager.Logic();
            }

        }

        private string GetCurrentFigureColor()
        {
            string str = SelectFigur.Text;
            string result = string.Empty;
            if (CheckWhite.IsChecked == true)
            {
                result = str.WhiteFigurePath();
            }
            else if (CheckBlack.IsChecked == true)
            {
                result = str.BlackFigurePath();
            }
            else
            {
                MessageBox.Show("You did not choose the color of the figure");
            }
            return result;
        }

        /// <summary>
        /// Select the color with play
        /// </summary>
        /// <returns>Return the play color Black or White</returns>
        private bool SetCurrentColor()
        {
            if (PlayColorWhite.IsChecked == true)
            {
                currentFigureColor = "White";
                return true;
            }
            else if (PlayColorBlack.IsChecked == true)
            {
                currentFigureColor = "Black";
                return true;
            }
            else
            {
                MessageBox.Show("You did not choose the color for game");
                return false;
            }
        }

        /// <summary>
        /// Check the figure and added the figure instance
        /// </summary>
        /// <param name="figure">The figure</param>
        /// <param name="color">The figure color</param>
        /// <param name="baseFigure">figure instance type</param>
        /// <returns>Return figure instance</returns>
        private bool GetFigureBase(string figure, string color, out BaseFigure baseFigure)
        {
            if (color == "White")
            {
                switch (figure)
                {
                    case "Queen":
                        if (whiteQueenCount == 0)
                        {
                            whiteQueenCount++;
                            baseFigure = new Queen(figure + color, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White queen inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "King":
                        if (whiteKingCount == 0)
                        {
                            whiteKingCount++;
                            baseFigure = new King(figure + color, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White king inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "Rook":
                        if (whiteRookCount <= 1)
                        {
                            whiteRookCount++;
                            baseFigure = new Rook(figure + color + whiteRookCount, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White rook inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "Bishop":
                        if (whiteBishopCount <= 1)
                        {
                            whiteBishopCount++;
                            baseFigure = new Bishop(figure + color + whiteBishopCount, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White rook inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "Knight":
                        if (whiteKnightCount <= 1)
                        {
                            whiteKnightCount++;
                            baseFigure = new Knight(figure + color + whiteKnightCount, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White knight inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "Pawn":
                        if (whitePawnCount <= 7)
                        {
                            whitePawnCount++;
                            baseFigure = new Pawn(figure + color + whitePawnCount, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White pawn inside board");
                            baseFigure = null;
                            return false;
                        }
                    default:
                        baseFigure = null;
                        return false;
                }
            }
            else
            {
                switch (figure)
                {
                    case "Queen":
                        if (blackQueenCount == 0)
                        {
                            blackQueenCount++;
                            baseFigure = new Queen(figure + color, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White queen inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "King":
                        if (blackKingCount == 0)
                        {
                            blackKingCount++;
                            baseFigure = new King(figure + color, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White king inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "Rook":
                        if (blackRookCount <= 1)
                        {
                            blackRookCount++;
                            baseFigure = new Rook(figure + color + blackRookCount, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White rook inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "Bishop":
                        if (blackBishopCount <= 1)
                        {
                            blackBishopCount++;
                            baseFigure = new Bishop(figure + color + blackBishopCount, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White rook inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "Knight":
                        if (blackKnightCount <= 1)
                        {
                            blackKnightCount++;
                            baseFigure = new Knight(figure + color + blackKnightCount, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White knight inside board");
                            baseFigure = null;
                            return false;
                        }
                    case "Pawn":
                        if (blackPawnCount <= 7)
                        {
                            blackPawnCount++;
                            baseFigure = new Pawn(figure + color + blackPawnCount, color, models);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("White pawn inside board");
                            baseFigure = null;
                            return false;
                        }
                    default:
                        baseFigure = null;
                        return false;
                }
            }
        }

        /// <summary>
        /// Change the coordinate for current figure
        /// </summary>
        /// <param name="CoordinatPoint">Coordinate with out parametr</param>
        /// <returns>Return the current figure coordinate</returns>
        private bool GetCoordinatsForPleacement(out CoordinatPoint CoordinatPoint)
        {
            string inputLetter = InputCoordinatsLetter.Text;
            if (inputLetter.Length > 1)
            {
                MessageBox.Show("This letter not found");
                CoordinatPoint = null;
                return false;
            }
            else
            {
                if (Convert.ToChar(inputLetter).CharToInt(out int o))
                {
                    try
                    {
                        string inputNumber = InputCoordinatsNumber.Text;
                        int j = Convert.ToInt32(inputNumber.ToString());
                        if (j <= 8 && j >= 1)
                        {
                            CoordinatPoint = new CoordinatPoint(o - 1, j - 1);
                            if (!GetPosition().Contains(CoordinatPoint))
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("There is a figure in that coordinate");
                                return false;
                            }

                        }
                        else
                        {
                            MessageBox.Show("The number is not found");
                            CoordinatPoint = null;
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("The number is not found");
                        CoordinatPoint = null;
                        return false;
                    }

                }
                else
                {
                    MessageBox.Show("This letter note found");
                    CoordinatPoint = null;
                    return false;
                }
            }

        }

        /// <summary>
        /// Change the coordinate for current figure
        /// </summary>
        /// <param name="CoordinatPoint">Coordinate with out parametr</param>
        /// <returns>Return the current figure coordinate</returns>
        private bool GetCurrentCoordinats(out CoordinatPoint CoordinatPoint, out BaseFigure baseFigureTemp)
        {
            string inputLetter = InputCoordinatsLetter_Corrent.Text;
            baseFigureTemp = null;
            if (inputLetter.Length > 1)
            {
                MessageBox.Show("This letter not found");
                CoordinatPoint = null;
                baseFigureTemp = null;
                return false;
            }
            else
            {
                if (Convert.ToChar(inputLetter).CharToInt(out int o))
                {
                    try
                    {
                        string inputNumber = InputCoordinatsNumber_Corrent.Text;
                        int j = Convert.ToInt32(inputNumber.ToString());
                        if (j <= 8 && j >= 1)
                        {
                            CoordinatPoint = new CoordinatPoint(o - 1, j - 1);
                            foreach (var item in models)
                            {
                                if (item.Coordinate == CoordinatPoint)
                                {
                                    if (item.Color == currentFigureColor)
                                    {
                                        baseFigureTemp = item;
                                        return true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("The color of the piece is not chosen correctly");
                                        CoordinatPoint = null;
                                        baseFigureTemp = null;
                                        return false;
                                    }
                                }
                            }
                            MessageBox.Show("There is a figure in not that coordinate");
                            CoordinatPoint = null;
                            baseFigureTemp = null;
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("The number is not found");
                            CoordinatPoint = null;
                            baseFigureTemp = null;
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("The number is not found");
                        CoordinatPoint = null;
                        baseFigureTemp = null;
                        return false;
                    }

                }
                else
                {
                    CoordinatPoint = null;
                    baseFigureTemp = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Change the coordinate for selected figure
        /// </summary>
        /// <param name="CoordinatPoint">Coordinate for fih=gure</param>
        /// <param name="figure">Figure instance</param>
        /// <returns>Return true if figure coordinate is availabe position</returns>
        private bool GetTargetCoordinats(out CoordinatPoint CoordinatPoint, BaseFigure figure)
        {
            string inputLetter = InputCoordinatsLetter_Selected.Text;
            if (inputLetter.Length > 1)
            {
                MessageBox.Show("This letter not found");
                CoordinatPoint = null;
                return false;
            }
            else
            {
                if (Convert.ToChar(inputLetter).CharToInt(out int o))
                {
                    try
                    {
                        string inputNumber = InputCoordinatsNumber_Selected.Text;
                        int j = Convert.ToInt32(inputNumber.ToString());
                        if (j <= 8 && j >= 1)
                        {
                            CoordinatPoint = new CoordinatPoint(o - 1, j - 1);
                            IRandomMove tempFigure = (IRandomMove)figure;
                            if (figure is King)
                            {
                                if (GetCurrentKingMoves().Contains(CoordinatPoint))
                                {
                                    return true;
                                }
                                else
                                {
                                    MessageBox.Show("There is a figure in not that coordinate");
                                    return false;
                                }
                            }
                            else if (tempFigure.AvailableMoves().Contains(CoordinatPoint))
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("There is a figure in not that coordinate");
                                return false;
                            }

                        }
                        else
                        {
                            MessageBox.Show("The number is not found");
                            CoordinatPoint = null;
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("The number is not found");
                        CoordinatPoint = null;
                        return false;
                    }

                }
                else
                {
                    CoordinatPoint = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Check the selectid figure and change figure position
        /// </summary>
        /// <returns>Return true if figure new coordinate is changed</returns>
        private bool GetCurrentFigure()
        {
            if (GetCurrentCoordinats(out CoordinatPoint coordinatPoint, out BaseFigure baseFigureTemp)
                           && GetTargetCoordinats(out CoordinatPoint selectedPoint, baseFigureTemp))
            {
                if (baseFigureTemp.Color == currentFigureColor)
                {
                    IRandomMove tempFigur = (IRandomMove)baseFigureTemp;
                    if (tempFigur.AvailableMoves().Contains(selectedPoint))
                    {
                        baseFigureTemp.SetFigurePosition(selectedPoint, Board);
                        currentListForBabyGame.Add(selectedPoint);
                        MovesTextBox.Text += $"{InputCoordinatsLetter_Corrent.Text + InputCoordinatsNumber_Corrent.Text} - " +
                                           $"{InputCoordinatsLetter_Selected.Text + InputCoordinatsNumber_Selected.Text}\n{new string('-', 8)}\n";
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("This figur can not move this position");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("You selected not right color");
                    return false;
                }
            }
            return false;
        }

        #region Game

        /// <summary>
        /// Check the available moves for current king 
        /// </summary>
        /// <returns>Return the list</returns>
        private List<CoordinatPoint> GetCurrentKingMoves()
        {
            IRandomMove currentKing = (IRandomMove)CurentKing;
            List<CoordinatPoint> result = new List<CoordinatPoint>();
            foreach (var item in currentKing.AvailableMoves())
            {
                if (!DangerPosition(CurentKing).Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Check the danger position for current king
        /// </summary>
        /// <param name="model">King instance withe or Black</param>
        /// <returns>Return danger position List for current king </returns>
        public List<CoordinatPoint> DangerPosition(BaseFigure model)
        {
            List<CoordinatPoint> result = new List<CoordinatPoint>();
            var modelNew = models.Where(c => c.Color != model.Color);
            foreach (var item in modelNew)
            {
                var temp = (IDangerMoves)item;
                var array = temp.DangerMoves();
                result.AddRange(array);
            }
            return result;
        }

        /// <summary>
        /// Check the all figurs position
        /// </summary>
        /// <returns>Return the figurs position</returns>
        private List<CoordinatPoint> GetPosition()
        {
            List<CoordinatPoint> positions = new List<CoordinatPoint>();
            foreach (var item in models)
            {
                positions.Add(item.Coordinate);
            }
            return positions;
        }

        #region For Knight Moves

        private bool GetCoordinateKnight(TextBox textBoxLetter, TextBox textBoxNumber, out CoordinatPoint CoordinatPoint)
        {
            string inputLetter = textBoxLetter.Text;
            if (inputLetter.Length > 1)
            {
                MessageBox.Show("This letter not found");
                CoordinatPoint = null;
                return false;
            }
            else
            {
                if (Convert.ToChar(inputLetter).CharToInt(out int o))
                {
                    try
                    {
                        string inputNumber = textBoxNumber.Text;
                        int j = Convert.ToInt32(inputNumber.ToString());
                        if (j <= 8 && j >= 1)
                        {
                            CoordinatPoint = new CoordinatPoint(o - 1, j - 1);

                            return true;
                        }
                        else
                        {
                            MessageBox.Show("The number is not found");
                            CoordinatPoint = null;
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("The number is not found");
                        CoordinatPoint = null;
                        return false;
                    }

                }
                else
                {
                    MessageBox.Show("This letter note found");
                    CoordinatPoint = null;
                    return false;
                }
            }

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (GetCoordinateKnight(KnightStartLetter, KnightStartNumber, out CoordinatPoint coordinatPoint))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("Resources/Black.Knight.png", UriKind.Relative);
                bitmap.EndInit();
                this.knightForeMoves = new Knight("KnightMoves", "Black", models);
                this.knightForeMoves.Bitmap = bitmap;
                this.knightForeMoves.SetFigurePosition(coordinatPoint, Board);
            }
        }
        private void KnightMoveCheck_Click(object sender, RoutedEventArgs e)
        {
            if (GetCoordinateKnight(KnightTargetLetter, KnightTargetNumber, out CoordinatPoint coordinatPoint))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("Resources/Black.Knight.png", UriKind.Relative);
                bitmap.EndInit();
                Knight knight = new Knight("KnightMovesTaarget", "Black", models);
                knight.Bitmap = bitmap;
                knight.SetFigurePosition(coordinatPoint, Board);
                var count = this.knightForeMoves.MinKnightCount(coordinatPoint);
                KnightMovesMessage.Text = $"For target coordinate your need {count} moves";
            }
        }

        #endregion

        #endregion

        //private void GamePanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    //var image = e.Source as Image;
        //    //if (image != null && image.Name.Contains(currentFigureColor))
        //    //{
        //    //    CoordinatPoint coordinatPoint = new CoordinatPoint(0, 0);
        //    //    this.dragObjectImage = image;
        //    //    coordinatPoint.X = Grid.GetColumn(image);
        //    //    coordinatPoint.Y = Grid.GetRow(image);
        //    //    foreach (var item in models)
        //    //    {
        //    //        if (item.Coordinate == coordinatPoint)
        //    //        {
        //    //            dragObject = item;
        //    //            this.startCoordinate = dragObject.Coordinate;
        //    //            break;
        //    //        }
        //    //    }
        //    //    if (_buttonPosition == null)
        //    //        _buttonPosition = dragObjectImage.TransformToAncestor(Board).Transform(new Point(0, 0));
        //    //    var mousePosition = Mouse.GetPosition(Board);
        //    //    deltaX = mousePosition.X - _buttonPosition.Value.X;
        //    //    deltaY = mousePosition.Y - _buttonPosition.Value.Y;
        //    //    _isMoving = true;
        //    }
        //}

        //private void GamePanel_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    //if (!_isMoving) return;

        //    //var mousePoint = Mouse.GetPosition(Board);

        //    //var offsetX = (_currentTT == null ? _buttonPosition.Value.X : _buttonPosition.Value.X - _currentTT.X) + deltaX - mousePoint.X;
        //    //var offsetY = (_currentTT == null ? _buttonPosition.Value.Y : _buttonPosition.Value.Y - _currentTT.Y) + deltaY - mousePoint.Y;

        //    //this.dragObjectImage.RenderTransform = new TranslateTransform(-offsetX, -offsetY);
        //}
        //private void GamePanel_PreviewMouseUp(object sender, MouseEventArgs e)
        //{
        //    //_currentTT = dragObjectImage.RenderTransform as TranslateTransform;
        //    //var mousePosition = Mouse.GetPosition(Board);
        //    //CoordinatPoint coordinatPoint = new CoordinatPoint(0, 0);
        //    //coordinatPoint.X = (int)Math.Floor((mousePosition.X) / 65);
        //    //coordinatPoint.Y = (int)Math.Floor((mousePosition.Y) / 65);
        //    //IAvailableMoves currentFigur = (IAvailableMoves)dragObject;
        //    //if (currentFigur is King)
        //    //{
        //    //    if (GetCurrentKingMoves().Contains(coordinatPoint))
        //    //    {
        //    //        dragObject.SetFigurePosition(coordinatPoint, Board);
        //    //        Manager manager = new(currentListForBabyGame, CurentKing, models, currentFigureColor, Board, MovesTextBox, MessageHandle);
        //    //        manager.Logic();
        //    //        _isMoving = false;
        //    //    }
        //    //    else
        //    //    {
        //    //        dragObject.SetFigurePosition(startCoordinate, Board);
        //    //        return;
        //    //    }
        //    //}
        //    //else if (currentFigur.AvailableMoves().Contains(coordinatPoint))
        //    //{
        //    //    dragObject.SetFigurePosition(coordinatPoint, Board);
        //    //    Manager manager = new(currentListForBabyGame, CurentKing, models, currentFigureColor, Board, MovesTextBox, MessageHandle);
        //    //    manager.Logic();
        //    //    _isMoving = false;
        //    //}
        //    //else
        //    //{
        //    //    dragObject.SetFigurePosition(startCoordinate, Board);
        //    //    return;
        //    //}

        //}
    }
}
