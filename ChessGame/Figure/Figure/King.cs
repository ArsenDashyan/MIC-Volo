﻿using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public delegate void MessageForMate(object sender, string str);
    public class King : BaseFigure, ICrosswise, IDiagonal, IAvailableMoves, IAntiCheck
    {
        public event MessageForMate MessageCheck;
        public King(string name, FColor color) : base(name, color)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<CoordinatePoint> Horizontal(List<BaseFigure> othereFigures)
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.X - 1 >= downPoint)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y));
                arr.Add(this.Coordinate);
            }
            if (this.Coordinate.X + 1 <= topPoint)
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
                    arr.Remove(item.Coordinate);
            }
            var newArr = arr.Distinct().ToList();
            newArr.Remove(this.Coordinate);
            return newArr;
        }
        public List<CoordinatePoint> Vertical(List<BaseFigure> othereFigures)
        {
            var arr = new List<CoordinatePoint>();
            var newArr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.Y - 1 >= downPoint)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y - 1));
                arr.Add(this.Coordinate);
            }
            if (this.Coordinate.Y + 1 <= topPoint)
            {
                arr.Add(this.Coordinate);
                arr.Add(new CoordinatePoint(this.Coordinate.X, this.Coordinate.Y + 1));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
                    arr.Remove(item.Coordinate);
            }
            newArr = arr.Distinct().ToList();
            newArr.Remove(this.Coordinate);
            return newArr;
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
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.X + 1 <= topPoint && this.Coordinate.Y - 1 >= downPoint)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y - 1));
            }
            if (this.Coordinate.X - 1 >= downPoint && this.Coordinate.Y + 1 <= topPoint)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y + 1));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
                {
                    if (arr.Contains(item.Coordinate))
                        arr.Remove(item.Coordinate);
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> LeftIndex(List<BaseFigure> othereFigures)
        {
            var arr = new List<CoordinatePoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.X - 1 >= downPoint && this.Coordinate.Y - 1 >= downPoint)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X - 1, this.Coordinate.Y - 1));
            }
            if (this.Coordinate.X + 1 <= 7 && this.Coordinate.Y + 1 <= topPoint)
            {
                arr.Add(new CoordinatePoint(this.Coordinate.X + 1, this.Coordinate.Y + 1));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
                    arr.Remove(item.Coordinate);
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatePoint> AvailableMoves(List<BaseFigure> othereFigures)
        {
            var result = new List<CoordinatePoint>();
            result.AddRange(RightIndex(othereFigures));
            result.AddRange(LeftIndex(othereFigures));
            result.AddRange(Crosswise(othereFigures));
            result.Remove(this.Coordinate);
            return result;
        }

        #endregion

        /// <summary>
        /// Checked Current king is check or no
        /// </summary>
        public void IsCheked(List<BaseFigure> othereFigures)
        {
            var modelNew = othereFigures.Where(f => f.Color != this.Color).ToList();
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                if (temp.AvailableMoves(othereFigures).Contains(this.Coordinate))
                {
                    IsMate(othereFigures);
                    MessageCheck(this, "Check");
                    break;
                }
                else
                    MessageCheck(this, " ");
            }
        }
        private void IsMate(List<BaseFigure> othereFigures)
        {
            var modelNew = othereFigures.Where(f => f.Color == this.Color).ToList();
            var moves = new List<CoordinatePoint>();
            foreach (var item in modelNew)
            {
                var temp = (IAntiCheck)item;
                moves.AddRange(temp.MovesWithKingIsNotUnderCheck(othereFigures, this));
            }
            if (moves.Count == 0)
                MessageCheck(this, "Mate");
            else
                MessageCheck(this, " ");
        }

    }
}
