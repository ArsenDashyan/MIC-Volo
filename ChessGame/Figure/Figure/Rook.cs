using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Rook : BaseFigure, ICrosswise, IAvailableMoves, IAntiCheck
    {
        public Rook(string name, FColor color, List<BaseFigure> othereFigures) : base(othereFigures)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<CoordinatePoint> Vertical()
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = 0; i <= 7; i++)
            {
                var coordinatPointTemp = new CoordinatePoint(this.Coordinate.X, i);
                arr.Add(coordinatPointTemp);
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        if (this.Coordinate.Y < item.Coordinate.Y)
                            arr = arr.Where(c => c.Y < item.Coordinate.Y).ToList();
                        else
                            arr = arr.Where(c => c.Y > item.Coordinate.Y).ToList();
                        item.isProtected = true;
                    }
                    else
                    {
                        if (item is King)
                        {
                            continue;
                        }
                        if (this.Coordinate.Y < item.Coordinate.Y)
                            arr = arr.Where(c => c.Y <= item.Coordinate.Y).ToList();
                        else
                            arr = arr.Where(c => c.Y >= item.Coordinate.Y).ToList();
                    }
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> Horizontal()
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = 0; i <= 7; i++)
            {
                var coordinatPointTemp = new CoordinatePoint(i, this.Coordinate.Y);
                arr.Add(coordinatPointTemp);
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        if (this.Coordinate.X < item.Coordinate.X)
                            arr = arr.Where(c => c.X < item.Coordinate.X).ToList();
                        else
                            arr = arr.Where(c => c.X > item.Coordinate.X).ToList();
                        item.isProtected = true;
                    }
                    else
                    {
                        if (item is King)
                        {
                            continue;
                        }
                        if (this.Coordinate.X < item.Coordinate.X)
                            arr = arr.Where(c => c.X <= item.Coordinate.X).ToList();
                        else
                            arr = arr.Where(c => c.X >= item.Coordinate.X).ToList();
                    }
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
            arrayHor.Remove(this.Coordinate);
            return arrayHor;
        }
        public List<CoordinatePoint> AvailableMoves()
        {
            return this.Crosswise();
        }

        /// <summary>
        /// Check the danger position for current king
        /// </summary>
        /// <param name="model">King instance withe or Black</param>
        /// <returns>Return danger position List for current king </returns>
        private static List<CoordinatePoint> DangerPosition(BaseFigure model)
        {
            var modelNew = model.othereFigures.Where(c => c.Color != model.Color);
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
                if (!DangerPosition(thisKing).Contains(thisKing.Coordinate))
                {
                    goodMoves.Add(item);
                }
                if (king.chekedFigure != null)
                {
                    if (item == king.chekedFigure.Coordinate)
                    {
                        goodMoves.Add(item);
                    }
                }
            }
            this.Coordinate = temp;
            return goodMoves;
        }
        #endregion
    }
}
