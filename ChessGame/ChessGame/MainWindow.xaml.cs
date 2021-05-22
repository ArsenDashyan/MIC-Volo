using GameManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChessGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Property and Feld
        private UIElement _dragObjectImage;
        private readonly List<string> _modelsForDeleteid = new();
        private readonly List<(Brush, Border)> _colorsBorder = new();
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        public GameManagment gameManagment;
        public int currentGameStatus;
        public string currentFigureColor;
        private string _startCoordinate;
        private bool _colorPower = false;
        private int _iBlack = 0;
        private int _jBlack = 0;
        private int _iWhite = 0;
        private int _jWhite = 0;
        private static string _gameStory;
        private static string _names;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            InitializeGameManagment();
            ShowPanels();
        }

        #region Methods for Events
        private void InitializeGameManagment()
        {
            gameManagment = new GameManagment(currentFigureColor, currentGameStatus);
            gameManagment.DeletePicture += SetDeleteFigurePicture;
            gameManagment.MateMessage += MessageMateForKingHame;
            gameManagment.MessageCheck += MessageCheckStandard;
            gameManagment.MessageForMove += MessageMoveForAllGame;
            gameManagment.MessagePawnChange += MessageForPawnChange;
            gameManagment.MessageProgress += MessageForProgress;
            gameManagment.RemovePicture += RemoveFigurePicture;
            gameManagment.SetPicture += SetFigurePicture;
            gameManagment.InitializeGameManagmentComponent();
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
            Image image = new();
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
        public void SetDeleteFigurePicture(object sender, string coordinate)
        {
            string[] coord = coordinate.Split('.');
            string str = coord[2] + '.' + coord[3] + '.' + coord[4];
            Image image = new();
            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(GetCurrentFigureImage(str), UriKind.Relative);
            bitmap.EndInit();
            image.Source = bitmap;
            image.Tag = str;
            _modelsForDeleteid.Add(str);
            if (str.Contains("White"))
            {
                if (_iBlack == 5)
                {
                    _iBlack = 0;
                    _jBlack++;
                }
                Grid.SetColumn(image, _iBlack++);
                Grid.SetRow(image, _jBlack);
                WhiteDeleteFigure.Children.Add(image);
            }
            else
            {
                if (_iWhite == 5)
                {
                    _iWhite = 0;
                    _jWhite++;
                }
                Grid.SetColumn(image, _iWhite++);
                Grid.SetRow(image, _jWhite);
                BlackDeleteFigure.Children.Add(image);
            }
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
        public void MessageMoveForAllGame(object sender, (string, string) coordinateTupl)
        {
            if (coordinateTupl.Item1 != string.Empty)
            {
                TextBox textBox;
                if (currentGameStatus == 3)
                    textBox = MovesTextBoxStandard;
                else
                    textBox = MovesTextBox;
                string[] start = coordinateTupl.Item1.Split('.');
                string[] target = coordinateTupl.Item2.Split('.');
                textBox.Text += $"{start[2]} {start[3]} - " + $"{start[0]}{start[1]} : " +
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
            StandardGamePanel.Visibility = Visibility.Collapsed;
            PawnChangePanel.Visibility = Visibility.Visible;
            PanelForGame.SelectedIndex = 3;
        }
        public async void MessageForProgress(object sender, (string, string) e)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            int completedPercentage = 0;
            for (int i = 0; i < 100; i++)
            {
                if (_cancellationToken.IsCancellationRequested)
                    break;
                try
                {
                    await Task.Delay(1000, _cancellationToken);
                    completedPercentage = (i + 1);
                }
                catch (Exception)
                {
                    completedPercentage = i;
                }
                Progress.Value = completedPercentage;
            }
            string messageForProgress = _cancellationToken.IsCancellationRequested ?
                string.Format($"Process was cancelled at {completedPercentage}%") :
                "Process completed normally";
            ProgressTextBox.Text = messageForProgress;
            Progress.Value = 0;
        }

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

        #endregion

        #region King game Button
        private void PleacementB1_Click(object sender, RoutedEventArgs e)
        {
            string[] tempFigure = GetCurrentFigureImage().Split('/');
            string color = tempFigure[^1].Split('.')[0];
            string figure = tempFigure[^1].Split('.')[1];
            string inputInfo = figure + '.' + color + '.';
            if (GetCurrentFigureCoordinate(InputCoordinatsLetter, InputCoordinatsNumber, out string coord))
            {
                inputInfo += coord;
                GameManagment.IsValidForPleacement(inputInfo);
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
                    gameManagment.MessageProgress += MessageForProgress;
                    MessageBox.Show("Good Luck!!!");
                }
            }
        }
        private void InstallButton(object sender, RoutedEventArgs e)
        {
            PlayB2.IsEnabled = false;
            if (GetCurrentFigureNew(out (string, string) tempCoord))
                gameManagment.Managment(tempCoord);
        }

        /// <summary>
        /// Show a Mate message
        /// </summary>
        /// <param name="sender">figure</param>
        /// <param name="message">Mate</param>
        public void MessageMateForKingHame(object sender, string message)
        {
            MessageBox.Show(message);
            _cancellationTokenSource.Cancel();
        }
        public static void MessageCheckStandard(object sender, string message)
        {
            if (message != " ")
                MessageBox.Show(message);
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
                gameManagment = new(currentFigureColor, currentGameStatus);
                return true;
            }
            else if (PlayColorBlack.IsChecked == true)
            {
                currentFigureColor = "Black";
                gameManagment = new(currentFigureColor, currentGameStatus);
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
                gameManagment.CreateStartKnight(currentCoord);
                KnightSetBtn.IsEnabled = false;
            }
        }
        private void KnightMoveCheck_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentFigureCoordinate(KnightTargetLetter, KnightTargetNumber, out string currentCoord))
            {
                gameManagment.CreateTargetKnight(currentCoord);
                KnightMovesMessage.Text = $"For target coordinate your need " +
                                     $"{GameManagment.MinKnightCount()} moves";
                KnightSetBtn.IsEnabled = true;
            }
        }

        #endregion

        #region Reset Method and buttons

        /// <summary>
        /// Reset Board for start game
        /// </summary>
        private void ResetBoard(int status)
        {
            var models = GameManagment.GetAllFiguresForReset(status);
            if (models != null)
            {
                foreach (var item in models)
                {
                    RemovePicture(item, Board);
                }
            }
        }
        private void ResetDeleteBoard()
        {
            foreach (var item in _modelsForDeleteid)
            {
                if (item.Contains("White"))
                    RemovePicture(item, WhiteDeleteFigure);
                else
                    RemovePicture(item, BlackDeleteFigure);
            }
        }

        /// <summary>
        /// Remove figure picture from board
        /// </summary>
        /// <param name="name">Figure name</param>
        public static void RemovePicture(string name, Grid grid)
        {
            foreach (var item in grid.Children)
            {
                if (item is Image image1)
                {
                    if (name == image1.Tag.ToString())
                    {
                        grid.Children.Remove(image1);
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
            ResetBoard(currentGameStatus);
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
            _cancellationTokenSource.Cancel();
            Progress.Value = 0;
            MovesTextBox.Text = " ";
            ProgressTextBox.Text = " ";
        }
        private void ResetStandardGame_Click(object sender, RoutedEventArgs e)
        {
            _gameStory = MovesTextBoxStandard.Text;
            AddGameStoryWithNote(_names);
            _names = string.Empty;
            _gameStory = string.Empty;
            ResetBoard(currentGameStatus);
            ResetDeleteBoard();
            ShowStandardGamePanel();
            gameManagment.SetAllFigures();
            currentGameStatus = 3;
            MessageBox.Show("You Change A Standard Game, Good Luck");
        }

        /// <summary>
        /// Reset button the Board for start Knight game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_ButtonForKnight(object sender, RoutedEventArgs e)
        {
            ResetBoard(currentGameStatus);
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
                    this._dragObjectImage = image;
                    int coordX = Grid.GetColumn(image);
                    int coordY = Grid.GetRow(image);
                    this._startCoordinate = $"{coordX}.{coordY}";
                    var colorsForFigure = GameManagment.GetAvalibleMoves(this._startCoordinate);
                    if (colorsForFigure != null)
                        GetColoredCells(colorsForFigure);
                    DragDrop.DoDragDrop(image, this._dragObjectImage, DragDropEffects.Move);
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
            if (coordinate != this._startCoordinate)
            {
                bool action = gameManagment.Managment((this._startCoordinate, coordinate));
                CurrentColorManager(action);
            }
            RemoveColoredCells();
        }

        #endregion

        #region MenuStrip
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Game Create Arsen Dashyan");
        }
        private void KingGame_Click(object sender, RoutedEventArgs e)
        {
            ResetBoard(currentGameStatus);
            ShowKingGamePanel();
            currentGameStatus = 1;
            MessageBox.Show("You Change A King Game, Good Luck");
        }
        private void KnightGame_Click(object sender, RoutedEventArgs e)
        {
            ResetBoard(currentGameStatus);
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
            ResetBoard(currentGameStatus);
            ShowStandardGamePanel();
            currentGameStatus = 3;
            MessageBox.Show("You Change A Standard Game, Good Luck");
        }
        public void ShowStandardGamePanel()
        {
            gameManagment.SetAllFigures();
            StandardGamePanel.Visibility = Visibility.Visible;
            PanelForGame.SelectedIndex = 2;
            KingGamePanel.Visibility = Visibility.Collapsed;
            KnightMovesPanel.Visibility = Visibility.Collapsed;
            PawnChangePanel.Visibility = Visibility.Collapsed;
            StartGamePanel.Visibility = Visibility.Collapsed;
            PlayForStandard.Visibility = Visibility.Visible;
            PlayColorBlackStandard.Visibility = Visibility.Visible;
            PlayColorWhiteStandard.Visibility = Visibility.Visible;
            FirstUserName.Visibility = Visibility.Visible;
            SecondUserName.Visibility = Visibility.Visible;
            InputCoordinatsLetter_Corrent.IsEnabled = true;
            InputCoordinatsNumber_Corrent.IsEnabled = true;
            InputCoordinatsLetter_Selected.IsEnabled = true;
            InputCoordinatsNumber_Selected.IsEnabled = true;
            InstalB3.IsEnabled = true;
            MovesTextBoxStandard.Text = " ";
            FirstUserName.Text = " ";
            SecondUserName.Text = " ";
        }
        public void ShowKnightGamePanel()
        {
            KnightMovesPanel.Visibility = Visibility.Visible;
            PanelForGame.SelectedIndex = 1;
            StandardGamePanel.Visibility = Visibility.Collapsed;
            PawnChangePanel.Visibility = Visibility.Collapsed;
            KingGamePanel.Visibility = Visibility.Collapsed;
            StartGamePanel.Visibility = Visibility.Collapsed;
            InputCoordinatsLetter_Corrent.IsEnabled = true;
            InputCoordinatsNumber_Corrent.IsEnabled = true;
            InputCoordinatsLetter_Selected.IsEnabled = true;
            InputCoordinatsNumber_Selected.IsEnabled = true;
            InstalB3.IsEnabled = true;
        }
        public void ShowKingGamePanel()
        {
            KingGamePanel.Visibility = Visibility.Visible;
            PanelForGame.SelectedIndex = 0;
            StandardGamePanel.Visibility = Visibility.Collapsed;
            KnightMovesPanel.Visibility = Visibility.Collapsed;
            PawnChangePanel.Visibility = Visibility.Collapsed;
            StartGamePanel.Visibility = Visibility.Collapsed;
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

        public void ShowPanels()
        {
            PanelForGame.SelectedIndex = 4;
            KingGamePanel.Visibility = Visibility.Collapsed;
            StandardGamePanel.Visibility = Visibility.Collapsed;
            KnightMovesPanel.Visibility = Visibility.Collapsed;
            PawnChangePanel.Visibility = Visibility.Collapsed;
            SomeGameLabel.Visibility = Visibility.Hidden;
            SomeGameComboBox.Visibility = Visibility.Hidden;
            SomeGamePlay.Visibility = Visibility.Hidden;
        }
        private void ChooseGameType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (GameTypeComboBox.Text)
                {
                    case "King Game":
                        currentGameStatus = 1;
                        ShowKingGamePanel();
                        break;
                    case "Knight Moves":
                        currentGameStatus = 2;
                        ShowKnightGamePanel();
                        break;
                    case "Standard Game":
                        currentGameStatus = 3;
                        SomeGameLabel.Visibility = Visibility.Visible;
                        SomeGameComboBox.Visibility = Visibility.Visible;
                        SomeGamePlay.Visibility = Visibility.Visible;
                        GameTypeComboBox.Visibility = Visibility.Hidden;
                        ChooseGameType.Visibility = Visibility.Hidden;
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You don't choose game type");
            }
        }

        private void SomeGamePlay_Click(object sender, RoutedEventArgs e)
        {
            if (SomeGameComboBox.Text == string.Empty)
            {
                ShowStandardGamePanel();
            }
            else
            {

            }

        }

        #endregion

        #region Standard Game

        /// <summary>
        /// Manage current color with Black or White
        /// </summary>
        private void CurrentColorManager(bool action)
        {
            if (action)
            {
                if (currentGameStatus == 3)
                {
                    if (_colorPower)
                    {
                        currentFigureColor = "White";
                        _colorPower = false;
                    }
                    else
                    {
                        currentFigureColor = "Black";
                        _colorPower = true;
                    }
                }
            }

        }
        private void PlayForStandard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PlayColorWhiteStandard.IsChecked == true)
                {
                    currentFigureColor = "White";
                    _colorPower = false;
                }
                else if (PlayColorBlackStandard.IsChecked == true)
                {
                    currentFigureColor = "Black";
                    _colorPower = true;
                }
                _names = $"{FirstUserName.Text}{SecondUserName.Text}";
                PlayForStandard.Visibility = Visibility.Hidden;
                PlayColorBlackStandard.Visibility = Visibility.Hidden;
                PlayColorWhiteStandard.Visibility = Visibility.Hidden;
                FirstUserName.Visibility = Visibility.Hidden;
                SecondUserName.Visibility = Visibility.Hidden;
                MessageBox.Show("Good Luck!!!");
            }
            catch (Exception)
            {
                MessageBox.Show("You did not choose the color for game");
                throw;
            }
        }

        /// <summary>
        /// Button for change a pawn and new figure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            string result = string.Empty;
            if (CheckBishop.IsChecked == true)
                result = "Bishop";
            else if (CheckKnight.IsChecked == true)
                result = "Knight";
            else if (CheckQuuen.IsChecked == true)
                result = "Queen";
            else if (CheckRook.IsChecked == true)
                result = "Rook";
            string path = currentFigureColor == "White" ? result.BlackFigurePath()
                                                        : result.WhiteFigurePath();
            string[] tempFigure = path.Split('/');
            string color = tempFigure[^1].Split('.')[0];
            string figure = tempFigure[^1].Split('.')[1];
            string inputInfo = figure + '.' + color;
            gameManagment.SetChangeFigureForPawn(inputInfo);
            StandardGamePanel.Visibility = Visibility.Visible;
            PawnChangePanel.Visibility = Visibility.Collapsed;
            PanelForGame.SelectedIndex = 2;
        }
        public static void AddGameStoryWithNote(string name)
        {
            string info = $"{name}:{DateTime.Now}" + "\n\n" + _gameStory;
            if (!File.Exists($"{name}.txt"))
            {
                File.Create($"{name}.txt").Close();
                File.WriteAllText($"{name}.txt", info);
            }
            else
                File.AppendAllText($"{name}.txt", "\n" + info);
        }
        private void GetColoredCells(List<string> list)
        {
            var coordinateList = GetCoordinateList(list);
            foreach (var item in Board.Children)
            {
                if (item is Border border)
                {
                    _colorsBorder.Add((border.Background, border));
                    foreach (var coord in coordinateList)
                    {
                        if (Grid.GetColumn(border) == coord.Item1 && Grid.GetRow(border) == coord.Item2)
                        {
                            border.Background = Brushes.LightGoldenrodYellow;
                        }
                    }
                }
            }
        }
        private void RemoveColoredCells()
        {
            foreach (var item in _colorsBorder)
            {
                item.Item2.Background = item.Item1;
            }
        }
        private List<(int, int)> GetCoordinateList(List<string> listColors)
        {
            var list = new List<(int, int)>();
            foreach (var item in listColors)
            {
                var temp = item.Split('.');
                list.Add((int.Parse(temp[0]), int.Parse(temp[1])));
            }
            var temp2 = this._startCoordinate.Split('.');
            return list.Where(c => c != (int.Parse(temp2[0]), int.Parse(temp2[1]))).ToList();
        }
        #endregion

        
    }
}
