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
        private bool _isPlayed = false;
        private int _iBlack = 0;
        private int _jBlack = 0;
        private int _iWhite = 0;
        private int _jWhite = 0;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            ShowPanels();
        }

        #region Methods for Events
        private void InitializeGameManagment()
        {
            gameManagment = new GameManagment(currentFigureColor, currentGameStatus);
            gameManagment.DeletePicture += RemoveDeletedFigure;
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

        /// <summary>
        /// removes eaten figure to deleted tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void RemoveDeletedFigure(object sender, string coordinate)
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

        /// <summary>
        /// showing the progress of the King's game, the duraation of the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// possitioing the selected figure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// play button of the King's game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// moving the pice durring the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// the message of the Check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
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

        /// <summary>
        /// button for the positioning the Knight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KnightSetButton(object sender, RoutedEventArgs e)
        {
            if (GetCurrentFigureCoordinate(KnightStartLetter, KnightStartNumber, out string currentCoord))
            {
                gameManagment.CreateStartKnight(currentCoord);
                KnightSetBtn.IsEnabled = false;
            }
        }
        /// <summary>
        /// showing the amount the moves of the Knight
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// removes the pices from the 'eaten tab'
        /// </summary>
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
                    if (image1.Tag.ToString().Contains(name))
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
            MovesTextBox.Text = string.Empty;
            ProgressTextBox.Text = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetStandardGame_Click(object sender, RoutedEventArgs e)
        {
            ResetBoard(currentGameStatus);
            ResetDeleteBoard();
            ShowStandardGamePanel();
            ShowUserPanelForStandardGame();
            gameManagment.SetAllFigures();
            MovesTextBoxStandard.Text = string.Empty;
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
            KnightMovesMessage.Text = string.Empty;
            KnightStartLetter.Text = string.Empty;
            KnightStartNumber.Text = string.Empty;
            KnightTargetLetter.Text = string.Empty;
            KnightTargetNumber.Text = string.Empty;
        }

        #endregion

        #region Drag and Drop

        /// <summary>
        /// method of the drag when mousedowning on figure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Board_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var image = e.Source as Image;
                string imageName = image.Source.ToString();
                if (image != null && imageName.Contains(currentFigureColor))
                {
                    _dragObjectImage = image;
                    int coordX = Grid.GetColumn(image);
                    int coordY = Grid.GetRow(image);
                    _startCoordinate = $"{coordX}.{coordY}";
                    var colorsForFigure = GameManagment.GetAvalibleMoves(_startCoordinate);
                    if (colorsForFigure != null)
                        GetColoredCells(colorsForFigure);
                    DragDrop.DoDragDrop(image, _dragObjectImage, DragDropEffects.Move);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You did not choose the color for game");
            }
        }

        /// <summary>
        /// method of the drop when mouseuping on board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_Drop(object sender, DragEventArgs e)
        {
            int coordX = Grid.GetColumn((UIElement)e.OriginalSource);
            int coordY = Grid.GetRow((UIElement)e.OriginalSource);
            if ($"{coordX}.{coordY}" != _startCoordinate)
            {
                if (gameManagment.Managment((_startCoordinate, $"{coordX}.{coordY}")))
                    CurrentColorManager();
            }
            RemoveColoredCells();
        }

        #endregion

        #region MenuStrip

        /// <summary>
        /// abot button of the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Game Create Arsen Dashyan");
        }

        /// <summary>
        /// the King's game button of the menubar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            ShowUserPanelForStandardGame();
            InitializeGameManagment();
            gameManagment.SetAllFigures();
            MovesTextBoxStandard.Text = string.Empty;
            currentGameStatus = 3;
            MessageBox.Show("You Change A Standard Game, Good Luck");
        }

        /// <summary>
        /// Shows the standart game's panel
        /// </summary>
        private void ShowStandardGamePanel()
        {
            StandardGamePanel.Visibility = Visibility.Visible;
            PanelForGame.SelectedIndex = 2;
            KingGamePanel.Visibility = Visibility.Collapsed;
            KnightMovesPanel.Visibility = Visibility.Collapsed;
            PawnChangePanel.Visibility = Visibility.Collapsed;
            StartGamePanel.Visibility = Visibility.Collapsed;
        }
        private void ShowUserPanelForStandardGame()
        {
            PlayColorBlackStandard.Visibility = Visibility.Visible;
            PlayColorWhiteStandard.Visibility = Visibility.Visible;
            FirstUserName.Visibility = Visibility.Visible;
            SecondUserName.Visibility = Visibility.Visible;
            PlayForStandard.Visibility = Visibility.Visible;
            FirstUserName.Text = string.Empty;
            SecondUserName.Text = string.Empty;
        }
        private void HideUserPanelForStandardGame()
        {
            PlayForStandard.Visibility = Visibility.Hidden;
            PlayColorBlackStandard.Visibility = Visibility.Hidden;
            PlayColorWhiteStandard.Visibility = Visibility.Hidden;
            FirstUserName.Visibility = Visibility.Hidden;
            SecondUserName.Visibility = Visibility.Hidden;
        }
        private void ShowKnightGamePanel()
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
        private void ShowKingGamePanel()
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

        /// <summary>
        ///  in the starting point shows the panel
        /// </summary>
        private void ShowPanels()
        {
            PanelForGame.SelectedIndex = 4;
            KingGamePanel.Visibility = Visibility.Collapsed;
            StandardGamePanel.Visibility = Visibility.Collapsed;
            KnightMovesPanel.Visibility = Visibility.Collapsed;
            PawnChangePanel.Visibility = Visibility.Collapsed;
            SomeGameLabel.Visibility = Visibility.Hidden;
            SomeGameLabel2.Visibility = Visibility.Hidden;
            SomeGameComboBox.Visibility = Visibility.Hidden;
            DateTimeComboBox.Visibility = Visibility.Hidden;
            CheckUsers.Visibility = Visibility.Hidden;
            SomeGamePlay.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// button for your selected game's type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseGameType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (GameTypeComboBox.Text)
                {
                    case "King Game":
                        currentGameStatus = 1;
                        ShowKingGamePanel();
                        InitializeGameManagment();
                        break;
                    case "Knight Moves":
                        currentGameStatus = 2;
                        ShowKnightGamePanel();
                        InitializeGameManagment();
                        break;
                    case "Standard Game":
                        currentGameStatus = 3;
                        SomeGameLabel.Visibility = Visibility.Visible;
                        SomeGameLabel2.Visibility = Visibility.Visible;
                        SomeGameComboBox.Visibility = Visibility.Visible;
                        GetItemsNotEndedComboBox();
                        CheckUsers.Visibility = Visibility.Visible;
                        GameTypeComboBox.Visibility = Visibility.Hidden;
                        ChooseGameType.Visibility = Visibility.Hidden;
                        SomeGamePlay.Visibility = Visibility.Visible;
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You don't choose game type");
            }
        }

        /// <summary>
        /// filling the user's name in the Combobox from the DB
        /// </summary>
        private void GetItemsNotEndedComboBox()
        {
            using (ChessDBContext context = new ChessDBContext())
            {
                var list = context.Users.ToList();
                foreach (var item in list)
                {
                    SomeGameComboBox.Items.Add($"{item.Name} - {item.Opponent}");
                }
            }
        }

        /// <summary>
        /// filling the selected users' games sorting by the date
        /// </summary>
        private void GetItemsCheckUsersComboBox()
        {
            var result = SomeGameComboBox.Text.Split('-');
            using (ChessDBContext context = new ChessDBContext())
            {
                var userList = context.Users.ToList();
                var gameList = context.Games.ToList();
                if (userList.Filtr(u => result[0].Contains(u.Name) && result[1].Contains(u.Opponent), out User resultUser))
                {
                    foreach (var item in gameList)
                    {
                        if (item.UserId == resultUser.Id)
                            DateTimeComboBox.Items.Add($"{item.DateTime}");
                    }
                }
            }
        }

        /// <summary>
        /// button that starting the doesn't endeed game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SomeGamePlay_Click(object sender, RoutedEventArgs e)
        {
            if (SomeGameComboBox.Text == string.Empty)
            {
                InitializeGameManagment();
                ShowStandardGamePanel();
                ShowUserPanelForStandardGame();
                gameManagment.SetAllFigures();
                MovesTextBoxStandard.Text = string.Empty;
            }
            else if (DateTimeComboBox.IsTextSearchEnabled)
            {
                string json = string.Empty;
                using (var context = new ChessDBContext())
                {
                    var gameList = context.Games.ToList();
                    if (gameList.Filtr(g => $"{g.DateTime}" == DateTimeComboBox.SelectedItem.ToString(), out Game game))
                    {
                        json = game.GameCondition;
                        MovesTextBoxStandard.Text = game.GameStory;
                        currentFigureColor = game.CurrentColor;
                        if (currentFigureColor == "White")
                            _colorPower = false;
                        else
                            _colorPower = true;
                    }
                }
                InitializeGameManagment();
                ShowStandardGamePanel();
                HideUserPanelForStandardGame();
                gameManagment.SetConditionFigures(json);
                MessageBox.Show($"Current Color is {currentFigureColor}");
            }
        }
        #endregion

        #region Standard Game

        /// <summary>
        /// button that filling the selected users' game into Combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckUsers_Click(object sender, RoutedEventArgs e)
        {
            DateTimeComboBox.Visibility = Visibility.Visible;
            SomeGamePlay.Visibility = Visibility.Visible;
            GetItemsCheckUsersComboBox();
        }

        /// <summary>
        /// Manage current color with Black or White
        /// </summary>
        private void CurrentColorManager()
        {
            _isPlayed = true;
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

        /// <summary>
        /// play button which appearing after choosing new standart game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                HideUserPanelForStandardGame();
                InitializeGameManagment();
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

        /// <summary>
        /// adds standart game's info into DB
        /// </summary>
        public void AddGameStoryInDB()
        {
            using (var context = new ChessDBContext())
            {
                var userList = context.Users.ToList();
                var user = new User()
                {
                    Name = FirstUserName.Text.ToLower(),
                    Opponent = SecondUserName.Text.ToLower(),
                };
                var gameDetail = new Game()
                {
                    GameCondition = GameManagment.GetAllFiguresForSave(),
                    GameStory = MovesTextBoxStandard.Text,
                    DateTime = DateTime.Now,
                    CurrentColor = currentFigureColor
                };
                if (userList.Filtr(u => u.Name == user.Name && u.Opponent == user.Opponent, out User resultUser))
                {
                    gameDetail.UserId = resultUser.Id;
                    context.Games.Add(gameDetail);
                    context.SaveChanges();
                }
                else
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    gameDetail.UserId = user.Id;
                    context.Games.Add(gameDetail);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// when clicking on the pice showing te available moves
        /// </summary>
        /// <param name="list"> fiure's availabele coordinates</param>
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
                            border.Background = Brushes.LightGoldenrodYellow;
                    }
                }
            }
        }

        /// <summary>
        /// remove the available moves' coloures from the board
        /// </summary>
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
            var temp2 = _startCoordinate.Split('.');
            return list.Where(c => c != (int.Parse(temp2[0]), int.Parse(temp2[1]))).ToList();
        }
        protected override void OnClosed(EventArgs e)
        {
            if (_isPlayed)
            {
                var result = MessageBox.Show("Saved the current game?",
                                "Message",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                    AddGameStoryInDB();
                else
                    base.OnClosed(e);
            }
            else
                base.OnClosed(e);
        }

        #endregion
    }
}
