using System;
using System.Collections.Generic;
using Utility;
using static Utility.KnightMove;
using Coordinats;
using System.Linq;

namespace ChessGame
{
    public class Knights : Model, ICrosswise
    {
        static int count = 1;
        public Knights(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
        public List<Point> Horizontal()
        {
            List<Point> result = new List<Point>();

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
            return result;
        }
        public List<Point> Vertical()
        {
            List<Point> result = new List<Point>();

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
                if (item is Queen queen)
                {
                    if (AvailableMoves().Contains(queen.point))
                        return true;
                }
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
                if (Math.Sqrt((this.point.X - item.point.X) * (this.point.X - item.point.X) +
                    (this.point.Y - item.point.Y) * (this.point.Y - item.point.Y)) <= 2)
                {
                    return true;
                }
            }
            foreach (var item in model)
            {
                if (item is Queen queen)
                {
                    if (queen.point.X == point.X)
                        return true;
                    if (queen.point.Y == point.Y)
                        return true;
                }
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
        public Point RandomMove(King king)
        {
            Point temp = this.point;
            Point tempForItem = null;
            foreach (var item in AvailableMoves())
            {
                this.point = item;
                if (IsProtected(item))
                {
                    if (Point.Modul(item, king.point) > 2)
                    {
                        if (AvailableMoves().Contains(king.point))
                        {
                            tempForItem = item;
                        }
                    }
                    else
                    {
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
                    {
                        tempForItem = item;
                    }
                }
            }
            return tempForItem;
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
    }
}
