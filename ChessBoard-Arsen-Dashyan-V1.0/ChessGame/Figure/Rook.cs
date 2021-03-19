using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGame
{
    public class Rook : Model, ICrosswise, IAvailableMoves, IRandomeMove
    {
        public Rook(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
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
            arrayHor.Remove(this.point);
            return arrayHor;
        }
        public List<Point> AvailableMoves()
        {
            return this.Crosswise();
        }
        public bool IsUnderAttack(King king)
        {
            if (Math.Abs(this.point.X - king.point.X) +
                    Math.Abs(this.point.Y - king.point.Y) <= 2)
            {
                return true;
            }
            return false;
        }
        public bool IsProtected()
        {
            var model = Manager.models.Where(c => c != this).ToList();
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
            var model = Manager.models.Where(c => c != this).ToList();
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
            Point temp = this.point;
            Point tempForItem = null;
            foreach (var item in AvailableMoves())
            {
                this.point = item;
                if (IsProtected(item))
                {
                    if (Point.Modul(item, king.point) >= 2)
                    {
                        if (AvailableMoves().Contains(king.point))
                        {
                            tempForItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    if (Point.Modul(item, king.point) >= 2)
                    {
                        if (AvailableMoves().Contains(king.point))
                        {
                            tempForItem = item;
                            break;
                        }
                        else if (AvailableMoves().Count == 14)
                        {
                            tempForItem = item;
                            break;
                        }
                        else
                            tempForItem = item;
                    }
                }
            }
            this.point = temp;
            if (tempForItem == null)
            {
                foreach (var item in AvailableMoves())
                {
                    if (!IsUnderAttack(king))
                        tempForItem = item;
                }
            }
            return tempForItem;
        }
    }
}
