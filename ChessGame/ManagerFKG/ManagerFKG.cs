using System;
using System.Collections.Generic;
using System.Linq;
using Helper;
using Figure;

namespace ManagerFKG
{
    class Manager<T,U>
    {
        #region Property and Feld
        private readonly List<BaseFigure> models;
        private readonly List<CoordinatePoint> currentListForBabyGame;
        private readonly BaseFigure CurentKing;
        private readonly string currentFigureColor;
        #endregion

        public Manager(List<CoordinatePoint> currentListForBabyGame, BaseFigure CurentKing, List<BaseFigure> models,
                                 string currentFigureColor)
        {
            this.currentListForBabyGame = currentListForBabyGame;
            this.CurentKing = CurentKing;
            this.models = models;
            this.currentFigureColor = currentFigureColor;
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
            //if (GetCurrentKingMoves().Count == 0)
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
    }
}
