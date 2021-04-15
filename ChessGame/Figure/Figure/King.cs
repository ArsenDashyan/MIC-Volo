﻿using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class King : BaseFigure, ICrosswise, IDiagonal, IRandomMove, IDangerMoves
    {
        public King(string name, string color, List<BaseFigure> othereFigures) : base(othereFigures)
        {
            Name = name;
            Color = color;
        }
        #region Move
        public List<CoordinatePoint> Horizontal()
        {
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
            List<CoordinatePoint> newArr = new List<CoordinatePoint>();
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
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
            List<CoordinatePoint> newArr = new List<CoordinatePoint>();
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
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
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
            List<CoordinatePoint> arr = new List<CoordinatePoint>();
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
        public List<CoordinatePoint> DangerMoves()
        {
            return this.AvailableMoves();
        }
        #endregion

        public bool IsUnderAttack(CoordinatePoint CoordinatPoint)
        {
            var modelNew = othereFigures.Where(c => c.Color != this.Color).ToList();
            foreach (var item in modelNew)
            {
                IAvailableMoves itemFigur = (IAvailableMoves)item;
                if (itemFigur.AvailableMoves().Contains(CoordinatPoint))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsProtected(CoordinatePoint CoordinatPoint)
        {
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            foreach (var item in model)
            {
                IAvailableMoves tempfigur = (IAvailableMoves)item;
                if (tempfigur.AvailableMoves().Contains(CoordinatPoint))
                {
                    return true;
                }
            }
            return false;
        }
        public CoordinatePoint RandomMove(King king)
        {
            CoordinatePoint temp = null;
            if (ProtectedShax(king, out CoordinatePoint tempForItem))
            {
                temp = tempForItem;
                return temp;
            }
            else if (IsUnderAttackShax(king, out CoordinatePoint tempForItem2))
            {
                temp = tempForItem2;
                return temp;
            }
            else if (IsUnderAttackMax(king, out CoordinatePoint tempForItem3))
            {
                temp = tempForItem3;
                return temp;
            }
            if (temp == null)
            {
                foreach (var item in AvailableMoves())
                {
                    if (!IsUnderAttack(item))
                    {
                        temp = item;
                        break;
                    }
                }
            }
            return temp;
        }
        private bool ProtectedShax(King king, out CoordinatePoint tempForItem)
        {
            CoordinatePoint temp = this.Coordinate;
            foreach (var item in AvailableMoves())
            {
                this.Coordinate = item;
                if (IsProtected(item))
                {
                    if (CoordinatePoint.Modul(item, king.Coordinate) >= 2d)
                    {
                        if (AvailableMoves().Contains(king.Coordinate))
                        {
                            tempForItem = item;
                            this.Coordinate = temp;
                            return true;
                        }
                    }
                }
            }
            this.Coordinate = temp;
            tempForItem = null;
            return false;
        }
        private bool IsUnderAttackShax(King king, out CoordinatePoint tempForItem)
        {
            CoordinatePoint temp = this.Coordinate;
            foreach (var item in AvailableMoves())
            {
                this.Coordinate = item;
                if (!IsUnderAttack(this.Coordinate))
                {
                    if (CoordinatePoint.Modul(item, king.Coordinate) >= 2d)
                    {
                        if (AvailableMoves().Contains(king.Coordinate))
                        {
                            tempForItem = item;
                            this.Coordinate = temp;
                            return true;
                        }
                    }
                }
            }
            this.Coordinate = temp;
            tempForItem = null;
            return false;
        }
        private bool IsUnderAttackMax(King king, out CoordinatePoint tempForItem)
        {
            CoordinatePoint temp = this.Coordinate;
            foreach (var item in AvailableMoves())
            {
                this.Coordinate = item;
                if (!IsUnderAttack(this.Coordinate))
                {
                    if (CoordinatePoint.Modul(item, king.Coordinate) >= 2d)
                    {
                        if (AvailableMoves().Count == 14)
                        {
                            tempForItem = item;
                            this.Coordinate = temp;
                            return true;
                        }
                    }
                }
            }
            this.Coordinate = temp;
            tempForItem = null;
            return false;
        }
    }
}
