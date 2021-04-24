using System;
using System.Collections.Generic;
using System.Linq;
using Figure;

namespace GameManager
{
    public delegate void MessageForMate(object sender, string str);
    public delegate void Picture(object sender, string e);
    public delegate void Message(object sender, (string, string) e);
    public class Manager
    {
        #region Property and Feld
        public static List<BaseFigure> models = new List<BaseFigure>();
        private List<CoordinatePoint> currentListForBabyGame = new();
        private BaseFigure CurentKing => 
               (BaseFigure)models.Where(c => c.Color == currentFigureColor && c is King).Single();
        private readonly string currentFigureColor;
        private BaseFigure baseFigure;
        public event MessageForMate MateMessage;
        public event Message messageForMove;
        public event Picture setPicture;
        public event Picture removePicture;
        private int whiteKingCount = 0;
        private int whiteQueenCount = 0;
        private int whiteBishopCount = 0;
        private int whiteRookCount = 0;
        private int whitePawnCount = 0;
        private int whiteKnightCount = 0;
        private int blackKingCount = 0;
        private int blackQueenCount = 0;
        private int blackBishopCount = 0;
        private int blackRookCount = 0;
        private int blackPawnCount = 0;
        private int blackKnightCount = 0;
        #endregion

        public Manager(string currentFigureColor)
        {
            this.currentFigureColor = currentFigureColor;
        }

        public void SetFigurePicture(object sender, string coordinate)
        {
            setPicture(this, coordinate);
        }
        public void RemoveFigurePicture(object sender, string coordinate)
        {
            removePicture(this, coordinate);
        }
        public void MessageMove(object sender, (string, string) coordinate)
        {
            messageForMove(this, coordinate);
        }

        /// <summary>
        /// Base logic for game
        /// </summary>
        public void Logic()
        {
            if (!UnderAttack())
            {
                if (!currentListForBabyGame.BabyGame())
                {
                    if (!IsMate())
                        MoveWithBlock();
                    else
                        MoveWithShax();
                }
                else
                    AntiBabyGame();
            }
        }

        #region Block Methods

        /// <summary>
        /// Change the positon with block when king is min available move and king inside in one half
        /// </summary>
        /// <param name="figure">Figure instance</param>
        /// <param name="keyValuePair">The out parametr</param>
        /// <returns>Reteurn the coordinate and king available move count ande figure</returns>
        private bool GetMinMovesWithBlockInOneHaalf(BaseFigure figure, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair)
        {
            var countList = new Dictionary<CoordinatePoint, (int, BaseFigure)>();
            IRandomMove tempFigur = (IRandomMove)figure;
            CoordinatePoint temp = figure.Coordinate;
            int targetX = CurentKing.Coordinate.X + 1 == 8 ? CurentKing.Coordinate.X : CurentKing.Coordinate.X + 1;
            int targetY = CurentKing.Coordinate.Y - 1 == -1 ? CurentKing.Coordinate.Y : CurentKing.Coordinate.Y - 1;
            foreach (var item in tempFigur.AvailableMoves().FiltrFor(c => c.X <= CurentKing.Coordinate.X && c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!tempFigur.IsUnderAttack(figure.Coordinate))
                {
                    if (GetCurrentKingMoves().Filtr(c => c.X >= targetX | c.Y <= targetY) && !tempFigur.AvailableMoves().Contains(CurentKing.Coordinate))
                        countList.Add(item, (GetCurrentKingMoves().Count, figure));
                }
            }
            figure.Coordinate = temp;
            if (countList.Count != 0)
            {
                keyValuePair = countList.Where(k => k.Value.Item1 != 0).OrderBy(k => k.Value.Item1).FirstOrDefault();
                return true;
            }
            else
            {
                keyValuePair = new KeyValuePair<CoordinatePoint, (int, BaseFigure)>();
                return false;
            }
        }

