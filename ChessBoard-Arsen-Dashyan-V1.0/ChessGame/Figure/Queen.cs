using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGame
{
    public class Queen : Model, IDiagonal, ICrosswise
    {
        public Queen(string name, ConsoleColor color)
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
            return arrayHor;
        }
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
                        Point pointTemp = new Point(i,j);
                        arr.Add(pointTemp);
                    }
                }
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
        public List<Point> AvailableMoves()
        {
            var result = new List<Point>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            result.AddRange(Crosswise());
            result.Remove(this.point);
            return result;
        }
        public bool IsUnderAttack(King king)
        {
            if (Math.Sqrt((this.point.X - king.point.X) * (this.point.X - king.point.X) +
                    (this.point.Y - king.point.Y) * (this.point.Y - king.point.Y)) <= 2)
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
                if (Math.Sqrt((this.point.X - item.point.X) * (this.point.X - item.point.X) +
                    (this.point.Y - item.point.Y) * (this.point.Y - item.point.Y)) <= 2)
                {
                    return true;
                }
            }
            foreach (var item in model)
            {
                if (item is Rook rook)
                {
                    if (AvailableMoves().Contains(rook.point))
                        return true;
                }
            }
            return false;
        }
        public bool IsProtected(Point point)
        {
            var model = Manager.models.Where(c => c != this).ToList();
            foreach (var item in model)
            {
                if (Math.Sqrt((point.X - item.point.X) * (point.X - item.point.X) +
                    (point.Y - item.point.Y) * (point.Y - item.point.Y)) <= 2)
                {
                    return true;
                }
            }
            foreach (var item in model)
            {
                if (item is Rook rook)
                {
                    if (rook.point.X == point.X)
                        return true;
                    if (rook.point.Y == point.Y)
                        return true;
                }
            }
            return false;
        }
        public Point RandomMove()
        {
            foreach (var item in AvailableMoves())
            {
                if (IsProtected(item))
                {
                    return item;
                }
            }
            int rnd = (new Random().Next(0, this.AvailableMoves().Count));
            return AvailableMoves()[rnd];
        }
    }
}
