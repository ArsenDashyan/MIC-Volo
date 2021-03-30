﻿using System;
using System.Collections.Generic;
using Utility;
using static Utility.KnightMove;
using Coordinats;
using System.Linq;

namespace ChessGame
{
    public class Knights : Model, ICrosswise, IRandomeMove
    {
        static int count = 1;
        public Knights(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<Point> Horizontal()
        {
            List<Point> result = new List<Point>();
            var model = Manager.models.Where(c => c != this).ToList();
            if (this.point.Y + 2 >= 1 && this.point.Y + 2 <= 8)
            {
                if (this.point.X - 1 >= 1 && this.point.X - 1 <= 8)
                    result.Add(new Point(this.point.X - 1, this.point.Y + 2));
                if (this.point.X + 1 <= 8 && this.point.X + 1 >= 1)
                    result.Add(new Point(this.point.X + 1, this.point.Y + 2));
            }
            if (this.point.Y - 2 <= 8 && this.point.Y - 2 >= 1)
            {
                if (this.point.X - 1 >= 1 && this.point.X - 1 <= 8)
                    result.Add(new Point(this.point.X - 1, this.point.Y - 2));
                if (this.point.X + 1 <= 8 && this.point.X + 1 >= 1)
                    result.Add(new Point(this.point.X + 1, this.point.Y - 2));
            }
            foreach (var item in model)
            {
                if (result.Contains(item.point))
                {
                    if (item.Color == this.Color)
                    {
                        if (result.IndexOf(this.point) < result.IndexOf(item.point))
                            result = result.Where(c => result.IndexOf(c) < result.IndexOf(item.point)).ToList();
                        else
                            result = result.Where(c => result.IndexOf(c) > result.IndexOf(item.point)).ToList();
                    }
                    else
                    {
                        if (result.IndexOf(this.point) < result.IndexOf(item.point))
                            result = result.Where(c => result.IndexOf(c) <= result.IndexOf(item.point)).ToList();
                        else
                            result = result.Where(c => result.IndexOf(c) >= result.IndexOf(item.point)).ToList();
                    }
                }
            }
            return result;
        }
        public List<Point> Vertical()
        {
            List<Point> result = new List<Point>();
            var model = Manager.models.Where(c => c != this).ToList();
            if (this.point.Y + 1 >= 1 && this.point.Y + 1 <= 8)
            {
                if (this.point.X - 2 >= 1 && this.point.X - 2 <= 8)
                    result.Add(new Point(this.point.X - 2, this.point.Y + 1));
                if (this.point.X + 2 <= 8 && this.point.X + 2 >= 1)
                    result.Add(new Point(this.point.X + 2, this.point.Y + 1));
            }
            if (this.point.Y - 1 <= 8 && this.point.Y - 1 >= 1)
            {
                if (this.point.X - 2 >= 1 && this.point.X - 2 <= 8)
                    result.Add(new Point(this.point.X - 2, this.point.Y - 1));
                if (this.point.X + 2 <= 8 && this.point.X + 2 >= 1)
                    result.Add(new Point(this.point.X + 2, this.point.Y - 1));
            }
            foreach (var item in model)
            {
                if (result.Contains(item.point))
                {
                    if (item.Color == this.Color)
                    {
                        if (result.IndexOf(this.point) < result.IndexOf(item.point))
                            result = result.Where(c => result.IndexOf(c) < result.IndexOf(item.point)).ToList();
                        else
                            result = result.Where(c => result.IndexOf(c) > result.IndexOf(item.point)).ToList();
                    }
                    else
                    {
                        if (result.IndexOf(this.point) < result.IndexOf(item.point))
                            result = result.Where(c => result.IndexOf(c) <= result.IndexOf(item.point)).ToList();
                        else
                            result = result.Where(c => result.IndexOf(c) >= result.IndexOf(item.point)).ToList();
                    }
                }
            }
            return result;
        }
        public List<Point> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
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
        public int MinCount(Point point)
        {
            var knightMoves = KnightMove.Crosswise(this.point);
            var endMoves = KnightMove.Crosswise(point);
            if (knightMoves.Contains((point)))
            {
                return count;
            }
            else
            {
                count++;
                if (KnightMove.Equals(this.point,point))
                {
                    return count;
                }
                else
                {
                    count++;
                    if (KnightMove.Equals(knightMoves, point))
                    {
                        return count;
                    }
                    else
                    {
                        count++;
                        if (KnightMove.Equals(knightMoves, endMoves))
                        {
                            return count;
                        }
                        else
                        {
                            count++;
                            if (KnightMove.EqualsEnd(knightMoves, endMoves))
                            {
                                return count;
                            }
                            else
                            {
                                return 6;
                            }
                        }
                    }
                }
            }
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
