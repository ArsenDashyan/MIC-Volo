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
        public event MessageForMate messageForPawnChange;
        public event Picture setPicture;
        public event Picture removePicture;
        private static CoordinatePoint coordinate;
        private int whiteKingMovesCount = 0;
        private int blackKingMovesCount = 0;
        private static int pawnCount = 2;
        #endregion

        #region Event Methods

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

        #endregion

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
                    if (king.Color == "White")
                    {
                        whiteKingMovesCount++;
                    }
                    else
                    {
                        blackKingMovesCount++;
                    }
                    baseFigure.SetFigurePosition(targetCoordinate);
                    return true;
                }
                else if (CheckCastling(king, out CoordinatePoint coordinatePoint))
                {
                    if (king.Color == "White")
                    {
                        whiteKingMovesCount++;
                    }
                    else
                    {
                        blackKingMovesCount++;
                    }
                    SetRookCastling(coordinatePoint);
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
                if (available is Pawn pawn)
                {
                    if (CheckPawnChange(targetCoordinate, pawn))
                    {
                        coordinate = targetCoordinate;
                        messageForPawnChange(pawn.Color, "Please enter a new Figure for change");
                        baseFigure.RemoveFigurePosition();
                    }
                    else
                    {
                        baseFigure.SetFigurePosition(targetCoordinate);
                    }
                }
                else
                {
                    baseFigure.SetFigurePosition(targetCoordinate);
                }
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
            var currentKing = (IAvailableMoves)king;
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

        /// <summary>
        /// Checked Rook is valid position for castling
        /// </summary>
        /// <param name="king">Current king</param>
        /// <param name="coordinatePoint"></param>
        /// <returns>Return true if rook position is valid</returns>
        private bool CheckRook(King king, out CoordinatePoint coordinatePoint)
        {
            var modelNew = models.Where(f => f.Color == king.Color && f is Rook).ToList();
            foreach (var item in modelNew)
            {
                if (CheckEpmtyCells(king, (Rook)item, out CoordinatePoint coordinte))
                {
                    coordinatePoint = coordinte;
                        return true;
                }
            }
            coordinatePoint = null;
            return false;
        }

        /// <summary>
        /// Check the empty cells for chande king and rook
        /// </summary>
        /// <param name="king">Current King</param>
        /// <param name="rook">Current Rook</param>
        /// <param name="coordinatePoint">Out parametr, rook coordinate</param>
        /// <returns></returns>
        private bool CheckEpmtyCells(King king, Rook rook, out CoordinatePoint coordinatePoint)
        {
            int count = 0;
            if (king.Coordinate.X < rook.Coordinate.X)
            {
                var coordinatePoint1 = new CoordinatePoint(king.Coordinate.X + 1, king.Coordinate.Y);
                var coordinatePoint2 = new CoordinatePoint(king.Coordinate.X + 2, king.Coordinate.Y);
                foreach (var item in models)
                {
                    if (item.Coordinate != coordinatePoint1 & item.Coordinate != coordinatePoint2)
                    {
                        count++;
                    }
                }
                if (count == models.Count)
                {
                    coordinatePoint = rook.Coordinate;
                    return true;
                }
            }
            else
            {
                var coordinatePoint1 = new CoordinatePoint(king.Coordinate.X - 1, king.Coordinate.Y);
                var coordinatePoint2 = new CoordinatePoint(king.Coordinate.X - 2, king.Coordinate.Y);
                var coordinatePoint3 = new CoordinatePoint(king.Coordinate.X - 3, king.Coordinate.Y);
                foreach (var item in models)
                {
                    if (item.Coordinate != coordinatePoint1 & item.Coordinate != coordinatePoint2 & item.Coordinate != coordinatePoint3)
                    {
                        count++;
                    }
                }
                if (count == models.Count)
                {
                    coordinatePoint = rook.Coordinate;
                    return true;
                }
            }
            coordinatePoint = null;
            return false;
        }

        /// <summary>
        /// Checked if have a castling king and rook
        /// </summary>
        /// <param name="king">Current king</param>
        /// <param name="coordinatePoint"></param>
        /// <returns>Return true, if castling is valid</returns>
        private bool CheckCastling(King king, out CoordinatePoint coordinatePoint)
        {
            if (king.Color == "White")
            {
                if (whiteKingMovesCount > 1)
                {
                    coordinatePoint = null;
                    return false;
                }
                else
                {
                    if (CheckRook(king, out CoordinatePoint coordinate))
                    {
                        coordinatePoint = coordinate;
                        return true;
                    }
                    coordinatePoint = null;
                    return false;
                }
            }
            else
            {
                if (blackKingMovesCount > 1)
                {
                    coordinatePoint = null;
                    return false;
                }
                else
                {
                    if (CheckRook(king, out CoordinatePoint coordinate))
                    {
                        coordinatePoint = coordinate;
                        return true;
                    }
                    coordinatePoint = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Set the rook change coordinate
        /// </summary>
        /// <param name="coordinatePoint"></param>
        private void SetRookCastling(CoordinatePoint coordinatePoint)
        {
            foreach (var item in models)
            {
                if (item.Coordinate == coordinatePoint)
                {
                    if (coordinatePoint.X == 7)
                    {
                        item.SetFigurePosition(new CoordinatePoint(5,coordinatePoint.Y));
                    }
                    else
                    {
                        item.SetFigurePosition(new CoordinatePoint(3, coordinatePoint.Y));
                    }
                }
            }
        }

        /// <summary>
        /// Check the pawn figure for change new figure
        /// </summary>
        /// <param name="coordinatePoint">Pawn coordinate</param>
        /// <param name="pawn">Current pawn</param>
        /// <returns></returns>
        public bool CheckPawnChange(CoordinatePoint coordinatePoint, Pawn pawn)
        {
            if (pawn.Color == "White")
            {
                if (coordinatePoint.Y == 0)
                {
                    return true;
                }
            }
            else
            {
                if (coordinatePoint.Y == 7)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Set new Figure With pawn
        /// </summary>
        /// <param name="info">New Figure information</param>
        public void SetChangeFigureForPawn(string info)
        {
            string[] strCurrent = info.Split('.');
            var currentFigur = strCurrent[0];
            string currentColor = strCurrent[1];
            pawnCount++;
            BaseFigure baseFigure = GetFigure(currentFigur, currentColor);
            models.Add(baseFigure);
            baseFigure.setPicture += SetFigurePicture;
            baseFigure.removePicture += RemoveFigurePicture;
            baseFigure.messageForMove += MessageMove;
            baseFigure.SetFigurePosition(coordinate);
        }

        /// <summary>
        /// Create new Figure
        /// </summary>
        /// <param name="figure">Figure name</param>
        /// <param name="color">Figure color</param>
        /// <returns>Return new Figure</returns>
        public BaseFigure GetFigure(string figure, string color)
        {
            if (color == "White")
            {
                switch (figure)
                {
                    case "Queen":
                        return  new Queen(figure + "." + color + '.' + pawnCount, color, models);
                    case "King":
                        return  new King(figure + "." + color + '.' + pawnCount, color, models);
                    case "Rook":
                        return new Rook(figure + "." + color + "." + pawnCount, color, models);
                    case "Bishop":
                        return new Bishop(figure + "." + color + "." + pawnCount, color, models);
                    case "Knight":
                        return new Knight(figure + "." + color + "." + pawnCount, color, models);
                    default:
                        return null;
                }
            }
            else
            {
                switch (figure)
                {
                    case "Queen":
                        return new Queen(figure + "." + color + '.' + pawnCount, color, models);
                    case "King":
                        return new King(figure + "." + color + '.' + pawnCount, color, models);
                    case "Rook":
                        return new Rook(figure + "." + color + "." + pawnCount, color, models);
                    case "Bishop":
                        return new Bishop(figure + "." + color + "." + pawnCount, color, models);
                    case "Knight":
                        return new Knight(figure + "." + color + "." + pawnCount, color, models);
                    default:
                        return null;
                }
            }
        }
    }
}
