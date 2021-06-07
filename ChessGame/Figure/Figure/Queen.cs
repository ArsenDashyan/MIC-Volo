using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Queen : BaseFigure, IDiagonal, ICrosswise, IAvailableMoves, IAntiCheck
    {
        public Queen(string name, FColor color) : base(name, color)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<CoordinatePoint> Vertical(List<BaseFigure> othereFigures)
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = downPoint; i <= topPoint; i++)
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
            return arr;
        }
        public List<CoordinatePoint> Horizontal(List<BaseFigure> othereFigures)
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = downPoint; i <= topPoint; i++)
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
            return arr;
        }
        public List<CoordinatePoint> Crosswise(List<BaseFigure> othereFigures)
        {
            var arrayHor = this.Horizontal(othereFigures);
            var arrayVert = this.Vertical(othereFigures);
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public List<CoordinatePoint> RightIndex(List<BaseFigure> othereFigures)
        {
            int sum = this.Coordinate.X + this.Coordinate.Y;
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = downPoint; i <= topPoint; i++)
            {
                for (int j = downPoint; j <= topPoint; j++)
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
        public List<CoordinatePoint> LeftIndex(List<BaseFigure> othereFigures)
        {
            var arr = new List<CoordinatePoint>();
            int sub = this.Coordinate.X - this.Coordinate.Y;
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = downPoint; i <= topPoint; i++)
            {
                for (int j = downPoint; j <= topPoint; j++)
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
        public List<CoordinatePoint> AvailableMoves(List<BaseFigure> othereFigures)
        {
            var result = new List<CoordinatePoint>();
            result.AddRange(RightIndex(othereFigures));
            result.AddRange(LeftIndex(othereFigures));
            result.AddRange(Crosswise(othereFigures));
            return result;
        }

        #endregion
    }
}
