using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGame
{
    public class King : Model, ICrosswise, IDiagonal
    {
        public King(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
        public bool IsMove(Point point) => (Math.Abs(this.point.X - point.X) <= 1 && Math.Abs(this.point.Y - point.Y) <= 1);
        public List<Point> Horizontal()
        {
            List<Point> arr = new List<Point>();
            if (this.point.X - 1 <= 8 && this.point.X - 1 >= 1)
            {
                arr.Add(new Point(this.point.X - 1, this.point.Y));
            }
            if (this.point.X + 1 <= 8 && this.point.X + 1 >= 1)
            {
                arr.Add(new Point(this.point.X + 1, this.point.Y));
            }
            return arr;
        }
        public List<Point> Vertical()
        {
            List<Point> arr = new List<Point>();
            if (this.point.Y - 1 <= 8 && this.point.Y - 1 >= 1)
            {
                arr.Add(new Point(this.point.X, this.point.Y - 1));
            }
            if (this.point.Y + 1 <= 8 && this.point.Y + 1 >= 1)
            {
                arr.Add(new Point(this.point.X, this.point.Y + 1));
            }
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
            if (this.point.X + 1 <= 8 && this.point.Y - 1 >= 1)
            {
                arr.Add(new Point(this.point.X + 1, this.point.Y - 1));
            }
            if (this.point.X - 1 >= 1 && this.point.Y + 1 <= 8)
            {
                arr.Add(new Point(this.point.X - 1, this.point.Y + 1));
            }
            return arr;
        }
        public List<Point> LeftIndex()
        {
            List<Point> arr = new List<Point>();
            if (this.point.X - 1 >= 1 && this.point.Y - 1 >= 1)
            {
                arr.Add(new Point(this.point.X - 1, this.point.Y - 1));
            }
            if (this.point.X + 1 <= 8 && this.point.Y + 1 <= 8)
            {
                arr.Add(new Point(this.point.X + 1, this.point.Y + 1));
            }
            return arr;
        }
        public List<Point> AvailableMoves()
        {
            var result = new List<Point>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            result.AddRange(Crosswise());
            result = result.Where(c => c != this.point && !Manager.DangerousPosition().Contains(c)).ToList();
            return result;
        }
    }
}
