﻿using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGame
{
    public class Rook : Model, ICrosswise, IRandomeMove
    {
        public Rook(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<Point> Vertical()
        {
            List<Point> arr = new List<Point>();
            var model = Manager.models.Where(c => c != this).ToList();
            for (int i = 1; i <= 8; i++)
            {
                Point pointTemp = new Point(this.point.X, i);
                arr.Add(pointTemp);
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.point))
                {
                    if (item.Color == this.Color)
                    {
                        if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.point)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.point)).ToList();
                    }
                    else
                    {
                        if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.point)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.point)).ToList();
                    }
                }
            }
            arr.Remove(this.point);
            return arr;
        }
        public List<Point> Horizontal()
        {
            List<Point> arr = new List<Point>();
            var model = Manager.models.Where(c => c != this).ToList();
            for (int i = 1; i <= 8; i++)
            {
                Point pointTemp = new Point(i, this.point.Y);
                arr.Add(pointTemp);
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.point))
                {
                    if (item.Color == this.Color)
                    {
                        if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.point)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.point)).ToList();
                    }
                    else
                    {
                        if (arr.IndexOf(this.point) < arr.IndexOf(item.point))
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.point)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.point)).ToList();
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
            arrayHor.Remove(this.point);
            return arrayHor;
        }
        public List<Point> AvailableMoves()
        {
            return this.Crosswise();
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
        public bool IsUnderAttack(Point point, Point point1)
        {
            if (Point.Modul(point1, point) < 2d)
            {
                return true;
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
                    return true;
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
                    return true;
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
                    if (Point.Modul(item, king.point) > 3d)
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
    }
}
