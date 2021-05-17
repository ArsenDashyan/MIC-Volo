using System.Collections.Generic;

namespace GameManager
{
    public class GameManagment
    {
        #region Property and Feld
        public static int CurrentGameStatus { get => _currentGameStatus; set => _currentGameStatus = value; }
        private static int _currentGameStatus;
        private KingGame _kingGame;
        private MovesKnight _movesKnight;
        private Standard _standard;
        public event MessageForMate MateMessage;
        public event Message MessageForMove;
        public event Message MessageProgress;
        public event Picture SetPicture;
        public event Picture RemovePicture;
        public event MessageForMate MessagePawnChange;
        public event MessageForMate MessageCheck;
        public event Picture DeletePicture;
        public string currentFigureColor;


        #endregion
        public GameManagment(string currentFigureColor, int currentGameStatus)
        {
            this.currentFigureColor = currentFigureColor;
            CurrentGameStatus = currentGameStatus;
            InitializeKnightGame();
            InitializeStandard();
        }
        private void InitializeStandard()
        {
            _standard = new Standard();
            _standard.SetPicture += SetFigurePicture;
            _standard.RemovePicture += RemoveFigurePicture;
            _standard.DeletePicture += SetDeleteFigurePicture;
            _standard.MessageForMove += MessageMoveForStandardGame;
            _standard.MessageCheck += MessageCheckStandard;
            _standard.MessagePawnChange += MessageForPawnChange;
        }
        public void IsValidForPleacement(string inputInfo)
        {
            InitializeKingGame();
            _kingGame.IsValidForPleacement(inputInfo);
        }
        private void InitializeKingGame()
        {
            _kingGame = new KingGame(currentFigureColor);
            _kingGame.SetPicture += SetFigurePicture;
            _kingGame.RemovePicture += RemoveFigurePicture;
            _kingGame.MessageForMove += MessageMoveForKingGame;
            _kingGame.MessageProgress += MessageForProgress;
            _kingGame.MateMessage += MessageMate;
        }
        private void InitializeKnightGame()
        {
            _movesKnight = new MovesKnight();
            _movesKnight.SetPicture += SetFigurePicture;
            _movesKnight.MessageForMoveKnight += delegate { };
        }
        public bool Managment((string, string) tupl)
        {
            switch (CurrentGameStatus)
            {
                case 1:
                    InitializeKingGame();
                    if (_kingGame.IsVAlidCoordinate(tupl.Item1, tupl.Item2))
                        _kingGame.Logic();
                    return false;
                case 3:
                    InitializeStandard();
                    if (Standard.IsVAlidCoordinate(tupl.Item1, tupl.Item2))
                    {
                        _standard.GetLogic(tupl.Item1, tupl.Item2);
                        return true;
                    }
                    else
                        return false;
            }
            return false;
        }
        public void SetAllFigures()
        {
            InitializeStandard();
            _standard.SetAllFigures();
        }
        public static List<string> GetAvalibleMoves(string coordinate)
        {
            return CurrentGameStatus switch
            {
                1 => KingGame.GetAvalibleMoves(coordinate),
                3 => Standard.GetAvalibleMoves(coordinate),
                _ => null,
            };
        }
        public void SetChangeFigureForPawn(string inputInfo)
        {
            InitializeStandard();
           _standard.SetChangeFigureForPawn(inputInfo);
        }
        public static List<string> GetAllFiguresForReset(int status)
        {
            var models = new List<string>();
            switch (status)
            {
                case 1:
                    models = KingGame.GetNamesForReset();
                    break;
                case 2:
                    models = MovesKnight.GetNamesForReset();
                    break;
                case 3:
                    models = Standard.GetNamesForReset();
                    break;
            }
            return models;
        }
        public void MessageMate(object sender, string message)
        {
            MateMessage(sender, message);
        }
        public void MessageCheckStandard(object sender, string message)
        {
            MessageCheck(sender, message);
        }

        /// <summary>
        /// Set the figure image
        /// </summary>
        /// <param name="baseFigure">Figure instance</param>
        /// <param name="coordinate">Figure </param>
        public void SetFigurePicture(object sender, string coordinate)
        {
            SetPicture(this, coordinate);
            
        }
        public void SetDeleteFigurePicture(object sender, string coordinate)
        {
            DeletePicture(this, coordinate);
        }

        /// <summary>
        /// Remove the figure image
        /// </summary>
        /// <param name="baseFigure">Figure instance</param>
        /// <param name="coordinate">Figure </param>
        public void RemoveFigurePicture(object sender, string coordinate)
        {
            RemovePicture(this, coordinate);
        }

        /// <summary>
        /// Show a figure moves
        /// </summary>
        /// <param name="baseFigure">Figure instanste</param>
        /// <param name="coordinate">Figure old and new coordinate</param>
        public void MessageMoveForKingGame(object sender, (string, string) coordinateTupl)
        {
            MessageForMove(this, coordinateTupl);
        }

        /// <summary>
        /// Show a figure moves
        /// </summary>
        /// <param name="baseFigure">Figure instanste</param>
        /// <param name="coordinate">Figure old and new coordinate</param>
        public void MessageMoveForStandardGame(object sender, (string, string) coordinateTupl)
        {
            MessageForMove(this, coordinateTupl);
        }

        /// <summary>
        /// Initialize a MessageForPawnChange event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void MessageForPawnChange(object sender, string message)
        {
            MessagePawnChange(sender, "Please enter a new Figure for change");
        }
        public void MessageForProgress(object sender, (string, string) e)
        {
            MessageProgress(this, ("", ""));
        }

        #region Knight Game
        public void CreateStartKnight(string coordinate)
        {
            _movesKnight.CreateStartKnight(coordinate);
        }
        public void CreateTargetKnight(string coordinate)
        {
            _movesKnight.CreateTargetKnight(coordinate);
        }
        public static int MinKnightCount()
        {
            return MovesKnight.MinKnightCount();
        }

        #endregion
    }
}
