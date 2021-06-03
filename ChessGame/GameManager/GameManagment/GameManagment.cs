using System.Collections.Generic;

namespace GameManager
{
    public class GameManagment
    {
        #region Property and Feld
        public static int CurrentGameStatus { get => _currentGameStatus; set => _currentGameStatus = value; }
        private static int _currentGameStatus;
        public string currentFigureColor;
        private static KingGame _kingGame;
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

        #endregion

        public GameManagment(string currentFigureColor, int currentGameStatus)
        {
            this.currentFigureColor = currentFigureColor;
            CurrentGameStatus = currentGameStatus;
        }

        #region Initialize
        public void InitializeGameManagmentComponent()
        {
            InitializeKingGame();
            InitializeKnightGame();
            InitializeStandard();
        }
        private void InitializeStandard()
        {
            _standard = new Standard();
            _standard.SetPicture += SetPicture;
            _standard.RemovePicture += RemovePicture;
            _standard.DeletePicture += DeletePicture;
            _standard.MessageForMove += MessageForMove;
            _standard.MessageCheck += MessageCheck;
            _standard.MessagePawnChange += MessagePawnChange;
        }
        private void InitializeKingGame()
        {
            _kingGame = new KingGame(currentFigureColor);
            _kingGame.SetPicture += SetPicture;
            _kingGame.RemovePicture += RemovePicture;
            _kingGame.MessageForMove += MessageForMove;
            _kingGame.MessageProgress += MessageForProgress;
            _kingGame.MateMessage += MateMessage;
        }
        private void InitializeKnightGame()
        {
            _movesKnight = new MovesKnight();
            _movesKnight.SetPicture += SetPicture;
            _movesKnight.MessageForMoveKnight += delegate { };
        }

        #endregion

        #region Methods

        /// <summary>
        /// decides which logic will run after getting the info from UI
        /// </summary>
        /// <param name="tupl">choosen cell's coordintes </param>
        /// <returns>true if standard game, and false otherway</returns>
        public bool Managment((string, string) tupl)
        {
            switch (CurrentGameStatus)
            {
                case 1:
                    if (_kingGame.IsVAlidCoordinate(tupl.Item1, tupl.Item2))
                        _kingGame.Logic();
                    return false;
                case 3:
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

        /// <summary>
        /// getting the available moves for current game's type
        /// </summary>
        /// <param name="coordinate">currenet coordinate</param>
        /// <returns>available moves</returns>
        public static List<string> GetAvalibleMoves(string coordinate)
        {
            return CurrentGameStatus switch
            {
                1 => KingGame.GetAvalibleMoves(coordinate),
                3 => Standard.GetAvalibleMoves(coordinate),
                _ => null,
            };
        }

        /// <summary>
        /// gives the info about all figures for deleting
        /// </summary>
        /// <param name="status">current game status</param>
        /// <returns>info about figures</returns>
        public static List<string> GetAllFiguresForReset(int status)
        {
            return status switch
            {
                1 => KingGame.GetNamesForReset(),
                2 => MovesKnight.GetNamesForReset(),
                3 => Standard.GetNamesForReset(),
                _ => null,
            };
        }

        #endregion

        #region Standard Game
        public void SetChangeFigureForPawn(string inputInfo) => _standard.SetChangeFigureForPawn(inputInfo);
        public void SetAllFigures() => _standard.SetAllFigures();
        public static string GetAllFiguresForSave() => Standard.GetDetailForSave();
        public void SetConditionFigures(string json) => _standard.SetConditionFigures(json);

        #endregion

        #region King Game
        public static void IsValidForPleacement(string inputInfo) => _kingGame.IsValidForPleacement(inputInfo);
        public void MessageForProgress(object sender, (string, string) e) => MessageProgress(this, ("", ""));

        #endregion

        #region Knight Game

        /// <summary>
        /// creates the knight in the starting point
        /// </summary>
        /// <param name="coordinate">starting coordinate</param>
        public void CreateStartKnight(string coordinate)
        {
            _movesKnight.CreateStartKnight(coordinate);
        }

        /// <summary>
        /// creates the aimed knght 
        /// </summary>
        /// <param name="coordinate">target coordnate</param>
        public void CreateTargetKnight(string coordinate)
        {
            _movesKnight.CreateTargetKnight(coordinate);
        }

        /// <summary>
        /// counts the coordinate form starting position to targetting position 
        /// </summary>
        /// <returns>returns the count</returns>
        public static int MinKnightCount()
        {
            return MovesKnight.MinKnightCount();
        }

        #endregion
    }
}
