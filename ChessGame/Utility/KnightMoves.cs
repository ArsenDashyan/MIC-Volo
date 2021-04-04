using Coordinats;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class KnightMove
    {
        public static List<CoordinatPoint> Horizontal(this CoordinatPoint CoordinatPoint)
        {
            List<CoordinatPoint> result = new List<CoordinatPoint>();

            if (CoordinatPoint.Y + 2 >= 0 && CoordinatPoint.Y + 2 <= 7)
            {
                if (CoordinatPoint.X - 1 >= 0 && CoordinatPoint.X - 1 <= 7)
                    result.Add(new CoordinatPoint(CoordinatPoint.X - 1, CoordinatPoint.Y + 2));
                if (CoordinatPoint.X + 1 <= 7 && CoordinatPoint.X + 1 >= 0)
                    result.Add(new CoordinatPoint(CoordinatPoint.X + 1, CoordinatPoint.Y + 2));
            }
            if (CoordinatPoint.Y - 2 <= 7 && CoordinatPoint.Y - 2 >= 0)
            {
                if (CoordinatPoint.X - 1 >= 0 && CoordinatPoint.X - 1 <= 7)
                    result.Add(new CoordinatPoint(CoordinatPoint.X - 1, CoordinatPoint.Y - 2));
                if (CoordinatPoint.X + 1 <= 7 && CoordinatPoint.X + 1 >= 0)
                    result.Add(new CoordinatPoint(CoordinatPoint.X + 1, CoordinatPoint.Y - 2));
            }
            return result;
        }
        public static List<CoordinatPoint> Vertical(this CoordinatPoint CoordinatPoint)
        {
            List<CoordinatPoint> result = new List<CoordinatPoint>();

            if (CoordinatPoint.Y + 1 >= 0 && CoordinatPoint.Y + 1 <= 7)
            {
                if (CoordinatPoint.X - 2 >= 0 && CoordinatPoint.X - 2 <= 7)
                    result.Add(new CoordinatPoint(CoordinatPoint.X - 2, CoordinatPoint.Y + 1));
                if (CoordinatPoint.X + 2 <= 7 && CoordinatPoint.X + 2 >= 0)
                    result.Add(new CoordinatPoint(CoordinatPoint.X + 2, CoordinatPoint.Y + 1));
            }
            if (CoordinatPoint.Y - 1 <= 7 && CoordinatPoint.Y - 1 >= 0)
            {
                if (CoordinatPoint.X - 2 >= 0 && CoordinatPoint.X - 2 <= 7)
                    result.Add(new CoordinatPoint(CoordinatPoint.X - 2, CoordinatPoint.Y - 1));
                if (CoordinatPoint.X + 2 <= 7 && CoordinatPoint.Y + 2 >= 0)
                    result.Add(new CoordinatPoint(CoordinatPoint.X + 2, CoordinatPoint.Y - 1));
            }
            return result;
        }
        public static List<CoordinatPoint> Crosswise(this CoordinatPoint CoordinatPoint)
        {
            var arrayHor = Horizontal(CoordinatPoint);
            var arrayVert = Vertical(CoordinatPoint);
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public static bool Equals(this CoordinatPoint CoordinatPoint, CoordinatPoint poi)
        {
            var knightMoves = Crosswise(CoordinatPoint);
            var endMoves = Crosswise(poi);
            var result = endMoves.Intersect(knightMoves).ToList();
            return result.Count > 0;
        }
        public static bool Equals(this List<CoordinatPoint> arr, CoordinatPoint CoordinatPoint)
        {
            List<CoordinatPoint> result = new List<CoordinatPoint>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item));
            }
            var endMoves = Crosswise(CoordinatPoint);
            return endMoves.Intersect(result).ToList().Count > 0;
        }
        public static bool Equals(this List<CoordinatPoint> arr, List<CoordinatPoint> arrEnd)
        {
            List<CoordinatPoint> result = new List<CoordinatPoint>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item));
            }
            List<CoordinatPoint> resultEnd = new List<CoordinatPoint>();
            foreach (var item in arrEnd)
            {
                resultEnd.AddRange(Crosswise(item));
            }
            return resultEnd.Intersect(result).ToList().Count > 0;
        }
        public static bool EqualsEnd(this List<CoordinatPoint> arr, List<CoordinatPoint> arrEnd)
        {
            List<CoordinatPoint> result = new List<CoordinatPoint>();
            List<CoordinatPoint> resultStart = new List<CoordinatPoint>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item));
            }
            foreach (var item in result)
            {
                resultStart.AddRange(Crosswise(item));
            }
            List<CoordinatPoint> resultEnd = new List<CoordinatPoint>();
            foreach (var item in arrEnd)
            {
                resultEnd.AddRange(Crosswise(item));
            }
            return resultEnd.Intersect(resultStart).ToList().Count > 0;
        }
    }
}
