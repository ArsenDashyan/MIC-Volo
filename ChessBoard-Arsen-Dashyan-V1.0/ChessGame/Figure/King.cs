﻿using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGame
{
    public class King : Model, ICrosswise, IDiagonal, IAvailableMoves, IRandomeMove
    {
        public King(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public bool IsMove(Point point)
        {
            if (this.point == null)
            {
                return true;
            }
            return (Math.Abs((int)this.point?.X - point.X) <= 1 && Math.Abs((int)this.point?.Y - point.Y) <= 1);
        }
        public List<Point> Horizontal()
        {
            List<Point> arr = new List<Point>();
            var model = Manager.models.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.point.X - 1 <= 8 && this.point.X - 1 >= 1)
            {
                arr.Add(new Point(this.point.X - 1, this.point.Y));
            }
            if (this.point.X + 1 <= 8 && this.point.X + 1 >= 1)
            {
                arr.Add(new Point(this.point.X + 1, this.point.Y));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.point))
                {
                    if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                    {
                        arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.point)).ToList();
                    }
                    else
                    {
                        arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.point)).ToList();
                    }
                }
            }
            arr.Remove(this.point);
            return arr;
        }
        public List<Point> Vertical()
        {
            List<Point> arr = new List<Point>();
            var model = Manager.models.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.point.Y - 1 <= 8 && this.point.Y - 1 >= 1)
            {
                arr.Add(new Point(this.point.X, this.point.Y - 1));
            }
            if (this.point.Y + 1 <= 8 && this.point.Y + 1 >= 1)
            {
                arr.Add(new Point(this.point.X, this.point.Y + 1));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.point))
                {
                    if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                    {
                        arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.point)).ToList();
                    }
                    else
                    {
                        arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.point)).ToList();
                    }
                }
            }
            arr.Remove(this.point);
            return arr;
        }
        public List<Point> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public List<Point> RightIndex()
        {
            List<Point> arr = new List<Point>();
            var model = Manager.models.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.point.X + 1 <= 8 && this.point.Y - 1 >= 1)
            {
                arr.Add(new Point(this.point.X + 1, this.point.Y - 1));
            }
            if (this.point.X - 1 >= 1 && this.point.Y + 1 <= 8)
            {
                arr.Add(new Point(this.point.X - 1, this.point.Y + 1));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.point))
                {
                    if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                    {
                        arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.point)).ToList();
                    }
                    else
                    {
                        arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.point)).ToList();
                    }
                }
            }
            arr.Remove(this.point);
            return arr;
        }
        public List<Point> LeftIndex()
        {
            List<Point> arr = new List<Point>();
            var model = Manager.models.Where(c => c.Color == this.Color && c != this).ToList();
            if (this.point.X - 1 >= 1 && this.point.Y - 1 >= 1)
            {
                arr.Add(new Point(this.point.X - 1, this.point.Y - 1));
            }
            if (this.point.X + 1 <= 8 && this.point.Y + 1 <= 8)
            {
                arr.Add(new Point(this.point.X + 1, this.point.Y + 1));
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.point))
                {
                    if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                    {
                        arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.point)).ToList();
                    }
                    else
                    {
                        arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.point)).ToList();
                    }
                }
            }
            arr.Remove(this.point);
            return arr;
        }
        public List<Point> AvailableMovesWithShaxAndBox()
        {
            var result = new List<Point>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            result.AddRange(Crosswise());
            result.Remove(this.point);
            result = result.Where(c=>!Manager.DangerPosition(this).Contains(c)).ToList();
            return result;
        }
        public List<Point> AvailableMoves()
        {
            var result = new List<Point>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            result.AddRange(Crosswise());
            result.Remove(this.point);
            return result;
        }
        #endregion

        public bool IsUnderAttack(Point point)
        {
            var modelNew = Manager.models.Where(c => c.Color == ConsoleColor.Red).ToList();
            foreach (var item in modelNew)
            {
                IAvailableMoves itemFigur = (IAvailableMoves)item;
                if (itemFigur.AvailableMoves().Contains(point))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsProtected()
        {
            var model = Manager.models.Where(c => c.Color == this.Color && c != this).ToList();
            foreach (var item in model)
            {
                IAvailableMoves tempfigur = (IAvailableMoves)item;
                if (tempfigur.AvailableMoves().Contains(this.point))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsProtected(Point point)
        {
            var model = Manager.models.Where(c => c.Color == this.Color && c != this).ToList();
            foreach (var item in model)
            {
                IAvailableMoves tempfigur = (IAvailableMoves)item;
                if (tempfigur.AvailableMoves().Contains(point))
                {
                    return true;
                }
            }
            return false;
        }
        public Point RandomMove(King king)
        {
            Point temp = null;
            if (ProtectedShax(king, out Point tempForItem))
            {
                temp = tempForItem;
                return temp;
            }
            else if (IsUnderAttackShax(king, out Point tempForItem2))
            {
                temp = tempForItem2;
                return temp;
            }
            else if (IsUnderAttackMax(king, out Point tempForItem3))
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
        private bool ProtectedShax(King king, out Point tempForItem)
        {
            Point temp = this.point;
            foreach (var item in AvailableMoves())
            {
                this.point = item;
                if (IsProtected(item))
                {
                    if (Point.Modul(item, king.point) >= 2d)
                    {
                        if (AvailableMoves().Contains(king.point))
                        {
                            tempForItem = item;
                            this.point = temp;
                            return true;
                        }
                    }
                }
            }
            this.point = temp;
            tempForItem = null;
            return false;
        }
        private bool IsUnderAttackShax(King king, out Point tempForItem)
        {
            Point temp = this.point;
            foreach (var item in AvailableMoves())
            {
                this.point = item;
                if (!IsUnderAttack(this.point))
                {
                    if (Point.Modul(item, king.point) >= 2d)
                    {
                        if (AvailableMoves().Contains(king.point))
                        {
                            tempForItem = item;
                            this.point = temp;
                            return true;
                        }
                    }
                }
            }
            this.point = temp;
            tempForItem = null;
            return false;
        }
        private bool IsUnderAttackMax(King king, out Point tempForItem)
        {
            Point temp = this.point;
            foreach (var item in AvailableMoves())
            {
                this.point = item;
                if (!IsUnderAttack(this.point))
                {
                    if (Point.Modul(item, king.point) >= 2d)
                    {
                        if (AvailableMoves().Count == 14)
                        {
                            tempForItem = item;
                            this.point = temp;
                            return true;
                        }
                    }
                }
            }
            this.point = temp;
            tempForItem = null;
            return false;
        }
        public bool IsUnderAttack(Point point, Point point1)
        {
            if (Point.Modul(point1, point) < 2d)
            {
                return true;
            }
            return false;
        }
    }
}
