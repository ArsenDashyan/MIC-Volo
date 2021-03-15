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
    }
}
