using System;
using System.Collections.Generic;
using Utility;
using static Utility.KnightMove;
using Coordinats;
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
