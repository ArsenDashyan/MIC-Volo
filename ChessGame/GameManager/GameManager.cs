using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    public class GameManagment
    {
        #region Property and Feld
        public static int CurrentGameStatus { get => currentGameStatus; set => currentGameStatus = value; }
        private static int currentGameStatus;
        private Manager manager;
        private MovesKnight movesKnight;
        private Standard standard;
        public event MessageForMate MateMessage;
        public event Message MessageForMove;
        public event Message MessageProgress;
        public event Picture SetPicture;
        public event Picture RemovePicture;
        public event MessageForMate MessagePawnChange;
        public event MessageForMate MessageCheck;
        public event Picture DeletePicture;
        public string currentFigureColor;
        private readonly bool colorPower = false;
        public List<string> models = new();

        #endregion
        public GameManagment(string currentFigureColor, int currentGameStatus)
        {
            this.currentFigureColor = currentFigureColor;
            CurrentGameStatus = currentGameStatus;

        }
        private void InitializeStandard()
        {
            standard = new Standard();
            standard.SetPicture += SetFigurePicture;
            standard.RemovePicture += RemoveFigurePicture;
            standard.DeletePicture += SetDeleteFigurePicture;
            standard.MessageForMove += MessageMoveForStandardGame;
            standard.MessageCheck += MessageCheckStandard;
            standard.MessagePawnChange += MessageForPawnChange;
        }
        public void IsValidForPleacement(string inputInfo)
        {
            InitializeKingGame();
            manager.IsValidForPleacement(inputInfo);
        }
        private void InitializeKingGame()
        {
            manager = new Manager(currentFigureColor);
            manager.SetPicture += SetFigurePicture;
            manager.RemovePicture += RemoveFigurePicture;
            manager.MessageForMove += MessageMoveForKingGame;
            manager.MessageProgress += MessageForProgress;
        }
        private void InitializeKnightGame()
        {
            movesKnight = new MovesKnight();
            movesKnight.SetPicture += SetFigurePicture;
            movesKnight.MessageForMoveKnight += delegate { };
        }
        public void Managment((string, string) tupl)
        {
            switch (CurrentGameStatus)
            {
                case 1:
                    InitializeKingGame();
                    if (manager.IsVAlidCoordinate(tupl.Item1, tupl.Item2))
                    {
                        manager.MateMessage += MessageMate;
                        manager.MessageProgress += MessageForProgress;
                        manager.Logic();
                    }
                    break;
                case 3:
                    InitializeStandard();
                    standard.RemovePicture += RemoveFigurePicture;
                    standard.DeletePicture += SetDeleteFigurePicture;
                    standard.IsVAlidCoordinate(tupl.Item1, tupl.Item2);
                    break;
            }
        }
        public void SetAllFigures()
        {
            InitializeStandard();
            standard.SetAllFigures();
        }
        public static List<string> GetAvalibleMoves(string coordinate)
        {
            return CurrentGameStatus switch
            {
                1 => Manager.GetAvalibleMoves(coordinate),
                3 => Standard.GetAvalibleMoves(coordinate),
                _ => null,
            };
        }
        public void SetChangeFigureForPawn(string inputInfo)
        {
            standard.SetChangeFigureForPawn(inputInfo);
        }
        public List<string> GetAllFiguresForReset()
        {
            switch (CurrentGameStatus)
            {
                case 1:
                    models = manager.GetNamesForReset();
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
    }
}
