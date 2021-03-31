using System;

namespace Coordinats
{
    public class CoordinatPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public CoordinatPoint(int x, int y) => (X, Y) = (x, y);
        public override bool Equals(object obj)
        {
            if (!(obj is CoordinatPoint))
                return false;
            else
            {
                CoordinatPoint temp = (CoordinatPoint)obj;
                return this.X == temp.X && this.Y == temp.Y;
            }
        }
        public override int GetHashCode()
        {
            return X ^ Y;
        }
        public override string ToString()
        {
            return $"{IntToChar(X)}{Y+1}";
        }
        private  char IntToChar(int number)
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
        public static bool operator ==(CoordinatPoint? p1, CoordinatPoint? p2)
        {
            return p1?.X == p2?.X && p1?.Y == p2?.Y;
        }
        public static bool operator !=(CoordinatPoint? p1, CoordinatPoint? p2)
        {
            return !(p1?.X == p2?.X && p1?.Y == p2?.Y);
        }

        public static double operator -(CoordinatPoint CoordinatPoint, CoordinatPoint CoordinatPoint1)
        {
            return Math.Sqrt((CoordinatPoint1.X - CoordinatPoint.X) * (CoordinatPoint1.X - CoordinatPoint.X) +
                            (CoordinatPoint1.Y - CoordinatPoint.Y) * (CoordinatPoint1.Y - CoordinatPoint.Y));
        }
        public static double Modul(CoordinatPoint CoordinatPoint, CoordinatPoint CoordinatPoint1)
        {
            return Math.Sqrt((CoordinatPoint1.X - CoordinatPoint.X) * (CoordinatPoint1.X - CoordinatPoint.X) +
                             (CoordinatPoint1.Y - CoordinatPoint.Y) * (CoordinatPoint1.Y - CoordinatPoint.Y));
        }
    }
}
