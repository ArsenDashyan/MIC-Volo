using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Knight : BaseFigure, ICrosswise, IAvailableMoves, IDangerMoves
    {
        public Knight(string name, string color, List<BaseFigure> othereFigures) : base(othereFigures)
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

        #region Danger Moves

        private List<CoordinatePoint> VerticalForDanger()
        {
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            if (this.Coordinate.Y + 1 <= 7)
            {
                if (this.Coordinate.X - 2 >= 0)
                    arr.Add(new CoordinatePoint(this.Coordinate.X - 2, this.Coordinate.Y + 1));
                if (this.Coordinate.X + 2 <= 7)
                    arr.Add(new CoordinatePoint(this.Coordinate.X + 2, this.Coordinate.Y + 1));
            }
            if (this.Coordinate.Y - 1 >= 0)
            {
                if (this.Coordinate.X - 2 >= 0)
                    arr.Add(new CoordinatePoint(this.Coordinate.X - 2, this.Coordinate.Y - 1));
                if (this.Coordinate.X + 2 <= 7)
                    arr.Add(new CoordinatePoint(this.Coordinate.X + 2, this.Coordinate.Y - 1));
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
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        private List<CoordinatePoint> HorizontalForDanger()
        {
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            if (this.Coordinate.Y + 2 <= 7)
            {
                if (this.Coordinate.X - 1 >= 0)
                    arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 2));
                if (this.Coordinate.X + 1 <= 7)
                    arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 2));
            }
            if (this.Coordinate.Y - 2 >= 0)
            {
                if (this.Coordinate.X - 1 >= 0)
                    arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 2));
                if (this.Coordinate.X + 1 <= 7)
                    arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 2));
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
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> DangerMoves()
        {
            var vertivalList = VerticalForDanger();
            vertivalList.AddRange(HorizontalForDanger());
            return vertivalList;
        }

        #endregion

        #endregion
    }
}
