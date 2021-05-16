using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Knight : BaseFigure, ICrosswise, IAvailableMoves, IAntiCheck
    {
        public readonly List<BaseFigure> othereFigures;
        public Knight(string name, FColor color, List<BaseFigure> allFigures) : base(color)
        {
            Name = name;
            Color = color;
            othereFigures = allFigures;
        }

        #region Move
        public List<CoordinatePoint> Horizontal()
        {
            var result = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            if (this.Coordinate.Y + 2 <= 7)
            {
                if (this.Coordinate.X - 1 >= 0)
                    result.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 2));
                if (this.Coordinate.X + 1 <= 7)
                    result.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 2));
            }
            if (this.Coordinate.Y - 2 >= 0)
            {
                if (this.Coordinate.X - 1 >= 0)
                    result.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 2));
                if (this.Coordinate.X + 1 <= 7)
                    result.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 2));
            }
            foreach (var item in model)
            {
                if (result.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        item.isProtected = true;
                        result.Remove(item.Coordinate);
                    }
                }
            }
            return result;
        }
        public List<CoordinatePoint> Vertical()
        {
            var result = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            if (this.Coordinate.Y + 1 <= 7)
            {
                if (this.Coordinate.X - 2 >= 0)
                    result.Add(new CoordinatePoint(this.Coordinate.X - 2, this.Coordinate.Y + 1));
                if (this.Coordinate.X + 2 <= 7)
                    result.Add(new CoordinatePoint(this.Coordinate.X + 2, this.Coordinate.Y + 1));
            }
            if (this.Coordinate.Y - 1 >= 0)
            {
                if (this.Coordinate.X - 2 >= 0)
                    result.Add(new CoordinatePoint(this.Coordinate.X - 2, this.Coordinate.Y - 1));
                if (this.Coordinate.X + 2 <= 7)
                    result.Add(new CoordinatePoint(this.Coordinate.X + 2, this.Coordinate.Y - 1));
            }
            foreach (var item in model)
            {
                if (result.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        item.isProtected = true;
                        result.Remove(item.Coordinate);
                    }
                }
            }
            return result;
        }
        public List<CoordinatePoint> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
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
