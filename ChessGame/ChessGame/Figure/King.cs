using Coordinats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    public class King : BaseFigure, ICrosswise, IDiagonal, IRandomMove, IDangerMoves
    {
        public King(string name, string color, List<BaseFigure> othereFigures) : base(othereFigures)
        {
            Name = name;
            Color = color;
        }
        #region Move
        public List<CoordinatPoint> Horizontal()
        {
            List<CoordinatPoint> arr = new List<CoordinatPoint>();
            List<CoordinatPoint> newArr = new List<CoordinatPoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.X - 1 >= 0)
            {
                arr.Add(new CoordinatPoint(this.Coordinate.X - 1, this.Coordinate.Y));
                arr.Add(this.Coordinate);
            }
            if (this.Coordinate.X + 1 <= 7)
            {
                arr.Add(this.Coordinate);
                arr.Add(new CoordinatPoint(this.Coordinate.X + 1, this.Coordinate.Y));
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
        public List<CoordinatPoint> Vertical()
        {
            List<CoordinatPoint> arr = new List<CoordinatPoint>();
            List<CoordinatPoint> newArr = new List<CoordinatPoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.Y - 1 >= 0)
            {
                arr.Add(new CoordinatPoint(this.Coordinate.X, this.Coordinate.Y - 1));
                arr.Add(this.Coordinate);
            }
            if (this.Coordinate.Y + 1 <= 7)
            {
                arr.Add(this.Coordinate);
                arr.Add(new CoordinatPoint(this.Coordinate.X, this.Coordinate.Y + 1));
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
        public List<CoordinatPoint> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public List<CoordinatPoint> RightIndex()
        {
            List<CoordinatPoint> arr = new List<CoordinatPoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.X + 1 <= 7 && this.Coordinate.Y - 1 >= 0)
            {
                arr.Add(new CoordinatPoint(this.Coordinate.X + 1, this.Coordinate.Y - 1));
            }
            if (this.Coordinate.X - 1 >= 0 && this.Coordinate.Y + 1 <= 7)
            {
                arr.Add(new CoordinatPoint(this.Coordinate.X - 1, this.Coordinate.Y + 1));
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
        public List<CoordinatPoint> LeftIndex()
        {
            List<CoordinatPoint> arr = new List<CoordinatPoint>();
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.Coordinate.X - 1 >= 0 && this.Coordinate.Y - 1 >= 0)
            {
                arr.Add(new CoordinatPoint(this.Coordinate.X - 1, this.Coordinate.Y - 1));
            }
            if (this.Coordinate.X + 1 <= 7 && this.Coordinate.Y + 1 <= 7)
            {
                arr.Add(new CoordinatPoint(this.Coordinate.X + 1, this.Coordinate.Y + 1));
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
        public List<CoordinatPoint> AvailableMoves()
        {
            var result = new List<CoordinatPoint>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            result.AddRange(Crosswise());
            result.Remove(this.Coordinate);
            return result;
        }
        public List<CoordinatPoint> DangerMoves()
        {
            return this.AvailableMoves();
        }
        #endregion

        public bool IsUnderAttack(CoordinatPoint CoordinatPoint)
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
        public bool IsProtected()
        {
            var model = othereFigures.Where(c => c.Color == this.Color && c != this).ToList();
            foreach (var item in model)
            {
                IAvailableMoves tempfigur = (IAvailableMoves)item;
                if (tempfigur.AvailableMoves().Contains(this.Coordinate))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsProtected(CoordinatPoint CoordinatPoint)
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
        public CoordinatPoint RandomMove(King king)
        {
            CoordinatPoint temp = null;
            if (ProtectedShax(king, out CoordinatPoint tempForItem))
            {
                temp = tempForItem;
                return temp;
            }
            else if (IsUnderAttackShax(king, out CoordinatPoint tempForItem2))
            {
                temp = tempForItem2;
                return temp;
            }
            else if (IsUnderAttackMax(king, out CoordinatPoint tempForItem3))
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
        private bool ProtectedShax(King king, out CoordinatPoint tempForItem)
        {
            CoordinatPoint temp = this.Coordinate;
            foreach (var item in AvailableMoves())
            {
                this.Coordinate = item;
                if (IsProtected(item))
                {
                    if (CoordinatPoint.Modul(item, king.Coordinate) >= 2d)
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
        private bool IsUnderAttackShax(King king, out CoordinatPoint tempForItem)
        {
            CoordinatPoint temp = this.Coordinate;
            foreach (var item in AvailableMoves())
            {
                this.Coordinate = item;
                if (!IsUnderAttack(this.Coordinate))
                {
                    if (CoordinatPoint.Modul(item, king.Coordinate) >= 2d)
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
        private bool IsUnderAttackMax(King king, out CoordinatPoint tempForItem)
        {
            CoordinatPoint temp = this.Coordinate;
            foreach (var item in AvailableMoves())
            {
                this.Coordinate = item;
                if (!IsUnderAttack(this.Coordinate))
                {
                    if (CoordinatPoint.Modul(item, king.Coordinate) >= 2d)
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
        public bool IsUnderAttack(CoordinatPoint CoordinatPoint, CoordinatPoint CoordinatPoint1)
        {
            return CoordinatPoint.Modul(CoordinatPoint1, CoordinatPoint) <= Math.Sqrt(2d);
        }
    }
}
