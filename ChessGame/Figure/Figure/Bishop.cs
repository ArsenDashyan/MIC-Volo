using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Bishop : BaseFigure, IDiagonal, IAvailableMoves, IAntiCheck
    {
        public Bishop(string name, FColor color) : base(name,color)
        {
            Name = name;
            Color = color;
        }

        #region Move
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
            return result;
        }
               
        #endregion
    }
}
