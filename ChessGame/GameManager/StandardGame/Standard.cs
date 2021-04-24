using Figure;
using System.Collections.Generic;
using System.Linq;

namespace GameManager
{
    public class Standard
    {
        #region Property and Feld
        private static List<BaseFigure> models = new();
        private BaseFigure baseFigure;
        public event Message messageForMove;
        public event Picture setPicture;
        public event Picture removePicture;
        #endregion

        /// <summary>
        /// Initialize a setPicture event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void SetFigurePicture(object sender, string coordinate)
        {
            setPicture(this, coordinate);
        }

        /// <summary>
        /// Initialize a removePicture event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void RemoveFigurePicture(object sender, string coordinate)
        {
            removePicture(this, coordinate);
        }

        /// <summary>
        /// Initialize a messageForMove event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void MessageMove(object sender, (string, string) coordinate)
        {
            messageForMove(this, coordinate);
        }

        /// <summary>
        /// Added coordinate and set all figures
        /// </summary>
        public void SetAllFigures()
        {
            foreach (var item in GetAllFigures())
            {
                item.setPicture += SetFigurePicture;
                item.removePicture += RemoveFigurePicture;
                item.messageForMove += MessageMove;
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

        /// <summary>
        /// Get all figures names for reset bord
        /// </summary>
        /// <returns>Return all figures names</returns>
        public List<string> GetNamesForReset()
        {
            var positions = new List<string>();
            if (models.Count != 0)
            {
                foreach (var item in models)
                {
                    positions.Add(item.Name);
                }
                models.Clear();
            }
            else
            {
                positions.Add("0");
            }
            return positions;
        }

        /// <summary>
        /// Check if coordinate is valid for figure, and set this figure
        /// </summary>
        /// <param name="current">Current coordinate with string format</param>
        /// <param name="target">Target coordinate with string format</param>
        /// <returns>Return true if target coordinate is valid for figure</returns>
        public bool IsVAlidCoordinate(string current, string target)
        {
            string[] strCurrent = current.Split('.');
            var currentCoordinate = new CoordinatePoint(int.Parse(strCurrent[0]), int.Parse(strCurrent[1]));
            string[] strTarget = target.Split('.');
            var targetCoordinate = new CoordinatePoint(int.Parse(strTarget[0]), int.Parse(strTarget[1]));
            foreach (var item in models)
            {
                if (item.Coordinate == currentCoordinate)
                {
                    baseFigure = item;
                    break;
                }
            }
            IAvailableMoves available = (IAvailableMoves)baseFigure;
            if (baseFigure is King king)
            {
                if (GetCurrentKingMoves(king).Contains(targetCoordinate))
                {
                    baseFigure.SetFigurePosition(targetCoordinate);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (available.AvailableMoves().Contains(targetCoordinate))
            {
                baseFigure.SetFigurePosition(targetCoordinate);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create All figures
        /// </summary>
        /// <returns>Return all figures</returns>
        private List<BaseFigure> GetAllFigures()
        {
            Queen queenWhite = new("Queen.White.1", "White", models);
            models.Add(queenWhite);
            King kingWhite = new("King.White.1", "White", models);
            models.Add(kingWhite);
            Knight knightWhite1 = new("Knight.White.1", "White", models);
            models.Add(knightWhite1);
            Knight knightWhite2 = new("Knight.White.2", "White", models);
            models.Add(knightWhite2);
            Bishop bishopWhite1 = new("Bishop.White.1", "White", models);
            models.Add(bishopWhite1);
            Bishop bishopWhite2 = new("Bishop.White.2", "White", models);
            models.Add(bishopWhite2);
            Rook rookWhite1 = new("Rook.White.1", "White", models);
            models.Add(rookWhite1);
            Rook rookWhite2 = new("Rook.White.2", "White", models);
            models.Add(rookWhite2);
            Pawn pawnWhite1 = new("Pawn.White.1", "White", models);
            models.Add(pawnWhite1);
            Pawn pawnWhite2 = new("Pawn.White.2", "White", models);
            models.Add(pawnWhite2);
            Pawn pawnWhite3 = new("Pawn.White.3", "White", models);
            models.Add(pawnWhite3);
            Pawn pawnWhite4 = new("Pawn.White.4", "White", models);
            models.Add(pawnWhite4);
            Pawn pawnWhite5 = new("Pawn.White.5", "White", models);
            models.Add(pawnWhite5);
            Pawn pawnWhite6 = new("Pawn.White.6", "White", models);
            models.Add(pawnWhite6);
            Pawn pawnWhite7 = new("Pawn.White.7", "White", models);
            models.Add(pawnWhite7);
            Pawn pawnWhite8 = new("Pawn.White.8", "White", models);
            models.Add(pawnWhite8);

            Queen queenBlack = new("Queen.Black.1", "Black", models);
            models.Add(queenBlack);
            King kingBlack = new("King.Black.1", "Black", models);
            models.Add(kingBlack);
            Knight knightBlack1 = new("Knight.Black.1", "Black", models);
            models.Add(knightBlack1);
            Knight knightBlack2 = new("Knight.Black.2", "Black", models);
            models.Add(knightBlack2);
            Bishop bishopBlack1 = new("Bishop.Black.1", "Black", models);
            models.Add(bishopBlack1);
            Bishop bishopBlack2 = new("Bishop.Black.2", "Black", models);
            models.Add(bishopBlack2);
            Rook rookBlack1 = new("Rook.Black.1", "Black", models);
            models.Add(rookBlack1);
            Rook rookBlack2 = new("Rook.Black.2", "Black", models);
            models.Add(rookBlack2);
            Pawn pawnBlack1 = new("Pawn.Black.1", "Black", models);
            models.Add(pawnBlack1);
            Pawn pawnBlack2 = new("Pawn.Black.2", "Black", models);
            models.Add(pawnBlack2);
            Pawn pawnBlack3 = new("Pawn.Black.3", "Black", models);
            models.Add(pawnBlack3);
            Pawn pawnBlack4 = new("Pawn.Black.4", "Black", models);
            models.Add(pawnBlack4);
            Pawn pawnBlack5 = new("Pawn.Black.5", "Black", models);
            models.Add(pawnBlack5);
            Pawn pawnBlack6 = new("Pawn.Black.6", "Black", models);
            models.Add(pawnBlack6);
            Pawn pawnBlack7 = new("Pawn.Black.7", "Black", models);
            models.Add(pawnBlack7);
            Pawn pawnBlack8 = new("Pawn.Black.8", "Black", models);
            models.Add(pawnBlack8);

            return models;
        }

        /// <summary>
        /// Check the available moves for current king 
        /// </summary>
        /// <returns>Return the list</returns>
        private List<CoordinatePoint> GetCurrentKingMoves(King king)
        {
            IRandomMove currentKing = (IRandomMove)king;
            var result = new List<CoordinatePoint>();
            foreach (var item in currentKing.AvailableMoves())
            {
                if (!DangerPosition(king).Contains(item))
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
        private List<CoordinatePoint> DangerPosition(BaseFigure model)
        {
            var modelNew = models.Where(c => c.Color != model.Color);
            var result = new List<CoordinatePoint>();
            foreach (var item in modelNew)
            {
                var temp = (IDangerMoves)item;
                var array = temp.DangerMoves();
                result.AddRange(array);
            }
            return result;
        }
    }
}
