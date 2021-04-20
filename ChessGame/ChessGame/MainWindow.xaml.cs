using Figure;
using ManagerFKG;
using MovesFKnight;
using StandardGame;
using System;
using System.Collections.Generic;
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
        public List<BaseFigure> models = new();
        private List<CoordinatePoint> currentListForBabyGame = new();
        public UIElement DragObjectImage { get => dragObjectImage; set => dragObjectImage = value; }
        public string currentFigureColor;
        private Knight knightForeMoves;
        private int countForKnightMoves = 0;
        private BaseFigure dragObject = null;
        private UIElement dragObjectImage = null;
        private CoordinatePoint startCoordinate;
        public bool gameManager = false;
        public Manager manager1;
        private bool colorPower = false;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Game

        #region King game Button
        private void PleacementB1_Click(object sender, RoutedEventArgs e)
        {
            string[] tempFigure = GetCurrentFigureImage().Split('/');
            string color = tempFigure[tempFigure.Length - 1].Split('.')[0];
            string figure = tempFigure[tempFigure.Length - 1].Split('.')[1];
            manager1 = new Manager(currentListForBabyGame, currentFigureColor);
            if (manager1.GetFigure(figure, color, out BaseFigure temp))
            {
                if (GetCoordinatesForPleacement(out CoordinatePoint CoordinatPoint))
                {
                    if (manager1.IsValidForPleacement(temp.Name, CoordinatPoint))
                    {
                        temp.setPicture += SetFigurePicture;
                        temp.removePicture += RemoveFigurePicture;
                        temp.messageForMove += MessageMove;
                        temp.SetFigurePosition(CoordinatPoint);
                    }
                }
            }
        }
        private void PlayB2_Click(object sender, RoutedEventArgs e)
        {
            if (SetCurrentColor())
            {
                if (PlayB2.IsEnabled == true)
                {
                    CheckBlack.IsEnabled = false;
                    CheckWhite.IsEnabled = false;
                    SelectFigur.Text = "";
                    InputCoordinatsLetter.Text = "";
                    InputCoordinatsNumber.Text = "";
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
        private void InstallButton(object sender, RoutedEventArgs e)
        {
            PlayB2.IsEnabled = false;
            //if (GetCurrentFigure())
            //{
            GetCurrentFigureNew();
            Manager manager = new(currentListForBabyGame, currentFigureColor);
            manager.MateMessage += MessageMate;
            manager.Logic();
            //}
        }

        #endregion

        /// <summary>
        /// Show a figure moves
        /// </summary>
        /// <param name="baseFigure">Figure instanste</param>
        /// <param name="coordinate">Figure old and new coordinate</param>
        public void MessageMove(object baseFigure, (CoordinatePoint, CoordinatePoint) coordinateTupl)
        {
            if (coordinateTupl.Item1 != null)
            {
                var tempFigure = (BaseFigure)baseFigure;
                MovesTextBox.Text += $"{tempFigure.Name} " + $"{coordinateTupl.Item1}-" +
                    $"{coordinateTupl.Item2}\n{new string('-', 8)}\n";
            }
        }

        /// <summary>
        /// Show a Mate message
        /// </summary>
        /// <param name="sender">figure</param>
        /// <param name="message">Mate</param>
        public void MessageMate(object sender, string message)
        {
            MessageHandle.Text = message;
        }

        /// <summary>
        /// Set the figure image
        /// </summary>
        /// <param name="baseFigure">Figure instance</param>
        /// <param name="coordinate">Figure </param>
        public void SetFigurePicture(object baseFigure, CoordinatePoint coordinate)
        {
            BaseFigure tempFigure = (BaseFigure)baseFigure;
            string str = tempFigure.Name;
            Image image = new Image();
            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GetCurrentFigureImage(str), UriKind.Relative);
            bitmap.EndInit();
            image.Source = bitmap;
            image.Tag = tempFigure.Name;
            Grid.SetColumn(image, coordinate.X);
            Grid.SetRow(image, coordinate.Y);
            Board.Children.Add(image);
        }

        /// <summary>
        /// Remove the figure image
        /// </summary>
        /// <param name="baseFigure">Figure instance</param>
        /// <param name="coordinate">Figure </param>
        public void RemoveFigurePicture(object baseFigure, CoordinatePoint coordinate)
        {
            BaseFigure tempFigure = (BaseFigure)baseFigure;
            foreach (var item in Board.Children)
            {
                if (item is Image image1)
                {
                    if (tempFigure.Name == image1.Tag.ToString())
                    {
                        Board.Children.Remove(image1);
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// Get a figure image source
        /// </summary>
        /// <param name="name">Figure name</param>
        /// <returns>Return the instance image source</returns>
        private string GetCurrentFigureImage(string name)
        {
            string[] str = name.Split('.');
            string result = str[1] == "White" ? str[0].WhiteFigurePath() : str[0].BlackFigurePath();
            return result;
        }

        /// <summary>
        /// Get a figure image source
        /// </summary>
        /// <returns>Return the instance image source</returns>
        private string GetCurrentFigureImage()
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
        /// Change the coordinate for current figure
        /// </summary>
        /// <param name="CoordinatPoint">Coordinate with out parametr</param>
        /// <returns>Return the current figure coordinate</returns>
        private bool GetCoordinatesForPleacement(out CoordinatePoint CoordinatPoint)
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
                            CoordinatPoint = new CoordinatePoint(o - 1, j - 1);
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
        public string GetCurrentFigureCoordinate(TextBox letter, TextBox number)
        {
            string inputLetter = letter.Text;
            if (inputLetter.Length > 1)
            {
                MessageBox.Show("This letter not found");
            }
            else
            {
                if (Convert.ToChar(inputLetter).CharToInt(out int o))
                {
                    try
                    {
                        string inputNumber = number.Text;
                        int j = Convert.ToInt32(inputNumber.ToString());
                        if (j <= 8 && j >= 1)
                        {
                            return $"{o - 1}.{j - 1}";
                        }
                        else
                        {
                            MessageBox.Show("The number is not found");
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("The number is not found");
                    }
                }
                else
                {
                    MessageBox.Show("This letter not found");
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Check the selectid figure and change figure position
        /// </summary>
        /// <returns>Return true if figure new coordinate is changed</returns>
        private void GetCurrentFigureNew()
        {
            string current = GetCurrentFigureCoordinate(InputCoordinatsLetter_Corrent, InputCoordinatsNumber_Corrent);
            string target = GetCurrentFigureCoordinate(InputCoordinatsLetter_Selected, InputCoordinatsNumber_Selected);
            manager1 = new Manager(currentListForBabyGame, currentFigureColor);
            manager1.IsVAlidCoordinate(current, target);
        }

        #endregion

        #region For Knight Moves

        /// <summary>
        /// Get a coordinate for knight start position
        /// </summary>
        /// <param name="textBoxLetter">Letter coordinate text box name</param>
        /// <param name="textBoxNumber">Number coordinate text box name</param>
        /// <param name="CoordinatPoint">Out parametr</param>
        /// <returns>Return the coordinate for knight start position</returns>
        private bool GetCoordinateKnight(TextBox textBoxLetter, TextBox textBoxNumber, out CoordinatePoint CoordinatPoint)
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
                            CoordinatPoint = new CoordinatePoint(o - 1, j - 1);
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
        private void KnightSetButton(object sender, RoutedEventArgs e)
        {
            if (GetCoordinateKnight(KnightStartLetter, KnightStartNumber, out CoordinatePoint coordinatPoint))
            {
                this.knightForeMoves = new Knight("Knight.Black", "Black", models);
                this.knightForeMoves.setPicture += SetFigurePicture;
                this.knightForeMoves.removePicture += RemoveFigurePicture;
                this.knightForeMoves.messageForMove += delegate { KnightMovesMessage.Text = " "; };
                knightForeMoves.SetFigurePosition(coordinatPoint);
                models.Add(this.knightForeMoves);
            }
        }
        private void KnightMoveCheck_Click(object sender, RoutedEventArgs e)
        {
            if (GetCoordinateKnight(KnightTargetLetter, KnightTargetNumber, out CoordinatePoint coordinatPoint))
            {
                Knight knight = new Knight("Knight.Black.Target", "Black", models);
                knight.setPicture += SetFigurePicture;
                knight.removePicture += RemoveFigurePicture;
                knight.messageForMove += delegate { KnightMovesMessage.Text = " "; };
                models.Add(knight);
                knight.SetFigurePosition(coordinatPoint);
                MovesKnight movesFKnight = new MovesKnight();
                countForKnightMoves = movesFKnight.MinKnightCount(coordinatPoint, this.knightForeMoves.Coordinate);
                KnightMovesMessage.Text = $"For target coordinate your need {countForKnightMoves} moves";
                countForKnightMoves = 0;
            }
        }

        #endregion

        #region Reset Method and buttons

        /// <summary>
        /// Reset Board for start game
        /// </summary>
        private void ResetBoard()
        {
            GetAllFigures(gameManager);
            foreach (var item in models)
            {
                item.RemoveFigurePosition();
            }
            models.Clear();
            MovesTextBox.Text = "";
            MessageHandle.Text = "";
        }

        /// <summary>
        /// Reset button the Board for start King game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_Button(object sender, RoutedEventArgs e)
        {
            ResetBoard();
            PlayB2.IsEnabled = true;
            CheckBlack.IsEnabled = true;
            CheckWhite.IsEnabled = true;
            SelectFigur.IsEnabled = true;
            InputCoordinatsLetter.IsEnabled = true;
            InputCoordinatsNumber.IsEnabled = true;
            PleacementB1.IsEnabled = true;
            PlayColorWhite.IsEnabled = true;
            PlayColorBlack.IsEnabled = true;
            InputCoordinatsLetter_Corrent.IsEnabled = false;
            InputCoordinatsNumber_Corrent.IsEnabled = false;
            InputCoordinatsLetter_Selected.IsEnabled = false;
            InputCoordinatsNumber_Selected.IsEnabled = false;
            InstalB3.IsEnabled = false;
        }

        /// <summary>
        /// Reset button the Board for start Knight game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_ButtonForKnight(object sender, RoutedEventArgs e)
        {
            ResetBoard();
            KnightMovesMessage.Text = "";
            KnightStartLetter.Text = "";
            KnightStartNumber.Text = "";
            KnightTargetLetter.Text = "";
            KnightTargetNumber.Text = "";
        }

        #endregion

        #region Drag and Drop
        private void Board_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var image = e.Source as Image;
            string imageName = image.Source.ToString();
            if (image != null && imageName.Contains(currentFigureColor))
            {
                CoordinatePoint coordinatPoint = new CoordinatePoint(0, 0);
                this.DragObjectImage = image;
                coordinatPoint.X = Grid.GetColumn(image);
                coordinatPoint.Y = Grid.GetRow(image);
                dragObject = GameManagerForDrop(gameManager, coordinatPoint);
                this.startCoordinate = dragObject.Coordinate;
                DragDrop.DoDragDrop(image, dragObject, DragDropEffects.Move);
            }
        }
        private void Image_Drop(object sender, DragEventArgs e)
        {
            CoordinatePoint coordinatPoint = new CoordinatePoint(0, 0);
            coordinatPoint.X = Grid.GetColumn((UIElement)e.OriginalSource);
            coordinatPoint.Y = Grid.GetRow((UIElement)e.OriginalSource);
            if (coordinatPoint != dragObject.Coordinate)
            {
                IAvailableMoves currentFigur = (IAvailableMoves)dragObject;
                if (currentFigur.AvailableMoves().Contains(coordinatPoint))
                {
                    dragObject.SetFigurePosition(coordinatPoint);
                    GameManager(gameManager);
                }
                else
                {
                    dragObject.SetFigurePosition(startCoordinate);
                    return;
                }
            }

        }

        #endregion

        #region MenuStrip
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Game Create Arsen Dashyan");
        }
        private void KingGame_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 1348;
            Thickness thickness = new Thickness(365);
            this.KnightPage.Margin = thickness;
            this.Width = 1000;
            ResetBoard();
            PlayB2.IsEnabled = true;
            CheckBlack.IsEnabled = true;
            CheckWhite.IsEnabled = true;
            SelectFigur.IsEnabled = true;
            InputCoordinatsLetter.IsEnabled = true;
            InputCoordinatsNumber.IsEnabled = true;
            PleacementB1.IsEnabled = true;
            PlayColorWhite.IsEnabled = true;
            PlayColorBlack.IsEnabled = true;
            InputCoordinatsLetter_Corrent.IsEnabled = false;
            InputCoordinatsNumber_Corrent.IsEnabled = false;
            InputCoordinatsLetter_Selected.IsEnabled = false;
            InputCoordinatsNumber_Selected.IsEnabled = false;
            InstalB3.IsEnabled = false;
            MessageBox.Show("You Change A King Game, Good Luck");
        }
        private void KnightGame_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 1348;
            Thickness thickness = new Thickness(0);
            this.KnightPage.Margin = thickness;
            this.Width = 1000;
            ResetBoard();
            KnightMovesMessage.Text = "";
            KnightStartLetter.Text = "";
            KnightStartNumber.Text = "";
            KnightTargetLetter.Text = "";
            KnightTargetNumber.Text = "";
            MessageBox.Show("You Change A Knight Game, Good Luck");
        }
        private void StandardGame_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 1348;
            this.Width = 620;
            ResetBoard();
            Standard standard = new Standard(currentFigureColor);
            models = standard.figures;
            SetAllFigures(models);
            MessageBox.Show("You Change A Standard Game, Good Luck");
        }
        #endregion

        private void GameManager(bool gameSatus)
        {
            if (gameSatus)
            {
                Manager manager = new(currentListForBabyGame, currentFigureColor);
                manager.MateMessage += MessageMate;
                manager.Logic();
            }
            else
            {
                if (colorPower)
                {
                    currentFigureColor = "White";
                    colorPower = false;
                }
                else
                {
                    currentFigureColor = "Black";
                    colorPower = true;
                }
            }
        }
        private BaseFigure GameManagerForDrop(bool gameSatus, CoordinatePoint coordinatePoint)
        {
            BaseFigure baseFigure = null;
            if (gameSatus)
            {
                Manager manager = new(currentListForBabyGame, currentFigureColor);
                baseFigure = manager.CheckTheBaseFigure(coordinatePoint);
            }
            else
            {
                baseFigure = Standard.CheckTheBaseFigure(coordinatePoint);
            }
            return baseFigure;
        }
        public void SetAllFigures(List<BaseFigure> baseFigures)
        {
            foreach (var item in baseFigures)
            {
                item.setPicture += SetFigurePicture;
                item.removePicture += RemoveFigurePicture;
                item.messageForMove += MessageMove;
                currentFigureColor = "White";
                switch (item.Name)
                {
                    case "Queen.White.1":
                        item.SetFigurePosition(new CoordinatePoint(3, 7));
                        continue;
                    case "Queen.Black.1":
                        item.SetFigurePosition(new CoordinatePoint(3, 0));
                        continue;
                    case "King.White.1":
                        item.SetFigurePosition(new CoordinatePoint(4, 7));
                        continue;
                    case "King.Black.1":
                        item.SetFigurePosition(new CoordinatePoint(4, 0));
                        continue;
                    case "Bishop.Black.1":
                        item.SetFigurePosition(new CoordinatePoint(2, 0));
                        continue;
                    case "Bishop.Black.2":
                        item.SetFigurePosition(new CoordinatePoint(5, 0));
                        continue;
                    case "Bishop.White.1":
                        item.SetFigurePosition(new CoordinatePoint(2, 7));
                        continue;
                    case "Bishop.White.2":
                        item.SetFigurePosition(new CoordinatePoint(5, 7));
                        continue;
                    case "Knight.White.1":
                        item.SetFigurePosition(new CoordinatePoint(1, 7));
                        continue;
                    case "Knight.White.2":
                        item.SetFigurePosition(new CoordinatePoint(6, 7));
                        continue;
                    case "Knight.Black.1":
                        item.SetFigurePosition(new CoordinatePoint(1, 0));
                        continue;
                    case "Knight.Black.2":
                        item.SetFigurePosition(new CoordinatePoint(6, 0));
                        continue;
                    case "Rook.Black.1":
                        item.SetFigurePosition(new CoordinatePoint(0, 0));
                        continue;
                    case "Rook.Black.2":
                        item.SetFigurePosition(new CoordinatePoint(7, 0));
                        continue;
                    case "Rook.White.1":
                        item.SetFigurePosition(new CoordinatePoint(0, 7));
                        continue;
                    case "Rook.White.2":
                        item.SetFigurePosition(new CoordinatePoint(7, 7));
                        continue;
                    case "Pawn.White.1":
                        item.SetFigurePosition(new CoordinatePoint(0, 6));
                        continue;
                    case "Pawn.White.2":
                        item.SetFigurePosition(new CoordinatePoint(1, 6));
                        continue;
                    case "Pawn.White.3":
                        item.SetFigurePosition(new CoordinatePoint(2, 6));
                        continue;
                    case "Pawn.White.4":
                        item.SetFigurePosition(new CoordinatePoint(3, 6));
                        continue;
                    case "Pawn.White.5":
                        item.SetFigurePosition(new CoordinatePoint(4, 6));
                        continue;
                    case "Pawn.White.6":
                        item.SetFigurePosition(new CoordinatePoint(5, 6));
                        continue;
                    case "Pawn.White.7":
                        item.SetFigurePosition(new CoordinatePoint(6, 6));
                        continue;
                    case "Pawn.White.8":
                        item.SetFigurePosition(new CoordinatePoint(7, 6));
                        continue;
                    case "Pawn.Black.1":
                        item.SetFigurePosition(new CoordinatePoint(0, 1));
                        continue;
                    case "Pawn.Black.2":
                        item.SetFigurePosition(new CoordinatePoint(1, 1));
                        continue;
                    case "Pawn.Black.3":
                        item.SetFigurePosition(new CoordinatePoint(2, 1));
                        continue;
                    case "Pawn.Black.4":
                        item.SetFigurePosition(new CoordinatePoint(3, 1));
                        continue;
                    case "Pawn.Black.5":
                        item.SetFigurePosition(new CoordinatePoint(4, 1));
                        continue;
                    case "Pawn.Black.6":
                        item.SetFigurePosition(new CoordinatePoint(5, 1));
                        continue;
                    case "Pawn.Black.7":
                        item.SetFigurePosition(new CoordinatePoint(6, 1));
                        continue;
                    case "Pawn.Black.8":
                        item.SetFigurePosition(new CoordinatePoint(7, 1));
                        continue;
                    default:
                        break;
                }
            }
        }

        private void GetAllFigures(bool gameSatus)
        {
            if (gameSatus)
            {
                models = Manager.models;
            }
            else
            {
                models = Standard.models;
            }
        }
    }
}
