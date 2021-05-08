using GameManager;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        private MovesKnight movesKnight;
        private Standard standard;
        private List<string> models = new();
        CancellationTokenSource cancellationTokenSource;
        CancellationToken cancellationToken;
        public static int currentGameStatus;
        public string currentFigureColor;
        private string startCoordinate;
        private int countForKnightMoves = 0;
        private bool colorPower = false;
        private string changedFigureType;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            InitializeStandard();
            InitializeKingGame();
            InitializeKnightGame();
        }

        #region Methods for Events

        private void InitializeStandard()
        {
            standard = new Standard();
            standard.setPicture += SetFigurePicture;
            standard.removePicture += RemoveFigurePicture;
            standard.messageForMove += MessageMoveForStandardGame;
            standard.messageCheck += MessageCheck;
            standard.messageForPawnChange += MessageForPawnChange;
        }
        private void InitializeKingGame()
        {
            manager = new Manager(currentFigureColor);
            manager.setPicture += SetFigurePicture;
            manager.removePicture += RemoveFigurePicture;
            manager.messageForMove += MessageMoveForKingGame;
        }
        private void InitializeKnightGame()
        {
            movesKnight = new MovesKnight();
            movesKnight.setPicture += SetFigurePicture;
            movesKnight.messageForMove += delegate { };
        }

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
        public void MessageMoveForKingGame(object sender, (string, string) coordinateTupl)
        {
            if (coordinateTupl.Item1 != string.Empty)
            {
                string[] start = coordinateTupl.Item1.Split('.');
                string[] target = coordinateTupl.Item2.Split('.');
                MovesTextBox.Text += $"{start[2]} {start[3]} -" + $"{start[0]}{start[1]} : " +
                    $"{target[0]}{target[1]}\n";
            }
        }
        /// <summary>
        /// Show a figure moves
        /// </summary>
        /// <param name="baseFigure">Figure instanste</param>
        /// <param name="coordinate">Figure old and new coordinate</param>
        public void MessageMoveForStandardGame(object sender, (string, string) coordinateTupl)
        {
            if (coordinateTupl.Item1 != string.Empty)
            {
                string[] start = coordinateTupl.Item1.Split('.');
                string[] target = coordinateTupl.Item2.Split('.');
                MovesTextBoxStandard.Text += $"{start[2]} {start[3]} - " + $"{start[0]}{start[1]} : " +
                    $"{target[0]}{target[1]}\n";
            }
        }

        /// <summary>
        /// Initialize a MessageForPawnChange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void MessageForPawnChange(object sender, string message)
        {
            PawnChengesPanel.Visibility = Visibility.Visible;
            MessageHandleStandard.Text = message;
        }
        public async void MessageForProgress(object sender, (string, string) e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            int completedPercentage = 0;
            for (int i = 0; i < 100; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                try
                {
                    await Task.Delay(1000, cancellationToken);
                    completedPercentage = (i + 1);
                }
                catch (Exception)
                {
                    completedPercentage = i;
                }
                Progress.Value = completedPercentage;
            }
            string messageForProgress = cancellationToken.IsCancellationRequested ?
                string.Format($"Process was cancelled at {completedPercentage}%") :
                "Process completed normally";
            ProgressTextBox.Text = messageForProgress;
            Progress.Value = 0;
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
            if (GetCurrentFigureCoordinate(InputCoordinatsLetter, InputCoordinatsNumber, out string coord))
            {
                inputInfo += coord;
                manager.IsValidForPleacement(inputInfo);
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
                    manager.messageForProgress += MessageForProgress;
                    MessageBox.Show("Good Luck!!!");
                }
            }
        }
        private void InstallButton(object sender, RoutedEventArgs e)
        {
            PlayB2.IsEnabled = false;
            if (GetCurrentFigureNew(out (string, string) tempCoord))
            {
                var tupl = tempCoord;
                GameManager(tupl);
            }
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
            cancellationTokenSource.Cancel();
        }
        public void MessageCheck(object sender, string message)
        {
            MessageHandle.Text = message;
            MessageHandleStandard.Text = message;
        }

        /// <summary>
        /// Get a figure image source
        /// </summary>
        /// <param name="name">Figure name</param>
        /// <returns>Return the instance image source</returns>
        private static string GetCurrentFigureImage(string name)
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
        private bool GetCurrentFigureNew(out (string, string) tuplCoord)
        {
            if (GetCurrentFigureCoordinate(InputCoordinatsLetter_Corrent, InputCoordinatsNumber_Corrent, out string currentCoord)
                            && GetCurrentFigureCoordinate(InputCoordinatsLetter_Selected, InputCoordinatsNumber_Selected, out string targetCoord))
            {
                string current = currentCoord;
                string target = targetCoord;
                tuplCoord = (current, target);
                return true;
            }
            tuplCoord = ("", "");
            return false;
        }

        #endregion

        #region For Knight Moves

        private void KnightSetButton(object sender, RoutedEventArgs e)
        {
            if (GetCurrentFigureCoordinate(KnightStartLetter, KnightStartNumber, out string currentCoord))
            {
                movesKnight.CreateStartKnight(currentCoord);
                KnightSetBtn.IsEnabled = false;
            }
        }
        private void KnightMoveCheck_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentFigureCoordinate(KnightTargetLetter, KnightTargetNumber, out string currentCoord))
            {
                movesKnight.CreateTargetKnight(currentCoord);
                countForKnightMoves = movesKnight.MinKnightCount();
                KnightMovesMessage.Text = $"For target coordinate your need {countForKnightMoves} moves";
                countForKnightMoves = 0;
                KnightSetBtn.IsEnabled = true;
            }
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
            MovesTextBox.Text = " ";
            MessageHandle.Text = " ";
            ProgressTextBox.Text = " ";
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
                    models = MovesKnight.GetNamesForReset();
                    break;
                case 3:
                    InitializeStandard();
                    models = Standard.GetNamesForReset();
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
            cancellationTokenSource.Cancel();
            Progress.Value = 0;
            manager.setPicture += SetFigurePicture;
            manager.removePicture += RemoveFigurePicture;
            manager.messageForMove += MessageMoveForKingGame;
        }

        /// <summary>
        /// Reset button the Board for start Knight game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_ButtonForKnight(object sender, RoutedEventArgs e)
        {
            ResetBoard();
            KnightSetBtn.IsEnabled = true;
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
            try
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
            catch (Exception)
            {
                MessageBox.Show("You did not choose the color for game");
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
            ResetBoard();
            ShowKingGamePanel();
            currentGameStatus = 1;
            MessageBox.Show("You Change A King Game, Good Luck");
        }
        private void KnightGame_Click(object sender, RoutedEventArgs e)
        {
            ResetBoard();
            ShowKnightGamePanel();
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
            ResetBoard();
            ShowStandardGamePanel();
            InitializeStandard();
            standard.SetAllFigures();
            currentGameStatus = 3;
            MessageBox.Show("You Change A Standard Game, Good Luck");
        }
        public void ShowStandardGamePanel()
        {
            StandardGamePanel.Visibility = Visibility.Visible;
            KingGamePanel.Visibility = Visibility.Collapsed;
            KnightMovesPanel.Visibility = Visibility.Collapsed;
            PawnChengesPanel.Visibility = Visibility.Hidden;
            InputCoordinatsLetter_Corrent.IsEnabled = true;
            InputCoordinatsNumber_Corrent.IsEnabled = true;
            InputCoordinatsLetter_Selected.IsEnabled = true;
            InputCoordinatsNumber_Selected.IsEnabled = true;
            InstalB3.IsEnabled = true;
        }
        public void ShowKnightGamePanel()
        {
            KnightMovesPanel.Visibility = Visibility.Visible;
            StandardGamePanel.Visibility = Visibility.Collapsed;
            KingGamePanel.Visibility = Visibility.Collapsed;
            InputCoordinatsLetter_Corrent.IsEnabled = true;
            InputCoordinatsNumber_Corrent.IsEnabled = true;
            InputCoordinatsLetter_Selected.IsEnabled = true;
            InputCoordinatsNumber_Selected.IsEnabled = true;
            InstalB3.IsEnabled = true;
            PawnChengesPanel.Visibility = Visibility.Hidden;
        }
        public void ShowKingGamePanel()
        {
            KingGamePanel.Visibility = Visibility.Visible;
            StandardGamePanel.Visibility = Visibility.Collapsed;
            KnightMovesPanel.Visibility = Visibility.Collapsed;
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
        private static bool GetCurrentFigureCoordinate(TextBox letter, TextBox number, out string coordInfo)
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
                            coordInfo = $"{o - 1}.{j - 1}";
                            return true;
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
            coordInfo = string.Empty;
            return false;
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
            }
        }

        /// <summary>
        /// Set all figures for Standard game
        /// </summary>
        public void SetAllFigures()
        {
            standard.SetAllFigures();
        }
        private void PlayForStandard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PlayColorWhiteStandard.IsChecked == true)
                {
                    currentFigureColor = "White";
                    colorPower = false;
                }
                else if (PlayColorBlackStandard.IsChecked == true)
                {
                    currentFigureColor = "Black";
                    colorPower = true;
                }
                InitializeStandard();
                PlayColorWhite.IsEnabled = false;
                PlayColorBlack.IsEnabled = false;
                PlayForStandard.IsEnabled = false;
                MessageBox.Show("Good Luck!!!");
            }
            catch (Exception)
            {
                MessageBox.Show("You did not choose the color for game");
                throw;
            }

        }

        #region Pawn Changed Figure

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
            PawnChengesPanel.Visibility = Visibility.Hidden;
            MessageHandle.Text = " ";
        }
        private string GetChangeFigureImage(string color)
        {
            string result = string.Empty;
            if (color == "White")
            {
                result = changedFigureType.BlackFigurePath();
            }
            else
            {
                result = changedFigureType.WhiteFigurePath();
            }
            return result;
        }
        private void ChangeQueenBtn_Click(object sender, RoutedEventArgs e)
        {
            changedFigureType = "Queen";
        }

        private void ChangeRookBtn_Click(object sender, RoutedEventArgs e)
        {
            changedFigureType = "Rook";
        }

        private void ChangeBishopBtn_Click(object sender, RoutedEventArgs e)
        {
            changedFigureType = "Bishop";
        }

        private void ChangeKnightBtn_Click(object sender, RoutedEventArgs e)
        {
            changedFigureType = "Knight";
        }

        #endregion
    }
}
