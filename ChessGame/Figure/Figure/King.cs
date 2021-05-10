using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public delegate void MessageForMate(object sender, string str);
    public class King : BaseFigure, ICrosswise, IDiagonal, IAvailableMoves, IAntiCheck
    {
        public BaseFigure chekedFigure;
        public event MessageForMate MessageCheck;
        public King(string name, FColor color, List<BaseFigure> othereFigures) : base(othereFigures)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<CoordinatePoint> Horizontal()
        {
            var arr = new List<CoordinatePoint>();
            var newArr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.X - 1 >= 0)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y));
                arr.Add(this.Coordinate);
            }
            if (this.Coordinate.X + 1 <= 7)
            {
                arr.Add(this.Coordinate);
                arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y));
            }
            if (!this.isMoved)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X + 2, this.Coordinate.Y));
                arr.Add(new CoordinatePoint(this.Coordinate.X - 2, this.Coordinate.Y));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
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
            newArr = arr.Distinct().ToList();
            newArr.Remove(this.Coordinate);
            return newArr;
        }
        public List<CoordinatePoint> Vertical()
        {
            var arr = new List<CoordinatePoint>();
            var newArr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.Y - 1 >= 0)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y - 1));
                arr.Add(this.Coordinate);
            }
            if (this.Coordinate.Y + 1 <= 7)
            {
                arr.Add(this.Coordinate);
                arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y + 1));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
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
            newArr = arr.Distinct().ToList();
            newArr.Remove(this.Coordinate);
            return newArr;
        }
        public List<CoordinatePoint> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public List<CoordinatePoint> RightIndex()
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.X + 1 <= 7 && this.Coordinate.Y - 1 >= 0)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 1));
            }
            if (this.Coordinate.X - 1 >= 0 && this.Coordinate.Y + 1 <= 7)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 1));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
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
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> LeftIndex()
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.X - 1 >= 0 && this.Coordinate.Y - 1 >= 0)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 1));
            }
            if (this.Coordinate.X + 1 <= 7 && this.Coordinate.Y + 1 <= 7)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 1));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
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
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> AvailableMoves()
        {
            var result = new List<CoordinatePoint>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            result.AddRange(Crosswise());
            result.Remove(this.Coordinate);
            return result;
        }

        /// <summary>
        /// Check the danger position for current king
        /// </summary>
        /// <param name="model">King instance withe or Black</param>
        /// <returns>Return danger position List for current king </returns>
        private static List<CoordinatePoint> DangerPosition(BaseFigure model)
        {
            var modelNew = model.othereFigures.Where(c => c.Color != model.Color);
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
                if (!DangerPosition(thisKing).Contains(thisKing.Coordinate))
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

        /// <summary>
        /// Checked Current king is check or no
        /// </summary>
        public void IsCheked()
        {
            var modelNew = othereFigures.Where(f => f.Color != this.Color).ToList();
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                if (temp.AvailableMoves().Contains(this.Coordinate))
                {
                    this.chekedFigure = item;
                    MessageCheck(this, "Check");
                    break;
                }
                else
                    MessageCheck(this, " ");
            }
        }
    }
}
