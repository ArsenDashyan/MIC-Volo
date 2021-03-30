using System;

namespace Coordinats
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y) => (X, Y) = (x, y);
        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;
            else
            {
                Point temp = (Point)obj;
                return this.X == temp.X && this.Y == temp.Y;
            }
        }
        public override int GetHashCode()
        {
            return X ^ Y;
        }

#nullable enable
        public static bool operator ==(Point? p1, Point? p2)
        {
            return p1?.X == p2?.X && p1?.Y == p2?.Y;
        }
        public static bool operator !=(Point? p1, Point? p2)
        {
            return !(p1?.X == p2?.X && p1?.Y == p2?.Y);
        }
        public static double Modul(Point point, Point point1)
        {
            return Math.Sqrt((point1.X - point.X) * (point1.X - point.X) +
                            (point1.Y - point.Y) * (point1.Y - point.Y));
        }
    }
}