        /// <summary>
        /// Change the positon with block when king is min available move and king inside in two half
        /// </summary>
        /// <param name="figure">Figure instance</param>
        /// <param name="keyValuePair">The out parametr</param>
        /// <returns>Reteurn the coordinate and king available move count ande figure</returns>
        private bool GetMinMovesWithBlockInTwoHaalf(BaseFigure figure, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair)
        {
            var countList = new Dictionary<CoordinatePoint, (int, BaseFigure)>();
            IRandomMove tempFigur = (IRandomMove)figure;
            CoordinatePoint temp = figure.Coordinate;
            int targetX = CurentKing.Coordinate.X - 1 == -1 ? CurentKing.Coordinate.X : CurentKing.Coordinate.X - 1;
            int targetY = CurentKing.Coordinate.Y - 1 == -1 ? CurentKing.Coordinate.Y : CurentKing.Coordinate.Y - 1;
            foreach (var item in tempFigur.AvailableMoves().FiltrFor(c => c.X >= CurentKing.Coordinate.X && c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!tempFigur.IsUnderAttack(figure.Coordinate))
                {
                    if (GetCurrentKingMoves().Filtr(c => c.X <= targetX | c.Y <= targetY) && !tempFigur.AvailableMoves().Contains(CurentKing.Coordinate))
                        countList.Add(item, (GetCurrentKingMoves().Count, figure));
                }
            }
            figure.Coordinate = temp;
            if (countList.Count != 0)
            {
                keyValuePair = countList.Where(k => k.Value.Item1 != 0).OrderBy(k => k.Value.Item1).FirstOrDefault();
                return true;
            }
            else
            {
                keyValuePair = new KeyValuePair<CoordinatePoint, (int, BaseFigure)>();
                return false;
            }
        }

        /// <summary>
        /// Change the positon with block when king is min available move and king inside in three half
        /// </summary>
        /// <param name="figure">Figure instance</param>
        /// <param name="keyValuePair">The out parametr</param>
        /// <returns>Reteurn the coordinate and king available move count ande figure</returns>
        private bool GetMinMovesWithBlockInThreeHaalf(BaseFigure figure, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair)
        {
            var countList = new Dictionary<CoordinatePoint, (int, BaseFigure)>();
            IRandomMove tempFigur = (IRandomMove)figure;
            CoordinatePoint temp = figure.Coordinate;
            int targetX = CurentKing.Coordinate.X - 1 == -1 ? CurentKing.Coordinate.X : CurentKing.Coordinate.X - 1;
            int targetY = CurentKing.Coordinate.Y + 1 == 8 ? CurentKing.Coordinate.Y : CurentKing.Coordinate.Y + 1;
            foreach (var item in tempFigur.AvailableMoves().FiltrFor(c => c.X >= CurentKing.Coordinate.X && c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!tempFigur.IsUnderAttack(figure.Coordinate))
                {
                    if (GetCurrentKingMoves().Filtr(c => c.X <= targetX | c.Y >= targetY) && !tempFigur.AvailableMoves().Contains(CurentKing.Coordinate))
                        countList.Add(item, (GetCurrentKingMoves().Count, figure));
                }
            }
            figure.Coordinate = temp;
            if (countList.Count != 0)
            {
                keyValuePair = countList.Where(k => k.Value.Item1 != 0).OrderBy(k => k.Value.Item1).FirstOrDefault();
                return true;
            }
            else
            {
                keyValuePair = new KeyValuePair<CoordinatePoint, (int, BaseFigure)>();
                return false;
            }
        }

        /// <summary>
        /// Change the positon with block when king is min available move and king inside in four half
        /// </summary>
        /// <param name="figure">Figure instance</param>
        /// <param name="keyValuePair">The out parametr</param>
        /// <returns>Reteurn the coordinate and king available move count ande figure</returns>
        private bool GetMinMovesWithBlockInFourHaalf(BaseFigure figure, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair)
        {
            var countList = new Dictionary<CoordinatePoint, (int, BaseFigure)>();
            IRandomMove tempFigur = (IRandomMove)figure;
            CoordinatePoint temp = figure.Coordinate;
            int targetX = CurentKing.Coordinate.X + 1 == 8 ? CurentKing.Coordinate.X : CurentKing.Coordinate.X + 1;
            int targetY = CurentKing.Coordinate.Y + 1 == 8 ? CurentKing.Coordinate.Y : CurentKing.Coordinate.Y + 1;
            foreach (var item in tempFigur.AvailableMoves().FiltrFor(c => c.X <= CurentKing.Coordinate.X && c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!tempFigur.IsUnderAttack(figure.Coordinate))
                {
                    if (GetCurrentKingMoves().Filtr(c => c.X >= targetX | c.Y >= targetY) && !tempFigur.AvailableMoves().Contains(CurentKing.Coordinate))
                        countList.Add(item, (GetCurrentKingMoves().Count, figure));
                }
            }
            figure.Coordinate = temp;
            if (countList.Count != 0)
            {
                keyValuePair = countList.Where(k => k.Value.Item1 != 0).OrderBy(k => k.Value.Item1).FirstOrDefault();
                return true;
            }
            else
            {
                keyValuePair = new KeyValuePair<CoordinatePoint, (int, BaseFigure)>();
                return false;
            }
        }

