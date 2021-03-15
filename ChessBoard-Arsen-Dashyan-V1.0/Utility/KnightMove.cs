using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace Utility
{
    public static class KnightMove
    {
        public static List<Point> Horizontal(this Point point)
        {
            List<Point> result = new List<Point>();

            if (point.Y + 2 >= 1 && point.Y + 2 <= 8)
            {
                if (point.X - 1 >= 1 && point.X - 1 <= 8)
                    result.Add(new Point(point.X - 1, point.Y + 2));
                if (point.X + 1 <= 8 && point.X + 1 >= 1)
                    result.Add(new Point(point.X + 1, point.Y + 2));
            }
            if (point.Y - 2 <= 8 && point.Y - 2 >= 1)
            {
                if (point.X - 1 >= 1 && point.X - 1 <= 8)
                    result.Add(new Point(point.X - 1, point.Y - 2));
                if (point.X + 1 <= 8 && point.X + 1 >= 1)
                    result.Add(new Point(point.X + 1, point.Y - 2));
            }
            return result;
        }
        public static List<Point> Vertical(this Point point)
        {
            List<Point> result = new List<Point>();

            if (point.Y + 1 >= 1 && point.Y + 1 <= 8)
            {
                if (point.X - 2 >= 1 && point.X - 2 <= 8)
                    result.Add(new Point(point.X - 2, point.Y + 1));
                if (point.X + 2 <= 8 && point.X + 2 >= 1)
                    result.Add(new Point(point.X + 2, point.Y + 1));
            }
            if (point.Y - 1 <= 8 && point.Y - 1 >= 1)
            {
                if (point.X - 2 >= 1 && point.X - 2 <= 8)
                    result.Add(new Point(point.X - 2, point.Y - 1));
                if (point.X + 2 <= 8 && point.Y + 2 >= 1)
                    result.Add(new Point(point.X + 2, point.Y - 1));
            }
            return result;
        }
        public static List<Point> Crosswise(this Point point)
        {
            var arrayHor = Horizontal(point);
            var arrayVert = Vertical(point);
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public static bool Equals(this Point point, Point poi)
        {
            var knightMoves = Crosswise(point);
            var endMoves = Crosswise(poi);
            var result = endMoves.Intersect(knightMoves).ToList();
            return result.Count > 0;
        }
        public static bool Equals(this List<Point> arr, Point point)
        {
            List<Point> result = new List<Point>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item));
            }
            var endMoves = Crosswise(point);
            return endMoves.Intersect(result).ToList().Count > 0;
        }
        public static bool Equals(this List<Point> arr, List<Point> arrEnd)
        {
            List<Point> result = new List<Point>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item));
            }
            List<Point> resultEnd = new List<Point>();
            foreach (var item in arrEnd)
            {
                resultEnd.AddRange(Crosswise(item));
            }
            return resultEnd.Intersect(result).ToList().Count > 0;
        }
        public static bool EqualsEnd(this List<Point> arr, List<Point> arrEnd)
        {
            List<Point> result = new List<Point>();
            List<Point> resultStart = new List<Point>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item));
            }
            foreach (var item in result)
            {
                resultStart.AddRange(Crosswise(item));
            }
            List<Point> resultEnd = new List<Point> ();
            foreach (var item in arrEnd)
            {
                resultEnd.AddRange(Crosswise(item));
            }
            return resultEnd.Intersect(resultStart).ToList().Count > 0;
        }
    }
}
