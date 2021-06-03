using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Pawn : BaseFigure, IVertical, IAvailableMoves, IAntiCheck
    {
        public Pawn(string name, FColor color) : base(name, color)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<CoordinatePoint> Vertical(List<BaseFigure> othereFigures)
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            if (this.Color == FColor.White)
            {
                if (!this.isMoved)
                {
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y - 1));
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y - 2));
                }
                else if (this.Coordinate.Y - 1 >= 0)
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y - 1));
                foreach (var item in othereFigures)
                {
                    if (arr.Contains(item.Coordinate))
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                    }
                }
                foreach (var item in model.Where(c => c.Color == FColor.Black))
                {
                    if (this.Coordinate.Y - 1 >= 0)
                    {
                        if (this.Coordinate.X + 1 <= 7)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 1))
                                if (item.Color == FColor.Black)
                                    arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 1));
                        }
                        if (this.Coordinate.X - 1 >= 0)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 1))
                                if (item.Color == FColor.Black)
                                    arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 1));
                        }
                    }
                }
            }
            else
            {
                if (!this.isMoved)
                {
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y + 1));
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y + 2));
                }
                else if (this.Coordinate.Y + 1 <= 7)
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y + 1));
                foreach (var item in othereFigures)
                {
                    if (arr.Contains(item.Coordinate))
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                    }
                }
                foreach (var item in othereFigures)
                {
                    if (this.Coordinate.Y + 1 >= 0)
                    {
                        if (this.Coordinate.X + 1 <= 7)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 1))
                                if (item.Color == FColor.White)
                                    arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 1));
                                else
                                    item.isProtected = true;
                        }
                        if (this.Coordinate.X - 1 >= 0)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 1))
                                if (item.Color == FColor.White)
                                    arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 1));
                                else
                                    item.isProtected = true;
                        }
                    }
                }
            }
            return arr;
        }
        public List<CoordinatePoint> Crosswise(List<BaseFigure> othereFigures)
        {
            return this.Vertical(othereFigures);
        }
        public List<CoordinatePoint> AvailableMoves(List<BaseFigure> othereFigures)
        {
            return this.Crosswise(othereFigures);
        }

        /// <summary>
        /// Check the danger position for current king
        /// </summary>
        /// <param name="model">King instance withe or Black</param>
        /// <returns>Return danger position List for current king </returns>
        private List<CoordinatePoint> DangerPosition(List<BaseFigure> othereFigures)
        {
            var modelNew = othereFigures.Where(c => c.Color != this.Color);
            var result = new List<CoordinatePoint>();
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                var array = temp.AvailableMoves(othereFigures);
                result.AddRange(array);
            }
            return result;
        }
        public List<CoordinatePoint> MovesWithKingIsNotUnderCheck(List<BaseFigure> othereFigures)
        {
            var thisKing = (BaseFigure)othereFigures.Where(c => c.Color == this.Color && c is King).Single();
            var king = (King)thisKing;
            var temp = this.Coordinate;
            var goodMoves = new List<CoordinatePoint>();
            foreach (var item in this.AvailableMoves(othereFigures))
            {
                this.Coordinate = item;
                if (!DangerPosition(othereFigures).Contains(thisKing.Coordinate))
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

