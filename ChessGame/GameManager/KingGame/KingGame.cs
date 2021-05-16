using System;
using System.Collections.Generic;
using System.Linq;
using Figure;

namespace GameManager
{
    public delegate void MessageForMate(object sender, string str);
    public delegate void Picture(object sender, string e);
    public delegate void Message(object sender, (string, string) e);
    public class KingGame
    {
        #region Property and Feld
        private static readonly List<BaseFigure> _models = new();
        private readonly List<CoordinatePoint> _currentListForBabyGame = new();
        private BaseFigure CurentKing =>
               (BaseFigure)_models.Where(c => c.Color == _currentFigureColor && c is King).Single();
        private readonly FColor _currentFigureColor;
        private BaseFigure _baseFigure;
        public event MessageForMate MateMessage;
        public event Message MessageForMove;
        public event Message MessageProgress;
        public event Picture SetPicture;
        public event Picture RemovePicture;
        private static int _whiteKingCount = 0;
        private static int _whiteQueenCount = 0;
        private static int _whiteBishopCount = 0;
        private static int _whiteRookCount = 0;
        private static int _whiteKnightCount = 0;
        private static int _blackKingCount = 0;
        private static int _blackQueenCount = 0;
        private static int _blackBishopCount = 0;
        private static int _blackRookCount = 0;
        private static int _blackKnightCount = 0;
        private static int _countForProgressStart = 0;
        #endregion

        public KingGame(string currentFigureColor)
        {
            this._currentFigureColor = currentFigureColor == "White" ? FColor.White : FColor.Black;
        }

        #region Events Method

        /// <summary>
        /// Initialize a setPicture event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void SetFigurePicture(object sender, string coordinate)
        {
            SetPicture(this, coordinate);
        }

        /// <summary>
        /// Initialize a removePicture event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void RemoveFigurePicture(object sender, string coordinate)
        {
            RemovePicture(this, coordinate);
        }

        /// <summary>
        /// Initialize a messageForMove event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="coordinate"></param>
        public void MessageMove(object sender, (string, string) coordinate)
        {
            MessageForMove(this, coordinate);
        }
        private void MessageForProgress()
        {
            if (_countForProgressStart == 0)
            {
                MessageProgress(this, ("", ""));
                _countForProgressStart++;
            }
        }

        #endregion

