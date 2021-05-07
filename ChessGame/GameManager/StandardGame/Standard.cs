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
        private BaseFigure chengedPawn;
        private BaseFigure CurentKing;
        private BaseFigure CheckedFigure;
        public event Message messageForMove;
        public event MessageForMate messageForPawnChange;
        public event MessageForMate messageCheck;
        public event Picture setPicture;
        public event Picture removePicture;
        private static CoordinatePoint coordinate;
        private static int pawnCount = 2;
        private bool isChecked = true;
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
            baseFigure = CheckedCurrentFigure(currentCoordinate);
            var available = (IAvailableMoves)baseFigure;
            if (IsCheckedThisKing(baseFigure, targetCoordinate))
            {
                if (isChecked)
                {
                    if (baseFigure is King king)
                        return KingFigureSet(king, targetCoordinate);
                    else if (available.AvailableMoves().Contains(targetCoordinate))
                    {
                        if (available is Pawn pawn)
                            PawnFigureSet(pawn, targetCoordinate);
                        else
                        {
                            baseFigure.SetFigurePosition(targetCoordinate);
                            IsCheckedKing(baseFigure);
                        }
                        return true;
                    }
                    return false;
                }
            }
            return false;
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
                }
            }
        }

        /// <summary>
        /// Get all figures names for reset bord
        /// </summary>
        /// <returns>Return all figures names</returns>
        public static List<string> GetNamesForReset()
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
        /// Check a current figure with a coordinate
        /// </summary>
        /// <param name="coordinatePoint">Current coordinate</param>
        /// <returns>Return a current figure</returns>
        private static BaseFigure CheckedCurrentFigure(CoordinatePoint coordinatePoint)
        {
            foreach (var item in models)
            {
                if (item.Coordinate == coordinatePoint)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// Check pawn have changed a new figure, if no changed pawn set a target coordinate
        /// </summary>
        /// <param name="pawn">Current pawn</param>
        /// <param name="targetCoordinate">Target coordinate</param>
        private void PawnFigureSet(Pawn pawn, CoordinatePoint targetCoordinate)
        {
            if (CheckPawnChange(targetCoordinate, pawn))
            {
                coordinate = targetCoordinate;
                pawn.SetFigurePosition(targetCoordinate);
                chengedPawn = pawn;
                messageForPawnChange(pawn.Color, "Please enter a new Figure for change");
                IsCheckedKing(baseFigure);
            }
            else
            {
                pawn.SetFigurePosition(targetCoordinate);
                IsCheckedKing(baseFigure);
            }
        }

        /// <summary>
        /// Create All figures
        /// </summary>
        /// <returns>Return all figures</returns>
        private static List<BaseFigure> GetAllFigures()
        {
            Queen queenWhite = new("Queen.White.1", FColor.White, models);
            models.Add(queenWhite);
            King kingWhite = new("King.White.1", FColor.White, models);
            models.Add(kingWhite);
            Knight knightWhite1 = new("Knight.White.1", FColor.White, models);
            models.Add(knightWhite1);
            Knight knightWhite2 = new("Knight.White.2", FColor.White, models);
            models.Add(knightWhite2);
            Bishop bishopWhite1 = new("Bishop.White.1", FColor.White, models);
            models.Add(bishopWhite1);
            Bishop bishopWhite2 = new("Bishop.White.2", FColor.White, models);
            models.Add(bishopWhite2);
            Rook rookWhite1 = new("Rook.White.1", FColor.White, models);
            models.Add(rookWhite1);
            Rook rookWhite2 = new("Rook.White.2", FColor.White, models);
            models.Add(rookWhite2);
            Pawn pawnWhite1 = new("Pawn.White.1", FColor.White, models);
            models.Add(pawnWhite1);
            Pawn pawnWhite2 = new("Pawn.White.2", FColor.White, models);
            models.Add(pawnWhite2);
            Pawn pawnWhite3 = new("Pawn.White.3", FColor.White, models);
            models.Add(pawnWhite3);
            Pawn pawnWhite4 = new("Pawn.White.4", FColor.White, models);
            models.Add(pawnWhite4);
            Pawn pawnWhite5 = new("Pawn.White.5", FColor.White, models);
            models.Add(pawnWhite5);
            Pawn pawnWhite6 = new("Pawn.White.6", FColor.White, models);
            models.Add(pawnWhite6);
            Pawn pawnWhite7 = new("Pawn.White.7", FColor.White, models);
            models.Add(pawnWhite7);
            Pawn pawnWhite8 = new("Pawn.White.8", FColor.White, models);
            models.Add(pawnWhite8);

            Queen queenBlack = new("Queen.Black.1", FColor.Black, models);
            models.Add(queenBlack);
            King kingBlack = new("King.Black.1", FColor.Black, models);
            models.Add(kingBlack);
            Knight knightBlack1 = new("Knight.Black.1", FColor.Black, models);
            models.Add(knightBlack1);
            Knight knightBlack2 = new("Knight.Black.2", FColor.Black, models);
            models.Add(knightBlack2);
            Bishop bishopBlack1 = new("Bishop.Black.1", FColor.Black, models);
            models.Add(bishopBlack1);
            Bishop bishopBlack2 = new("Bishop.Black.2", FColor.Black, models);
            models.Add(bishopBlack2);
            Rook rookBlack1 = new("Rook.Black.1", FColor.Black, models);
            models.Add(rookBlack1);
            Rook rookBlack2 = new("Rook.Black.2", FColor.Black, models);
            models.Add(rookBlack2);
            Pawn pawnBlack1 = new("Pawn.Black.1", FColor.Black, models);
            models.Add(pawnBlack1);
            Pawn pawnBlack2 = new("Pawn.Black.2", FColor.Black, models);
            models.Add(pawnBlack2);
            Pawn pawnBlack3 = new("Pawn.Black.3", FColor.Black, models);
            models.Add(pawnBlack3);
            Pawn pawnBlack4 = new("Pawn.Black.4", FColor.Black, models);
            models.Add(pawnBlack4);
            Pawn pawnBlack5 = new("Pawn.Black.5", FColor.Black, models);
            models.Add(pawnBlack5);
            Pawn pawnBlack6 = new("Pawn.Black.6", FColor.Black, models);
            models.Add(pawnBlack6);
            Pawn pawnBlack7 = new("Pawn.Black.7", FColor.Black, models);
            models.Add(pawnBlack7);
            Pawn pawnBlack8 = new("Pawn.Black.8", FColor.Black, models);
            models.Add(pawnBlack8);

            return models;
        }

        #region King perfomance

        /// <summary>
        /// Check current king coordinate for move and set
        /// </summary>
        /// <param name="king">Current king</param>
        /// <param name="targetCoordinate">Target coordinate for current king</param>
        /// <returns>Return true if king is moved</returns>
        private bool KingFigureSet(King king, CoordinatePoint targetCoordinate)
        {
            if (GetCurrentKingMoves(king).Contains(targetCoordinate))
            {
                baseFigure.SetFigurePosition(targetCoordinate);
                IsCheckedKing(baseFigure);
                return true;
            }
            else if (CheckCastling(king, out CoordinatePoint coordinatePoint))
            {
                SetRookCastling(coordinatePoint);
                baseFigure.SetFigurePosition(targetCoordinate);
                IsCheckedKing(baseFigure);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check the available moves for current king 
        /// </summary>
        /// <returns>Return the list</returns>
        private static List<CoordinatePoint> GetCurrentKingMoves(King king)
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
        /// Checked Current king is check or no
        /// </summary>
        /// <param name="baseFigure">Current figure with moved</param>
        /// <returns>Return true if king is under check</returns>
        private void IsCheckedKing(BaseFigure baseFigure)
        {
            CurentKing = (BaseFigure)models.Where(c => c.Color != baseFigure.Color && c is King).Single();
            if (DangerPosition(CurentKing).Contains(CurentKing.Coordinate))
            {
                CheckedFigure = baseFigure;
                messageCheck(this, "Check");
                isChecked = false;
            }
            else
            {
                isChecked = true;
                messageCheck(this, " ");
            }

        }

        /// <summary>
        /// Check the danger position for current king
        /// </summary>
        /// <param name="model">King instance withe or Black</param>
        /// <returns>Return danger position List for current king </returns>
        private static List<CoordinatePoint> DangerPosition(BaseFigure model)
        {
            var modelNew = models.Where(c => c.Color != model.Color);
            var result = new List<CoordinatePoint>();
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                var array = temp.AvailableMoves();
                result.AddRange(array);
            }
            return result;
        }
        private bool IsCheckedThisKing(BaseFigure baseFigure, CoordinatePoint coordinatePoint)
        {
            var thisKing = (BaseFigure)models.Where(c => c.Color == baseFigure.Color && c is King).Single();
            var temp = baseFigure.Coordinate;
            baseFigure.Coordinate = coordinatePoint;
            if (!DangerPosition(thisKing).Contains(thisKing.Coordinate))
            {
                baseFigure.Coordinate = temp;
                isChecked = true;
                return true;
            }
            else if (coordinatePoint == CheckedFigure?.Coordinate)
            {
                baseFigure.Coordinate = temp;
                isChecked = true;
                return true;
            }
            else
            {
                baseFigure.Coordinate = temp;
                return false;
            }
        }

        #endregion

        #region Castling

        /// <summary>
        /// Checked Rook is valid position for castling
        /// </summary>
        /// <param name="king">Current king</param>
        /// <param name="coordinatePoint"></param>
        /// <returns>Return true if rook position is valid</returns>
        private bool CheckRook(King king, out CoordinatePoint coordinatePoint)
        {
            var modelNew = models.Where(f => f.Color == king.Color && f is Rook && !f.isMoved).ToList();
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
            }
            else
            {
                var coordinatePoint1 = new CoordinatePoint(king.Coordinate.X - 1, king.Coordinate.Y);
                var coordinatePoint2 = new CoordinatePoint(king.Coordinate.X - 2, king.Coordinate.Y);
                var coordinatePoint3 = new CoordinatePoint(king.Coordinate.X - 3, king.Coordinate.Y);
                foreach (var item in models)
                {
                    if (item.Coordinate != coordinatePoint1 & item.Coordinate != coordinatePoint2 
                                                            & item.Coordinate != coordinatePoint3)
                        count++;
                }
            }
            if (count == models.Count)
            {
                coordinatePoint = rook.Coordinate;
                return true;
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
            if (king.isMoved)
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

        /// <summary>
        /// Set the rook change coordinate
        /// </summary>
        /// <param name="coordinatePoint"></param>
        private void SetRookCastling(CoordinatePoint coordinatePoint)
        {
            foreach (var item in models.Where(c => c is Rook))
            {
                if (item.Coordinate == coordinatePoint)
                {
                    if (coordinatePoint.X == 7)
                        item.SetFigurePosition(new CoordinatePoint(5, coordinatePoint.Y));
                    else
                        item.SetFigurePosition(new CoordinatePoint(3, coordinatePoint.Y));
                }
            }
        }

        #endregion

        #region Check Pawn

        /// <summary>
        /// Check the pawn figure for change new figure
        /// </summary>
        /// <param name="coordinatePoint">Pawn coordinate</param>
        /// <param name="pawn">Current pawn</param>
        /// <returns></returns>
        private static bool CheckPawnChange(CoordinatePoint coordinatePoint, Pawn pawn)
        {
            if (pawn.Color == FColor.White)
            {
                if (coordinatePoint.Y == 0)
                    return true;
            }
            else
            {
                if (coordinatePoint.Y == 7)
                    return true;
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
            var baseFigure = GetFigure(currentFigur, currentColor);
            baseFigure.Coordinate = coordinate;
            models.Add(baseFigure);
            baseFigure.setPicture += SetFigurePicture;
            baseFigure.removePicture += RemoveFigurePicture;
            baseFigure.messageForMove += MessageMove;
            baseFigure.SetFigurePosition(coordinate);
            chengedPawn.RemoveFigurePosition();
            models.Remove(chengedPawn);
        }

        /// <summary>
        /// Create new Figure
        /// </summary>
        /// <param name="figure">Figure name</param>
        /// <param name="color">Figure color</param>
        /// <returns>Return new Figure</returns>
        private static BaseFigure GetFigure(string figure, string color)
        {
            var fColor = color == "White" ? FColor.White : FColor.Black;
            return figure switch
            {
                "Queen" => new Queen(figure + "." + color + '.' + pawnCount, fColor, models),
                "King" => new King(figure + "." + color + '.' + pawnCount, fColor, models),
                "Rook" => new Rook(figure + "." + color + "." + pawnCount, fColor, models),
                "Bishop" => new Bishop(figure + "." + color + "." + pawnCount, fColor, models),
                "Knight" => new Knight(figure + "." + color + "." + pawnCount, fColor, models),
                _ => null,
            };
        }

        #endregion
    }
}
