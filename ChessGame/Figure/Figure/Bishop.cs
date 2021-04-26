using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Bishop : BaseFigure, IDiagonal, IAvailableMoves, IDangerMoves
    {
        public Bishop(string name, string color, List<BaseFigure> othereFigures) : base(othereFigures)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<CoordinatePoint> RightIndex()
        {
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            int sum = this.Coordinate.X + this.Coordinate.Y;
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (i + j == sum)
                    {
                        CoordinatePoint CoordinatPointTemp = new CoordinatePoint(i, j);
                        arr.Add(CoordinatPointTemp);
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
                        {
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        }
                        else
                        {
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                        }
                    }
                    else
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                        {
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.Coordinate)).ToList();
                        }
                        else
                        {
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.Coordinate)).ToList();
                        }
                    }
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> LeftIndex()
        {
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
            int sub = this.Coordinate.X - this.Coordinate.Y;
            var model = othereFigures.Where(c => c != this).ToList();

            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (i - j == sub)
                    {
                        CoordinatePoint CoordinatPointTemp = new CoordinatePoint(i, j);
                        arr.Add(CoordinatPointTemp);
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
                        {
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        }
                        else
                        {
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                        }
                    }
                    else
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                        {
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.Coordinate)).ToList();
                        }
                        else
                        {
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.Coordinate)).ToList();
                        }
                    }
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> AvailableMoves()
        {
            var result = new List<CoordinatePoint>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            result.Remove(this.Coordinate);
            return result;
        }

        #region Danger Moves

        private List<CoordinatePoint> RightIndexForDanger()
        {
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            int sum = this.Coordinate.X + this.Coordinate.Y;
            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (i + j == sum)
                    {
                        CoordinatePoint CoordinatPointTemp = new CoordinatePoint(i, j);
                        arr.Add(CoordinatPointTemp);
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
                        {
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        }
                        else
                        {
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                        }
                    }
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        private List<CoordinatePoint> LeftIndexForDanger()
        {
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
            int sub = this.Coordinate.X - this.Coordinate.Y;
            var model = othereFigures.Where(c => c != this).ToList();

            for (int i = 0; i <= 7; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    if (i - j == sub)
                    {
                        CoordinatePoint CoordinatPointTemp = new CoordinatePoint(i, j);
                        arr.Add(CoordinatPointTemp);
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
                        {
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        }
                        else
                        {
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                        }
                    }
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> DangerMoves()
        {
            var vertivalList = RightIndexForDanger();
            vertivalList.AddRange(LeftIndexForDanger());
            return vertivalList;
        }
        #endregion

        #endregion
    }
}
