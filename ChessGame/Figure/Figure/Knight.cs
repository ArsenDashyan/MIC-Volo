using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Knight : BaseFigure, ICrosswise, IAvailableMoves
    {
        public Knight(string name, FColor color, List<BaseFigure> othereFigures) : base(othereFigures)
        {
            Name = name;
            Color = color;
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
                        result.Remove(item.Coordinate);
                    }
                }
            }
            return result;
        }
        public List<CoordinatePoint> Vertical()
        {
            List<CoordinatePoint> result = new List<CoordinatePoint>();
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

        #endregion
    }
}
