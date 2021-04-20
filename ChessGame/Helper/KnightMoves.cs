using Figure;
using System.Collections.Generic;
using System.Linq;

namespace Helper
{
    public static class KnightMove
    {
        public static List<CoordinatePoint> Horizontal(this CoordinatePoint CoordinatPoint)
        {
            List<CoordinatePoint> result = new List<CoordinatePoint>();

            if (CoordinatPoint.Y + 2 >= 0 && CoordinatPoint.Y + 2 <= 7)
            {
                if (CoordinatPoint.X - 1 >= 0 && CoordinatPoint.X - 1 <= 7)
                    result.Add(new CoordinatePoint(CoordinatPoint.X - 1, CoordinatPoint.Y + 2));
                if (CoordinatPoint.X + 1 <= 7 && CoordinatPoint.X + 1 >= 0)
                    result.Add(new CoordinatePoint(CoordinatPoint.X + 1, CoordinatPoint.Y + 2));
            }
            if (CoordinatPoint.Y - 2 <= 7 && CoordinatPoint.Y - 2 >= 0)
            {
                if (CoordinatPoint.X - 1 >= 0 && CoordinatPoint.X - 1 <= 7)
                    result.Add(new CoordinatePoint(CoordinatPoint.X - 1, CoordinatPoint.Y - 2));
                if (CoordinatPoint.X + 1 <= 7 && CoordinatPoint.X + 1 >= 0)
                    result.Add(new CoordinatePoint(CoordinatPoint.X + 1, CoordinatPoint.Y - 2));
            }
            return result;
        }
        public static List<CoordinatePoint> Vertical(this CoordinatePoint CoordinatPoint)
        {
            List<CoordinatePoint> result = new List<CoordinatePoint>();

            if (CoordinatPoint.Y + 1 >= 0 && CoordinatPoint.Y + 1 <= 7)
            {
                if (CoordinatPoint.X - 2 >= 0 && CoordinatPoint.X - 2 <= 7)
                    result.Add(new CoordinatePoint(CoordinatPoint.X - 2, CoordinatPoint.Y + 1));
                if (CoordinatPoint.X + 2 <= 7 && CoordinatPoint.X + 2 >= 0)
                    result.Add(new CoordinatePoint(CoordinatPoint.X + 2, CoordinatPoint.Y + 1));
            }
            if (CoordinatPoint.Y - 1 <= 7 && CoordinatPoint.Y - 1 >= 0)
            {
                if (CoordinatPoint.X - 2 >= 0 && CoordinatPoint.X - 2 <= 7)
                    result.Add(new CoordinatePoint(CoordinatPoint.X - 2, CoordinatPoint.Y - 1));
                if (CoordinatPoint.X + 2 <= 7 && CoordinatPoint.Y + 2 >= 0)
                    result.Add(new CoordinatePoint(CoordinatPoint.X + 2, CoordinatPoint.Y - 1));
            }
            return result;
        }
        public static List<CoordinatePoint> Crosswise(this CoordinatePoint CoordinatPoint)
        {
            var arrayHor = Horizontal(CoordinatPoint);
            var arrayVert = Vertical(CoordinatPoint);
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public static bool Equals(this CoordinatePoint CoordinatPoint, CoordinatePoint poi)
        {
            var knightMoves = Crosswise(CoordinatPoint);
            var endMoves = Crosswise(poi);
            var result = endMoves.Intersect(knightMoves).ToList();
            return result.Count > 0;
        }
        public static bool Equals(this List<CoordinatePoint> arr, CoordinatePoint CoordinatPoint)
        {
            List<CoordinatePoint> result = new List<CoordinatePoint>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item));
            }
            var endMoves = Crosswise(CoordinatPoint);
            return endMoves.Intersect(result).ToList().Count > 0;
        }
        public static bool Equals(this List<CoordinatePoint> arr, List<CoordinatePoint> arrEnd)
        {
            List<CoordinatePoint> result = new List<CoordinatePoint>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item));
            }
            List<CoordinatePoint> resultEnd = new List<CoordinatePoint>();
            foreach (var item in arrEnd)
            {
                resultEnd.AddRange(Crosswise(item));
            }
            return resultEnd.Intersect(result).ToList().Count > 0;
        }
        public static bool EqualsEnd(this List<CoordinatePoint> arr, List<CoordinatePoint> arrEnd)
        {
            List<CoordinatePoint> result = new List<CoordinatePoint>();
            List<CoordinatePoint> resultStart = new List<CoordinatePoint>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item));
            }
            foreach (var item in result)
            {
                resultStart.AddRange(Crosswise(item));
            }
            List<CoordinatePoint> resultEnd = new List<CoordinatePoint>();
            foreach (var item in arrEnd)
            {
                resultEnd.AddRange(Crosswise(item));
            }
            return resultEnd.Intersect(resultStart).ToList().Count > 0;
        }
    }
}
