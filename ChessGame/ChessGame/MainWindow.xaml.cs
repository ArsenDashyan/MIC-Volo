using GameManager;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ChessGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Property and Feld
        private UIElement DragObjectImage { get => dragObjectImage; set => dragObjectImage = value; }
        private UIElement dragObjectImage = null;
        private Manager manager;
        private MovesKnight movesKnight = new MovesKnight();
        private Standard standard = new Standard();
        private List<string> models = new();
        public static int currentGameStatus;
        public string currentFigureColor;
        private string startCoordinate;
        private int countForKnightMoves = 0;
        private bool colorPower = false;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            manager = new Manager(currentFigureColor);
            manager.setPicture += SetFigurePicture;
            manager.removePicture += RemoveFigurePicture;
            manager.messageForMove += MessageMove;
            standard.setPicture += SetFigurePicture;
            standard.removePicture += RemoveFigurePicture;
            standard.messageForMove += MessageMove;
            standard.messageCheck += MessageMate;
            standard.messageForPawnChange += MessageForPawnChange;
            movesKnight.setPicture += SetFigurePicture;
            movesKnight.messageForMove += delegate { };
        }

        #region Methods for Events

        /// <summary>
        /// Set the figure image
        /// </summary>
        /// <param name="baseFigure">Figure instance</param>
        /// <param name="coordinate">Figure </param>
        public void SetFigurePicture(object sender, string coordinate)
        {
            string[] coord = coordinate.Split('.');
            string str = coord[2] + '.' + coord[3] + '.' + coord[4];
            Image image = new Image();
            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GetCurrentFigureImage(str), UriKind.Relative);
            bitmap.EndInit();
            image.Source = bitmap;
            image.Tag = str;
            Grid.SetColumn(image, Convert.ToChar(coord[0]).CharToInt());
            Grid.SetRow(image, int.Parse(coord[1]) - 1);
            Board.Children.Add(image);
        }

        /// <summary>
        /// Remove the figure image
        /// </summary>
        /// <param name="baseFigure">Figure instance</param>
        /// <param name="coordinate">Figure </param>
        public void RemoveFigurePicture(object sender, string coordinate)
        {
            string[] info = coordinate.Split('.');
            string tempName = info[2] + '.' + info[3] + '.' + info[4];
            foreach (var item in Board.Children)
            {
                if (item is Image image1)
                {
                    if (tempName == image1.Tag.ToString())
                    {
                        Board.Children.Remove(image1);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Show a figure moves
        /// </summary>
        /// <param name="baseFigure">Figure instanste</param>
        /// <param name="coordinate">Figure old and new coordinate</param>
        public void MessageMove(object sender, (string, string) coordinateTupl)
        {
            if (coordinateTupl.Item1 != string.Empty)
            {
                string[] start = coordinateTupl.Item1.Split('.');
                string[] target = coordinateTupl.Item2.Split('.');
                MovesTextBox.Text += $"{start[2]}{start[3]}-" + $"{start[0]}{start[1]}:" +
                    $"{target[0]}{target[1]}\n{new string('-', 8)}\n";
            }
        }

        /// <summary>
        /// Initialize a MessageForPawnChange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void MessageForPawnChange(object sender, string message)
        {
            ChooseFigureForPawn.IsEnabled = true;
            ChooseButton.IsEnabled = true;
            MessageHandle.Text = message;
        }

        #endregion

        #region King Game

        #region King game Button
        private void PleacementB1_Click(object sender, RoutedEventArgs e)
        {
            string[] tempFigure = GetCurrentFigureImage().Split('/');
            string color = tempFigure[tempFigure.Length - 1].Split('.')[0];
            string figure = tempFigure[tempFigure.Length - 1].Split('.')[1];
            string inputInfo = figure + '.' + color + '.';
            inputInfo += GetCurrentFigureCoordinate(InputCoordinatsLetter, InputCoordinatsNumber);
            manager.IsValidForPleacement(inputInfo);
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
            var tupl = GetCurrentFigureNew();
            GameManager(tupl);
        }

        #endregion

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
        private string GetChangeFigureImage(string color)
        {
            string str = ChooseFigureForPawn.Text;
            string result = string.Empty;
            if (color == "White")
            {
                result = str.BlackFigurePath();
            }
            else
            {
                result = str.WhiteFigurePath();
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
                manager = new(currentFigureColor);
                return true;
            }
            else if (PlayColorBlack.IsChecked == true)
            {
                currentFigureColor = "Black";
                manager = new(currentFigureColor);
                return true;
            }
            else
            {
                MessageBox.Show("You did not choose the color for game");
                return false;
            }
        }

        /// <summary>
        /// Check the selectid figure and change figure position
        /// </summary>
        /// <returns>Return true if figure new coordinate is changed</returns>
        private (string, string) GetCurrentFigureNew()
        {
            string current = GetCurrentFigureCoordinate(InputCoordinatsLetter_Corrent, InputCoordinatsNumber_Corrent);
            string target = GetCurrentFigureCoordinate(InputCoordinatsLetter_Selected, InputCoordinatsNumber_Selected);
            return (current, target);
        }

        #endregion

        #region For Knight Moves

        private void KnightSetButton(object sender, RoutedEventArgs e)
        {
            string coordinate = GetCurrentFigureCoordinate(KnightStartLetter, KnightStartNumber);
            movesKnight.CreateStartKnight(coordinate);
        }
        private void KnightMoveCheck_Click(object sender, RoutedEventArgs e)
        {
            string coordinate = GetCurrentFigureCoordinate(KnightTargetLetter, KnightTargetNumber);
            movesKnight.CreateTargetKnight(coordinate);
            countForKnightMoves = movesKnight.MinKnightCount();
            KnightMovesMessage.Text = $"For target coordinate your need {countForKnightMoves} moves";
            countForKnightMoves = 0;
        }

        #endregion

        #region Reset Method and buttons

        /// <summary>
        /// Reset Board for start game
        /// </summary>
        private void ResetBoard()
        {
            GetAllFiguresForReset();
            if (models[0] != "0")
            {
                foreach (var item in models)
                {
                    RemovePicture(item);
                }
            }
            MovesTextBox.Text = "";
            MessageHandle.Text = "";
        }

        /// <summary>
        /// Get all figures names for reset board
        /// </summary>
        private void GetAllFiguresForReset()
        {
            switch (currentGameStatus)
            {
                case 1:
                    models = manager.GetNamesForReset();
                    break;
                case 2:
                    models = movesKnight.GetNamesForReset();
                    break;
                case 3:
                    models = standard.GetNamesForReset();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Remove figure picture from board
        /// </summary>
        /// <param name="name">Figure name</param>
        public void RemovePicture(string name)
        {
            foreach (var item in Board.Children)
            {
                if (item is Image image1)
                {
                    if (name == image1.Tag.ToString())
                    {
                        Board.Children.Remove(image1);
                        break;
                    }
                }
            }

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
                this.DragObjectImage = image;
                int coordX = Grid.GetColumn(image);
                int coordY = Grid.GetRow(image);
                string coordinate = $"{coordX}.{coordY}";
                this.startCoordinate = coordinate;
                DragDrop.DoDragDrop(image, this.DragObjectImage, DragDropEffects.Move);
            }
        }
        private void Image_Drop(object sender, DragEventArgs e)
        {
            int coordX = Grid.GetColumn((UIElement)e.OriginalSource);
            int coordY = Grid.GetRow((UIElement)e.OriginalSource);
            string coordinate = $"{coordX}.{coordY}";
            if (coordinate != this.startCoordinate)
            {
                GameManager((this.startCoordinate, coordinate));
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
            KnightPage.Margin = thickness;
            this.Width = 1000;
            ResetBoard();
            ShowKingGamePanel();
            currentGameStatus = 1;
            MessageBox.Show("You Change A King Game, Good Luck");
        }
        private void KnightGame_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 1348;
            Thickness thickness = new Thickness(0);
            KnightPage.Margin = thickness;
            this.Width = 1000;
            ResetBoard();
            KnightMovesMessage.Text = "";
            KnightStartLetter.Text = "";
            KnightStartNumber.Text = "";
            KnightTargetLetter.Text = "";
            KnightTargetNumber.Text = "";
            currentGameStatus = 2;
            MessageBox.Show("You Change A Knight Game, Good Luck");
        }
        private void StandardGame_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 1348;
            Thickness thickness = new Thickness(365);
            KnightPage.Margin = thickness;
            this.Width = 1000;
            ResetBoard();
            ShowStandardGamePanel();
            standard.SetAllFigures();
            currentGameStatus = 3;
            MessageBox.Show("You Change A Standard Game, Good Luck");
        }
        public void ShowStandardGamePanel()
        {
            PleacementB1.Visibility = Visibility.Hidden;
            SelectFigur.Visibility = Visibility.Hidden;
            InputCoordinatsLetter.Visibility = Visibility.Hidden;
            InputCoordinatsNumber.Visibility = Visibility.Hidden;
            CheckWhite.Visibility = Visibility.Hidden;
            CheckBlack.Visibility = Visibility.Hidden;
            PleacementFigureCoordinate.Visibility = Visibility.Hidden;
            PlayB2.Visibility = Visibility.Hidden;
            PlayColorWhite.Visibility = Visibility.Hidden;
            PlayColorBlack.Visibility = Visibility.Hidden;
            PlayColorLabel.Visibility = Visibility.Hidden;
            ChooseFigureForPawn.Visibility = Visibility.Visible;
            ChooseButton.Visibility = Visibility.Visible;
            InputCoordinatsLetter_Corrent.IsEnabled = true;
            InputCoordinatsNumber_Corrent.IsEnabled = true;
            InputCoordinatsLetter_Selected.IsEnabled = true;
            InputCoordinatsNumber_Selected.IsEnabled = true;
            InstalB3.IsEnabled = true;
            ChooseFigureForPawn.IsEnabled = false;
            ChooseButton.IsEnabled = false;
        }
        public void ShowKingGamePanel()
        {
            ChooseFigureForPawn.Visibility = Visibility.Hidden;
            ChooseButton.Visibility = Visibility.Hidden;
            PleacementB1.Visibility = Visibility.Visible;
            SelectFigur.Visibility = Visibility.Visible;
            InputCoordinatsLetter.Visibility = Visibility.Visible;
            InputCoordinatsNumber.Visibility = Visibility.Visible;
            CheckWhite.Visibility = Visibility.Visible;
            CheckBlack.Visibility = Visibility.Visible;
            PleacementFigureCoordinate.Visibility = Visibility.Visible;
            PlayB2.Visibility = Visibility.Visible;
            PlayColorWhite.Visibility = Visibility.Visible;
            PlayColorBlack.Visibility = Visibility.Visible;
            PlayColorLabel.Visibility = Visibility.Visible;
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
        #endregion

        /// <summary>
        /// Get coordinates with textBoxs for moved figure
        /// </summary>
        /// <param name="letter">Letter coordinate</param>
        /// <param name="number">Number coordinate</param>
        /// <returns></returns>
        private string GetCurrentFigureCoordinate(TextBox letter, TextBox number)
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
        /// Manage game with run King or Standard
        /// </summary>
        /// <param name="tupl">coordinates current and target</param>
        private void GameManager((string, string) tupl)
        {
            switch (currentGameStatus)
            {
                case 1:
                    if (manager.IsVAlidCoordinate(tupl.Item1, tupl.Item2))
                    {
                        manager.MateMessage += MessageMate;
                        manager.Logic();
                    }
                    break;
                case 3:
                    standard.removePicture += RemoveFigurePicture;
                    if (standard.IsVAlidCoordinate(tupl.Item1, tupl.Item2))
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
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set all figures for Standard game
        /// </summary>
        public void SetAllFigures()
        {
            currentFigureColor = "White";
            standard.SetAllFigures();
        }

        /// <summary>
        /// Button for change a pawn and new figure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            string[] tempFigure = GetChangeFigureImage(currentFigureColor).Split('/');
            string color = tempFigure[tempFigure.Length - 1].Split('.')[0];
            string figure = tempFigure[tempFigure.Length - 1].Split('.')[1];
            string inputInfo = figure + '.' + color;
            standard.SetChangeFigureForPawn(inputInfo);
            ChooseFigureForPawn.IsEnabled = false;
            ChooseButton.IsEnabled = false;
            MessageHandle.Text = " ";
        }
        
    }
}
