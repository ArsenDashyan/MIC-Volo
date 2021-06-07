using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Knight : BaseFigure, ICrosswise, IAvailableMoves, IAntiCheck
    {
        public Knight(string name, FColor color) : base(name,color)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<CoordinatePoint> Horizontal(List<BaseFigure> othereFigures)
        {
            var result = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            if (this.Coordinate.Y + 2 <= topPoint)
            {
                if (this.Coordinate.X - 1 >= downPoint)
                    result.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 2));
                if (this.Coordinate.X + 1 <= topPoint)
                    result.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 2));
            }
            if (this.Coordinate.Y - 2 >= downPoint)
            {
                if (this.Coordinate.X - 1 >= downPoint)
                    result.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 2));
                if (this.Coordinate.X + 1 <= topPoint)
                    result.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 2));
            }
            foreach (var item in model)
            {
                if (result.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                        result.Remove(item.Coordinate);
                }
            }
            return result;
        }
        public List<CoordinatePoint> Vertical(List<BaseFigure> othereFigures)
        {
            var result = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            if (this.Coordinate.Y + 1 <= topPoint)
            {
                if (this.Coordinate.X - 2 >= downPoint)
                    result.Add(new CoordinatePoint(this.Coordinate.X - 2, this.Coordinate.Y + 1));
                if (this.Coordinate.X + 2 <= topPoint)
                    result.Add(new CoordinatePoint(this.Coordinate.X + 2, this.Coordinate.Y + 1));
            }
            if (this.Coordinate.Y - 1 >= downPoint)
            {
                if (this.Coordinate.X - 2 >= downPoint)
                    result.Add(new CoordinatePoint(this.Coordinate.X - 2, this.Coordinate.Y - 1));
                if (this.Coordinate.X + 2 <= topPoint)
                    result.Add(new CoordinatePoint(this.Coordinate.X + 2, this.Coordinate.Y - 1));
            }
            foreach (var item in model)
            {
                if (result.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                        result.Remove(item.Coordinate);
                }
            }
            return result;
        }
        public List<CoordinatePoint> Crosswise(List<BaseFigure> othereFigures)
        {
            var arrayHor = this.Horizontal(othereFigures);
            var arrayVert = this.Vertical(othereFigures);
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public List<CoordinatePoint> AvailableMoves(List<BaseFigure> othereFigures)
        {
            return this.Crosswise(othereFigures);
        }

        #endregion
    }
}
