using System;

namespace Figure
{
    public class CoordinatePoint: EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public CoordinatePoint(int x, int y) => (X, Y) = (x, y);
        public override bool Equals(object obj)
        {
            if (!(obj is CoordinatePoint))
                return false;
            else
            {
                CoordinatePoint temp = (CoordinatePoint)obj;
                return this.X == temp.X && this.Y == temp.Y;
            }
        }
        public override int GetHashCode()
        {
            return X ^ Y;
        }
        public override string ToString()
        {
            return $"{IntToChar(X)}.{Y + 1}";
        }
        private char IntToChar(int number)
        {
            char ch = number switch
            {
                0 => 'a',
                1 => 'b',
                2 => 'c',
                3 => 'd',
                4 => 'e',
                5 => 'f',
                6 => 'g',
                7 => 'h',
                _ => 'x'
            };
            return ch;
        }
#nullable enable
        public static bool operator ==(CoordinatePoint? p1, CoordinatePoint? p2)
        {
            return p1?.X == p2?.X && p1?.Y == p2?.Y;
        }
        public static bool operator !=(CoordinatePoint? p1, CoordinatePoint? p2)
        {
            return !(p1?.X == p2?.X && p1?.Y == p2?.Y);
        }

        public static double operator -(CoordinatePoint CoordinatPoint, CoordinatePoint CoordinatPoint1)
        {
            return Math.Sqrt((CoordinatPoint1.X - CoordinatPoint.X) * (CoordinatPoint1.X - CoordinatPoint.X) +
                            (CoordinatPoint1.Y - CoordinatPoint.Y) * (CoordinatPoint1.Y - CoordinatPoint.Y));
        }
        public static double Modul(CoordinatePoint CoordinatPoint, CoordinatePoint CoordinatPoint1)
        {
            return Math.Sqrt((CoordinatPoint1.X - CoordinatPoint.X) * (CoordinatPoint1.X - CoordinatPoint.X) +
                             (CoordinatPoint1.Y - CoordinatPoint.Y) * (CoordinatPoint1.Y - CoordinatPoint.Y));
        }
    }
}