        /// <summary>
        /// Base logic for game
        /// </summary>
        public void Logic()
        {
            MessageForProgress();
            if (!UnderAttack())
            {
                if (!_currentListForBabyGame.BabyGame())
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
            var tempFigur = (IAvailableMoves)figure;
            CoordinatePoint temp = figure.Coordinate;
            int targetX = CurentKing.Coordinate.X + 1 == 8 ? CurentKing.Coordinate.X : CurentKing.Coordinate.X + 1;
            int targetY = CurentKing.Coordinate.Y - 1 == -1 ? CurentKing.Coordinate.Y : CurentKing.Coordinate.Y - 1;
            foreach (var item in tempFigur.AvailableMoves().Where(c => c != temp).ToList().FiltrFor(c => c.X <= CurentKing.Coordinate.X && c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!IsUnderAttack(figure, figure.Coordinate))
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
            var tempFigur = (IAvailableMoves)figure;
            CoordinatePoint temp = figure.Coordinate;
            int targetX = CurentKing.Coordinate.X - 1 == -1 ? CurentKing.Coordinate.X : CurentKing.Coordinate.X - 1;
            int targetY = CurentKing.Coordinate.Y - 1 == -1 ? CurentKing.Coordinate.Y : CurentKing.Coordinate.Y - 1;
            foreach (var item in tempFigur.AvailableMoves().Where(c => c!=temp).ToList().FiltrFor(c => c.X >= CurentKing.Coordinate.X && c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!IsUnderAttack(figure, figure.Coordinate))
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
            var tempFigur = (IAvailableMoves)figure;
            CoordinatePoint temp = figure.Coordinate;
            int targetX = CurentKing.Coordinate.X - 1 == -1 ? CurentKing.Coordinate.X : CurentKing.Coordinate.X - 1;
            int targetY = CurentKing.Coordinate.Y + 1 == 8 ? CurentKing.Coordinate.Y : CurentKing.Coordinate.Y + 1;
            foreach (var item in tempFigur.AvailableMoves().Where(c => c != temp).ToList().FiltrFor(c => c.X >= CurentKing.Coordinate.X && c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!IsUnderAttack(figure, figure.Coordinate))
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
            var tempFigur = (IAvailableMoves)figure;
            CoordinatePoint temp = figure.Coordinate;
            int targetX = CurentKing.Coordinate.X + 1 == 8 ? CurentKing.Coordinate.X : CurentKing.Coordinate.X + 1;
            int targetY = CurentKing.Coordinate.Y + 1 == 8 ? CurentKing.Coordinate.Y : CurentKing.Coordinate.Y + 1;
            foreach (var item in tempFigur.AvailableMoves().Where(c => c != temp).ToList().FiltrFor(c => c.X <= CurentKing.Coordinate.X && c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!IsUnderAttack(figure, figure.Coordinate))
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
            var modelNew = _models.Where(c => c.Color != _currentFigureColor).Reverse().ToList();
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
            var tempFigur = (IAvailableMoves)figure;
            CoordinatePoint temp = figure.Coordinate;
            foreach (var item in tempFigur.AvailableMoves().FiltrFor(c => c - CurentKing.Coordinate >= 2))
            {
                figure.Coordinate = item;
                if (!IsUnderAttack(figure, figure.Coordinate))
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
            var modelNew = _models.Where(c => c.Color != _currentFigureColor && !(c is King)).ToList();
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
            var randomeMove = (IAvailableMoves)setFigurFirst;
            if (!IsUnderAttack(setFigurFirst, minMoves.Key))
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
            var modelNew = _models.Where(c => c.Color != _currentFigureColor).Reverse().ToList();
            var list = HalfBlock().Where(c => c.Value.Item1 != 0).ToList();
            var minMoves = list.OrderBy(c => c.Value.Item1).FirstOrDefault();
            var maxMoves = list.OrderBy(c => c.Value.Item1).LastOrDefault();
            BaseFigure setFigurFirst = minMoves.Value.Item2;
            BaseFigure setFigurLast = maxMoves.Value.Item2;
            var randomeMove = (IAvailableMoves)setFigurFirst;
            if (!IsUnderAttack(setFigurFirst, minMoves.Key))
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
            var modelNew = _models.Where(c => c.Color != _currentFigureColor && !(c is King)).ToList();
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

        /// <summary>
        /// Check figure is under attack or no
        /// </summary>
        /// <param name="baseFigure">current figure</param>
        /// <param name="CoordinatPoint">figure coordinate</param>
        /// <returns>Return true if figure is under attack</returns>
        private static bool IsUnderAttack(BaseFigure baseFigure, CoordinatePoint CoordinatPoint)
        {
            var modelNew = _models.Where(c => c.Color != baseFigure.Color).ToList();
            foreach (var item in modelNew)
            {
                IAvailableMoves itemFigur = (IAvailableMoves)item;
                if (itemFigur.AvailableMoves().Contains(CoordinatPoint))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check figure is protected or no
        /// </summary>
        /// <param name="baseFigure">Current figure</param>
        /// <param name="CoordinatPoint">Current coordinate</param>
        /// <returns>Return true if figure is protected</returns>
        private static bool IsProtected(BaseFigure baseFigure, CoordinatePoint CoordinatPoint)
        {
            var model = _models.Where(c => c != baseFigure && c.Color == baseFigure.Color).ToList();
            foreach (var item in model)
            {
                IAvailableMoves tempfigur = (IAvailableMoves)item;
                if (tempfigur.AvailableMoves().Contains(CoordinatPoint))
                    return true;
            }
            return false;
        }

        #region Random Move

        /// <summary>
        /// Check a randome move for current figure
        /// </summary>
        /// <param name="baseFigure">Currnnt Figure</param>
        /// <param name="king">Current king</param>
        /// <returns>Return the new coordinate for random move</returns>
        private static CoordinatePoint RandomMove(BaseFigure baseFigure, King king)
        {
            CoordinatePoint temp = null;
            IAvailableMoves tempFigure = (IAvailableMoves)baseFigure;
            if (ProtectedShax(baseFigure, king, out CoordinatePoint tempForItem))
            {
                temp = tempForItem;
                return temp;
            }
            else if (IsUnderAttackShax(baseFigure, king, out CoordinatePoint tempForItem2))
            {
                temp = tempForItem2;
                return temp;
            }
            else if (IsUnderAttackMax(baseFigure, king, out CoordinatePoint tempForItem3))
            {
                temp = tempForItem3;
                return temp;
            }
            if (temp == null)
            {
                foreach (var item in tempFigure.AvailableMoves())
                {
                    if (!IsUnderAttack(baseFigure, item))
                    {
                        temp = item;
                        break;
                    }
                }
            }
            return temp;
        }
        private static bool ProtectedShax(BaseFigure baseFigure, King king, out CoordinatePoint tempForItem)
        {
            CoordinatePoint temp = baseFigure.Coordinate;
            IAvailableMoves tempFigure = (IAvailableMoves)baseFigure;
            foreach (var item in tempFigure.AvailableMoves())
            {
                baseFigure.Coordinate = item;
                if (IsProtected(baseFigure, item))
                {
                    if (CoordinatePoint.Modul(item, king.Coordinate) >= 2d)
                    {
                        if (tempFigure.AvailableMoves().Contains(king.Coordinate))
                        {
                            tempForItem = item;
                            baseFigure.Coordinate = temp;
                            return true;
                        }
                    }
                }
            }
            baseFigure.Coordinate = temp;
            tempForItem = null;
            return false;
        }
        private static bool IsUnderAttackShax(BaseFigure baseFigure, King king, out CoordinatePoint tempForItem)
        {
            CoordinatePoint temp = baseFigure.Coordinate;
            IAvailableMoves tempFigure = (IAvailableMoves)baseFigure;
            foreach (var item in tempFigure.AvailableMoves())
            {
                baseFigure.Coordinate = item;
                if (!IsUnderAttack(baseFigure, baseFigure.Coordinate))
                {
                    if (CoordinatePoint.Modul(item, king.Coordinate) >= 2d)
                    {
                        if (tempFigure.AvailableMoves().Contains(king.Coordinate))
                        {
                            tempForItem = item;
                            baseFigure.Coordinate = temp;
                            return true;
                        }
                    }
                }
            }
            baseFigure.Coordinate = temp;
            tempForItem = null;
            return false;
        }
        private static bool IsUnderAttackMax(BaseFigure baseFigure, King king, out CoordinatePoint tempForItem)
        {
            CoordinatePoint temp = baseFigure.Coordinate;
            IAvailableMoves tempFigure = (IAvailableMoves)baseFigure;
            foreach (var item in tempFigure.AvailableMoves())
            {
                baseFigure.Coordinate = item;
                if (!IsUnderAttack(baseFigure, baseFigure.Coordinate))
                {
                    if (CoordinatePoint.Modul(item, king.Coordinate) > 2)
                    {
                        if (tempFigure.AvailableMoves().Count == 14)
                        {
                            tempForItem = item;
                            baseFigure.Coordinate = temp;
                            return true;
                        }
                    }
                }
            }
            baseFigure.Coordinate = temp;
            tempForItem = null;
            return false;
        }

        #endregion

        [Obsolete("It is still under development, I am thinking of possible cases")]
        /// <summary>
        /// Play anti baby game
        /// </summary>
        private void AntiBabyGame()
        {
            var modelNew = _models.Where(c => c.Color != _currentFigureColor).ToList();
            var destination = new List<(double, BaseFigure)>();
            foreach (var figur in modelNew)
            {
                double tempDestination = figur.Coordinate - CurentKing.Coordinate;
                destination.Add((tempDestination, figur));
            }
            (double, BaseFigure) max = destination.OrderBy(k => k.Item1).FirstOrDefault();
            var tempCoordinate = RandomMove(max.Item2, (King)CurentKing);
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
            var modelNew = _models.Where(c => c.Color != _currentFigureColor).ToList();
            foreach (var figur in modelNew)
            {
                var tempFigur = (IAvailableMoves)figur;
                if (IsUnderAttack(figur, figur.Coordinate))
                {
                    if (!IsProtected(figur, figur.Coordinate))
                    {
                        var coordinatPoint1 = RandomMove(figur, (King)CurentKing);
                        figur.SetFigurePosition(coordinatPoint1);
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
            var currentKing = (IAvailableMoves)CurentKing;
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
        private static List<CoordinatePoint> DangerPosition(BaseFigure model)
        {
            var modelNew = _models.Where(c => c.Color != model.Color);
            var result = new List<CoordinatePoint>();
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                var array = temp.AvailableMoves();
                result.AddRange(array);
            }
            return result;
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
            foreach (var item in _models)
            {
                if (item.Coordinate == currentCoordinate)
                {
                    if (item.Color == _currentFigureColor)
                    {
                        _baseFigure = item;
                        break;
                    }
                }
            }
            IAvailableMoves available = (IAvailableMoves)_baseFigure;
            if (_baseFigure is King)
            {
                if (GetCurrentKingMoves().Contains(targetCoordinate))
                {
                    _currentListForBabyGame.Add(targetCoordinate);
                    _baseFigure.SetFigurePosition(targetCoordinate);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (available.AvailableMoves().Contains(targetCoordinate))
            {
                _baseFigure.SetFigurePosition(targetCoordinate);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if coordinate is valid for figure for pleacement, and set this figure
        /// </summary>
        /// <param name="current">Current coordinate with string format</param>
        /// <param name="target">Target coordinate with string format</param>
        /// <returns>Return true if target coordinate is valid for figure</returns>
        public void IsValidForPleacement(string name)
        {
            string[] info = name.Split('.');
            var coordinatePoint = new CoordinatePoint(int.Parse(info[2]), int.Parse(info[3]));
            if (GetFigure(info[0], info[1], out _baseFigure))
            {
                _baseFigure.SetPicture += SetFigurePicture;
                _baseFigure.RemovePicture += RemoveFigurePicture;
                _baseFigure.DeletePicture += delegate {};
                _baseFigure.MessageForMove += MessageMove;
                _models.Add(_baseFigure);
                if (!GetPosition().Contains(coordinatePoint))
                {
                    if (_baseFigure is King king)
                    {
                        if (!DangerPosition(king).Contains(coordinatePoint))
                        {
                            _baseFigure.SetFigurePosition(coordinatePoint);
                        }
                        else
                        {
                            if (name.Split('.')[1] == "White")
                                _whiteKingCount = 0;
                            else
                                _blackKingCount = 0;
                        }
                    }
                    else
                    {
                        _baseFigure.SetFigurePosition(coordinatePoint);
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
        public static bool GetFigure(string figure, string color, out BaseFigure baseFigure)
        {
            if (color == "White")
            {
                switch (figure)
                {
                    case "Queen":
                        if (_whiteQueenCount == 0)
                        {
                            _whiteQueenCount++;
                            baseFigure = new Queen(figure + "." + color + '.' + _whiteQueenCount, FColor.White, _models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "King":
                        if (_whiteKingCount == 0)
                        {
                            _whiteKingCount++;
                            baseFigure = new King(figure + "." + color + '.' + _whiteKingCount, FColor.White, _models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Rook":
                        if (_whiteRookCount <= 1)
                        {
                            _whiteRookCount++;
                            baseFigure = new Rook(figure + "." + color + "." + _whiteRookCount, FColor.White, _models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Bishop":
                        if (_whiteBishopCount <= 1)
                        {
                            _whiteBishopCount++;
                            baseFigure = new Bishop(figure + "." + color + "." + _whiteBishopCount, FColor.White, _models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Knight":
                        if (_whiteKnightCount <= 1)
                        {
                            _whiteKnightCount++;
                            baseFigure = new Knight(figure + "." + color + "." + _whiteKnightCount, FColor.White, _models);
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
                        if (_blackQueenCount == 0)
                        {
                            _blackQueenCount++;
                            baseFigure = new Queen(figure + "." + color + '.' + _blackQueenCount, FColor.Black, _models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "King":
                        if (_blackKingCount == 0)
                        {
                            _blackKingCount++;
                            baseFigure = new King(figure + "." + color + '.' + _blackKingCount, FColor.Black, _models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Rook":
                        if (_blackRookCount <= 1)
                        {
                            _blackRookCount++;
                            baseFigure = new Rook(figure + "." + color + "." + _blackRookCount, FColor.Black, _models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Bishop":
                        if (_blackBishopCount <= 1)
                        {
                            _blackBishopCount++;
                            baseFigure = new Bishop(figure + "." + color + "." + _blackBishopCount, FColor.Black, _models);
                            return true;
                        }
                        else
                        {
                            baseFigure = null;
                            return false;
                        }
                    case "Knight":
                        if (_blackKnightCount <= 1)
                        {
                            _blackKnightCount++;
                            baseFigure = new Knight(figure + "." + color + "." + _blackKnightCount, FColor.Black, _models);
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
        private static List<CoordinatePoint> GetPosition()
        {
            var positions = new List<CoordinatePoint>();
            foreach (var item in _models)
            {
                positions.Add(item.Coordinate);
            }
            return positions;
        }

        /// <summary>
        /// Get all figures names for reset bord
        /// </summary>
        /// <returns>Return all figures names</returns>
        public static List<string> GetNamesForReset()
        {
            var names = new List<string>();
            if (_models.Count != 0)
            {
                foreach (var item in _models)
                {
                    names.Add(item.Name);
                }
                _whiteKingCount = 0;
                _whiteQueenCount = 0;
                _whiteBishopCount = 0;
                _whiteRookCount = 0;
                _whiteKnightCount = 0;
                _blackKingCount = 0;
                _blackQueenCount = 0;
                _blackBishopCount = 0;
                _blackRookCount = 0;
                _blackKnightCount = 0;
                _countForProgressStart = 0;
                _models.Clear();
            }
            else
                names = null;
            return names;
        }

        private static CoordinatePoint GetCoordinateByString(string path)
        {
            string[] strCurrent = path.Split('.');
            return new CoordinatePoint(int.Parse(strCurrent[0]), int.Parse(strCurrent[1]));
        }

        /// <summary>
        /// Check a current figure with a coordinate
        /// </summary>
        /// <param name="coordinatePoint">Current coordinate</param>
        /// <returns>Return a current figure</returns>
        private static BaseFigure CheckedCurrentFigure(CoordinatePoint coordinatePoint)
        {
            foreach (var item in _models)
            {
                if (item.Coordinate == coordinatePoint)
                    return item;
            }
            return null;
        }
        public static List<string> GetAvalibleMoves(string coordinate)
        {
            var result = new List<string>();
            var currentCoordinate = GetCoordinateByString(coordinate);
            var baseFigure = CheckedCurrentFigure(currentCoordinate);
            var antiCheck = (IAntiCheck)baseFigure;
            var movesList = antiCheck.MovesWithKingIsNotUnderCheck();
            foreach (var item in movesList)
            {
                result.Add($"{item.X}.{item.Y}");
            }
            return result;
        }
    }
}
