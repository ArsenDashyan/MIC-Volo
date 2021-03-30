using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGame
{
   public class Bishop : Model, IDiagonal, IRandomeMove
    {
        public Bishop(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<Point> RightIndex()
        {
            List<Point> arr = new List<Point>();
            var model = Manager.models.Where(c => c != this).ToList();
            int sum = this.point.X + this.point.Y;
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (i + j == sum)
                    {
                        Point pointTemp = new Point(i, j);
                        arr.Add(pointTemp);
                    }
                }
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.point))
                {
                    if (item.Color == this.Color)
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
                    else
                    {
                        if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                        {
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.point)).ToList();
                        }
                        else
                        {
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.point)).ToList();
                        }
                    }
                }
            }
            arr.Remove(this.point);
            return arr;
        }
        public List<Point> LeftIndex()
        {
            List<Point> arr = new List<Point>();
            int sub = this.point.X - this.point.Y;
            var model = Manager.models.Where(c => c != this).ToList();

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (i - j == sub)
                    {
                        Point pointTemp = new Point(i, j);
                        arr.Add(pointTemp);
                    }
                }
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.point))
                {
                    if (item.Color ==this.Color)
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
                    else
                    {
                        if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                        {
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.point)).ToList();
                        }
                        else
                        {
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.point)).ToList();
                        }
                    }
                }
            }
            arr.Remove(this.point);
            return arr;
        }
        public List<Point> AvailableMoves()
        {
            var result = new List<Point>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
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
            var model = Manager.models.Where(c => c != this && c.Color == this.Color).ToList();
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
            var model = Manager.models.Where(c => c != this && c.Color == this.Color).ToList();
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
