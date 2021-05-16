using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Bishop : BaseFigure, IDiagonal, IAvailableMoves, IAntiCheck
    {
        public readonly List<BaseFigure> othereFigures;
        public Bishop(string name, FColor color, List<BaseFigure> allFigures) : base(color)
        {
            Name = name;
            Color = color;
            othereFigures = allFigures;
        }

        #region Move
        public List<CoordinatePoint> RightIndex()
        {
            int sum = this.Coordinate.X + this.Coordinate.Y;
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (i + j == sum)
                    {
                        var coordinatPointTemp = new CoordinatePoint(i, j);
                        arr.Add(coordinatPointTemp);
                    }
                }
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                        item.isProtected = true;
                    }
                    else
                    {
                        if (item is King)
                            continue;
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.Coordinate)).ToList();
                    }
                }
            }
            return arr;
        }
        public List<CoordinatePoint> LeftIndex()
        {
            var arr = new List<CoordinatePoint>();
            int sub = this.Coordinate.X - this.Coordinate.Y;
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (i - j == sub)
                    {
                        var coordinatPointTemp = new CoordinatePoint(i, j);
                        arr.Add(coordinatPointTemp);
                    }
                }
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                        item.isProtected = true;
                    }
                    else
                    {
                        if (item is King)
                        {
                            continue;
                        }
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.Coordinate)).ToList();
                    }
                }
            }
            return arr;
        }
        public List<CoordinatePoint> AvailableMoves()
        {
            var result = new List<CoordinatePoint>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            return result;
        }

        /// <summary>
        /// Check the danger position for current king
        /// </summary>
        /// <param name="model">King instance withe or Black</param>
        /// <returns>Return danger position List for current king </returns>
        private List<CoordinatePoint> DangerPosition()
        {
            var modelNew = othereFigures.Where(c => c.Color != this.Color);
            var result = new List<CoordinatePoint>();
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                var array = temp.AvailableMoves();
                result.AddRange(array);
            }
            return result;
        }
        public List<CoordinatePoint> MovesWithKingIsNotUnderCheck()
        {
            var thisKing = (BaseFigure)othereFigures.Where(c => c.Color == this.Color && c is King).Single();
            var king = (King)thisKing;
            var temp = this.Coordinate;
            var goodMoves = new List<CoordinatePoint>();
            foreach (var item in this.AvailableMoves())
            {
                this.Coordinate = item;
                if (!DangerPosition().Contains(thisKing.Coordinate))
                {
                    goodMoves.Add(item);
                }
                if (king.chekedFigure != null)
                {
                    if (item == king.chekedFigure.Coordinate)
                        goodMoves.Add(item);
                }
            }
            this.Coordinate = temp;
            return goodMoves;
        }

        #endregion
    }
}
