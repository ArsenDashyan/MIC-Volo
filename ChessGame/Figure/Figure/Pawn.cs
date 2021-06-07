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
                else if (this.Coordinate.Y - 1 >= downPoint)
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
                foreach (var item in model.Where(f => f.Color == FColor.Black))
                {
                    if (this.Coordinate.Y - 1 >= downPoint)
                    {
                        if (this.Coordinate.X + 1 <= topPoint)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 1))
                                arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 1));
                        }
                        if (this.Coordinate.X - 1 >= downPoint)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 1))
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
                foreach (var item in model.Where(f => f.Color == FColor.White))
                {
                    if (this.Coordinate.Y + 1 >= downPoint)
                    {
                        if (this.Coordinate.X + 1 <= topPoint)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 1))
                                arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 1));
                        }
                        if (this.Coordinate.X - 1 >= downPoint)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 1))
                                arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 1));
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

        #endregion
    }
}