        /// <summary>
        /// Checks which position is best for blocking depending on the king position
        /// </summary>
        /// <returns>Return blocking position list</returns>
        private List<KeyValuePair<CoordinatePoint, (int, BaseFigure)>> HalfBlock()
        {
            var modelNew = models.Where(c => c.Color != currentFigureColor).Reverse().ToList();
            var list = new List<KeyValuePair<CoordinatePoint, (int, BaseFigure)>>();
            int currentKingHalf = CurentKing.Coordinate.GetCurrentKingHalf();
            foreach (var figur in modelNew)
            {
                switch (currentKingHalf)
                {
                    case 1:
                        if (GetMinMovesWithBlockInOneHaalf(figur, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair1))
                            list.Add(keyValuePair1);
                        break;
                    case 2:
                        if (GetMinMovesWithBlockInTwoHaalf(figur, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair2))
                            list.Add(keyValuePair2);
                        break;
                    case 3:
                        if (GetMinMovesWithBlockInThreeHaalf(figur, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair3))
                            list.Add(keyValuePair3);
                        break;
                    case 4:
                        if (GetMinMovesWithBlockInFourHaalf(figur, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair4))
                            list.Add(keyValuePair4);
                        break;
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// Change the positon with shax when king is min available move
        /// </summary>
        /// <param name="figure">Figure instance</param>
        /// <param name="keyValuePair">The out parametr</param>
        /// <returns>Rteurn the coordinate and king available move count ande figure </returns>
        private bool GetMinMovesWithShax(BaseFigure figure, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair)
        {
            var countList = new Dictionary<CoordinatePoint, (int, BaseFigure)>();
            IRandomMove tempFigur = (IRandomMove)figure;
            CoordinatePoint temp = figure.Coordinate;
            foreach (var item in tempFigur.AvailableMoves().FiltrFor(c => c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!tempFigur.IsUnderAttack(figure.Coordinate))
                {
                    if (tempFigur.AvailableMoves().Contains(CurentKing.Coordinate))
                        countList.Add(item, (GetCurrentKingMoves().Count, figure));
                }
            }
            figure.Coordinate = temp;
            if (countList.Count != 0)
            {
                keyValuePair = countList.OrderBy(k => k.Value.Item1).FirstOrDefault();
                return true;
            }
            else
            {
                keyValuePair = new KeyValuePair<CoordinatePoint, (int, BaseFigure)>();
                return false;
            }
        }

        /// <summary>
        /// Move the figure for shax with position is good
        /// </summary>
        private void MoveWithShax()
        {
            var modelNew = models.Where(c => c.Color != currentFigureColor && !(c is King)).ToList();
            var list = new List<KeyValuePair<CoordinatePoint, (int, BaseFigure)>>();
            foreach (var figur in modelNew)
            {
                if (GetMinMovesWithShax(figur, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair))
                    list.Add(keyValuePair);
            }
            var minMoves = list.OrderBy(c => c.Value.Item1).FirstOrDefault();
            var maxMoves = list.OrderBy(c => c.Value.Item1).LastOrDefault();
            BaseFigure setFigurFirst = minMoves.Value.Item2;
            BaseFigure setFigurLast = maxMoves.Value.Item2;
            var randomeMove = (IRandomMove)setFigurFirst;
            if (!randomeMove.IsUnderAttack(minMoves.Key))
            {
                setFigurFirst.SetFigurePosition(minMoves.Key);
            }
            else
            {
                setFigurLast.SetFigurePosition(maxMoves.Key);
            }
            MateMessage(this, "Mate");
        }

        /// <summary>
        /// Move the figure for block with position is good
        /// </summary>
        private void MoveWithBlock()
        {
            var modelNew = models.Where(c => c.Color != currentFigureColor).Reverse().ToList();
            var list = HalfBlock().Where(c => c.Value.Item1 != 0).ToList();
            var minMoves = list.OrderBy(c => c.Value.Item1).FirstOrDefault();
            var maxMoves = list.OrderBy(c => c.Value.Item1).LastOrDefault();
            BaseFigure setFigurFirst = minMoves.Value.Item2;
            BaseFigure setFigurLast = maxMoves.Value.Item2;
            IRandomMove randomeMove = (IRandomMove)setFigurFirst;
            if (!randomeMove.IsUnderAttack(minMoves.Key))
            {
                setFigurFirst.SetFigurePosition(minMoves.Key);
            }
            else
            {
                setFigurLast.SetFigurePosition(maxMoves.Key);
            }
        }

        /// <summary>
        /// Change maybe exist the figure with one step math 
        /// </summary>
        /// <returns>Return true if figure exist</returns>
        private bool IsMate()
        {
            var modelNew = models.Where(c => c.Color != currentFigureColor && !(c is King)).ToList();
            var list = new List<KeyValuePair<CoordinatePoint, (int, BaseFigure)>>();
            foreach (var figur in modelNew)
            {
                if (GetMinMovesWithShax(figur, out KeyValuePair<CoordinatePoint, (int, BaseFigure)> keyValuePair))
                    list.Add(keyValuePair);
            }
            if (list.Count != 0)
            {
                var minMoves = list.Where(c => c.Value.Item1 == 0).FirstOrDefault();
                return minMoves.Key != null;
            }
            return false;
        }

        [Obsolete("It is still under development, I am thinking of possible cases")]
        /// <summary>
        /// Play anti baby game
        /// </summary>
        private void AntiBabyGame()
        {
            var modelNew = models.Where(c => c.Color != currentFigureColor).ToList();
            var destination = new List<(double, BaseFigure)>();
            foreach (var figur in modelNew)
            {
                double tempDestination = figur.Coordinate - CurentKing.Coordinate;
                destination.Add((tempDestination, figur));
            }
            (double, BaseFigure) max = destination.OrderBy(k => k.Item1).FirstOrDefault();
            var tempFigur = (IRandomMove)max.Item2;
            var tempCoordinate = tempFigur.RandomMove((King)CurentKing);
            max.Item2.SetFigurePosition(tempCoordinate);
            //BaseFigure temp = modelNew[new Random().Next(0, modelNew.Count())];
            //IRandomMove tempFigur = (IRandomMove)temp;
            //var tempCoordinate = tempFigur.RandomMove((King)CurentKing);
            //MovesTextBox.Text += $"{temp.Coordinate} - " +
            //                           $"{tempCoordinate}\n{new string('-', 8)}\n";
            //temp.SetFigurePosition(tempCoordinate, Board);
            //DeleteFigur(temp);
        }

        /// <summary>
        /// change meybe figure in under attack
        /// </summary>
        /// <returns>Return true or false</returns>
        private bool UnderAttack()
        {
            var modelNew = models.Where(c => c.Color != currentFigureColor).ToList();
            foreach (var figur in modelNew)
            {
                var tempFigur = (IRandomMove)figur;
                if (tempFigur.IsUnderAttack(figur.Coordinate))
                {
                    if (!tempFigur.IsProtected(figur.Coordinate))
                    {
                        CoordinatePoint CoordinatPoint1 = tempFigur.RandomMove((King)CurentKing);
                        figur.SetFigurePosition(CoordinatPoint1);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check the available moves for current king 
        /// </summary>
        /// <returns>Return the list</returns>
        private List<CoordinatePoint> GetCurrentKingMoves()
        {
            IRandomMove currentKing = (IRandomMove)CurentKing;
            var result = new List<CoordinatePoint>();
            foreach (var item in currentKing.AvailableMoves())
            {
                if (!DangerPosition(CurentKing).Contains(item))
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
                    if (item.Color == currentFigureColor)
                    {
                        baseFigure = item;
                        break;
                    }
                }
            }
            IAvailableMoves available = (IAvailableMoves)baseFigure;
            if (baseFigure is King)
            {
                if (GetCurrentKingMoves().Contains(targetCoordinate))
                {
                    currentListForBabyGame.Add(targetCoordinate);
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
        public void IsValidForPleacement(string name)
        {
            string[] info = name.Split('.');
            CoordinatePoint coordinatePoint = new CoordinatePoint(int.Parse(info[2]), int.Parse(info[3]));
            if (GetFigure(info[0], info[1], out baseFigure))
            {
                baseFigure.setPicture += SetFigurePicture;
                baseFigure.removePicture += RemoveFigurePicture;
                baseFigure.messageForMove += MessageMove;
                models.Add(baseFigure);
                if (!GetPosition().Contains(coordinatePoint))
                {
                    if (baseFigure is King king)
                    {
                        if (!DangerPosition(king).Contains(coordinatePoint))
                        {
                            baseFigure.SetFigurePosition(coordinatePoint);
                        }
                        else
                        {
                            if (name.Split('.')[1] == "White")
                                whiteKingCount = 0;
                            else
                                blackKingCount = 0;
                        }
                    }
                    else
                    {
                        baseFigure.SetFigurePosition(coordinatePoint);
                    }
                }
            }
        }

        /// <summary>
        /// Check the figure and added the figure instance
        /// </summary>
        /// <param name="figure">The figure</param>
        /// <param name="color">The figure color</param>
        /// <param name="baseFigure">figure instance type</param>
        /// <returns>Return figure instance</returns>
        public bool GetFigure(string figure, string color, out BaseFigure baseFigure)
        {
            if (color == "White")
            {
                switch (figure)
                {
                    case "Queen":
                        if (whiteQueenCount == 0)
                        {
                            whiteQueenCount++;
                            baseFigure = new Queen(figure + "." + color + '.' + whiteQueenCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "King":
                        if (whiteKingCount == 0)
                        {
                            whiteKingCount++;
                            baseFigure = new King(figure + "." + color + '.' + whiteKingCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Rook":
                        if (whiteRookCount <= 1)
                        {
                            whiteRookCount++;
                            baseFigure = new Rook(figure + "." + color + "." + whiteRookCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Bishop":
                        if (whiteBishopCount <= 1)
                        {
                            whiteBishopCount++;
                            baseFigure = new Bishop(figure + "." + color + "." + whiteBishopCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Knight":
                        if (whiteKnightCount <= 1)
                        {
                            whiteKnightCount++;
                            baseFigure = new Knight(figure + "." + color + "." + whiteKnightCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Pawn":
                        if (whitePawnCount <= 7)
                        {
                            whitePawnCount++;
                            baseFigure = new Pawn(figure + "." + color + "." + whitePawnCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    default:
                        baseFigure = null;
                        return false;
                }
            }
            else
            {
                switch (figure)
                {
                    case "Queen":
                        if (blackQueenCount == 0)
                        {
                            blackQueenCount++;
                            baseFigure = new Queen(figure + "." + color + '.' + blackQueenCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "King":
                        if (blackKingCount == 0)
                        {
                            blackKingCount++;
                            baseFigure = new King(figure + "." + color + '.' + blackKingCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Rook":
                        if (blackRookCount <= 1)
                        {
                            blackRookCount++;
                            baseFigure = new Rook(figure + "." + color + "." + blackRookCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Bishop":
                        if (blackBishopCount <= 1)
                        {
                            blackBishopCount++;
                            baseFigure = new Bishop(figure + "." + color + "." + blackBishopCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Knight":
                        if (blackKnightCount <= 1)
                        {
                            blackKnightCount++;
                            baseFigure = new Knight(figure + "." + color + "." + blackKnightCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Pawn":
                        if (blackPawnCount <= 7)
                        {
                            blackPawnCount++;
                            baseFigure = new Pawn(figure + "." + color + "." + blackPawnCount, color, models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    default:
                        baseFigure = null;
                        return false;
                }
            }
        }

        /// <summary>
        /// Check the all figurs position
        /// </summary>
        /// <returns>Return the figurs position</returns>
        private List<CoordinatePoint> GetPosition()
        {
            var positions = new List<CoordinatePoint>();
            foreach (var item in models)
            {
                positions.Add(item.Coordinate);
            }
            return positions;
        }
        public List<string> GetNamesForReset()
        {
            var names = new List<string>();
            if (models.Count != 0)
            {
                foreach (var item in models)
                {
                    names.Add(item.Name);
                }
                whiteKingCount = 0;
                whiteQueenCount = 0;
                whiteBishopCount = 0;
                whiteRookCount = 0;
                whitePawnCount = 0;
                whiteKnightCount = 0;
                blackKingCount = 0;
                blackQueenCount = 0;
                blackBishopCount = 0;
                blackRookCount = 0;
                blackPawnCount = 0;
                blackKnightCount = 0;
                models.Clear();
            }
            else
            {
                names.Add("0");
            }
            return names;

        }
    }
}
