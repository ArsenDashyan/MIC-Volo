using System.Collections.Generic;

namespace GameManager
{
    public class GameManagment
    {
        #region Property and Feld
        public static int CurrentGameStatus { get => _currentGameStatus; set => _currentGameStatus = value; }
        private static int _currentGameStatus;
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
        public string currentFigureColor;

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
        public static void IsValidForPleacement(string inputInfo) => _kingGame.IsValidForPleacement(inputInfo);
        public void SetChangeFigureForPawn(string inputInfo) => _standard.SetChangeFigureForPawn(inputInfo);
        public void SetAllFigures() => _standard.SetAllFigures();
        public void MessageForProgress(object sender, (string, string) e) => MessageProgress(this, ("", ""));
        public static List<string> GetAvalibleMoves(string coordinate)
        {
            return CurrentGameStatus switch
            {
                1 => KingGame.GetAvalibleMoves(coordinate),
                3 => Standard.GetAvalibleMoves(coordinate),
                _ => null,
            };
        }
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
        public static string GetAllFiguresForSave() => Standard.GetNamesForSave();

        public void SetConditionFigures(string json) => _standard.SetConditionFigures(json);
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
