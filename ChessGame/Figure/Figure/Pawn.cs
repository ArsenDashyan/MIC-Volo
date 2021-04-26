using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class Pawn : BaseFigure, IVertical, IAvailableMoves, IDangerMoves
    {
        public Pawn(string name, string color, List<BaseFigure> othereFigures) : base(othereFigures)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<CoordinatePoint> Vertical()
        {
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            if (this.Color == "White")
            {
                if (this.Coordinate.Y == 6)
                {
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y - 1));
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y - 2));
                }
                else
                {
                    if (this.Coordinate.Y - 1 >= 0)
                    {
                        arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y - 1));
                    }
                }
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
                foreach (var item in model.Where(c => c.Color == "Black"))
                {
                    if (this.Coordinate.Y - 1 >= 0)
                    {
                        if (this.Coordinate.X + 1 <= 7)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 1))
                            {
                                arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 1));
                            }
                        }
                        if (this.Coordinate.X - 1 >= 0)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 1))
                            {
                                arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 1));
                            }
                        }
                    }
                }
            }
            else
            {
                if (this.Coordinate.Y == 1)
                {
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y + 1));
                    arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y + 2));
                }
                else
                {
                    if (this.Coordinate.Y + 1 <= 7)
                    {
                        arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y + 1));
                    }
                }
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
                foreach (var item in model.Where(c => c.Color == "White"))
                {
                    if (this.Coordinate.Y + 1 >= 0)
                    {
                        if (this.Coordinate.X + 1 <= 7)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 1))
                            {
                                arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 1));
                            }
                        }
                        if (this.Coordinate.X - 1 >= 0)
                        {
                            if (item.Coordinate == new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 1))
                            {
                                arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 1));
                            }
                        }
                    }
                }
            }


            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> Crosswise()
        {
            return this.Vertical();
        }
        public List<CoordinatePoint> AvailableMoves()
        {
            var result = new List<CoordinatePoint>();
            result.AddRange(Crosswise());
            result.Remove(this.Coordinate);
            return result;
        }

        #region Danger Moves
        private List<CoordinatePoint> VerticalForDanger()
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            if (this.Color == "Black")
            {
                if (this.Coordinate.Y + 1 >= 0)
                {
                    if (this.Coordinate.X + 1 <= 7)
                    {
                        arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 1));
                    }
                    if (this.Coordinate.X - 1 >= 0)
                    {
                        arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 1));
                    }
                }
            }
            else
            {
                if (this.Coordinate.Y - 1 >= 0)
                {
                    if (this.Coordinate.X + 1 <= 7)
                    {
                        arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 1));
                    }
                    if (this.Coordinate.X - 1 >= 0)
                    {
                        arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 1));
                    }
                }
            }
            return arr;
        }
        public List<CoordinatePoint> DangerMoves()
        {
            return this.VerticalForDanger();
        }
        #endregion
        #endregion
    }
}

