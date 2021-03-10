using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class KnightMove
    {
        public static List<(int, int)> Horizontal(this int a, int b)
        {
            List<(int, int)> result = new List<(int, int)>();

            if (b + 2 >= 1 && b + 2 <= 8)
            {
                if (a - 1 >= 1 && a - 1 <= 8)
                    result.Add((a - 1, b + 2));
                if (a + 1 <= 8 && a + 1 >= 1)
                    result.Add((a + 1, b + 2));
            }
            if (b - 2 <= 8 && b - 2 >= 1)
            {
                if (a - 1 >= 1 && a - 1 <= 8)
                    result.Add((a - 1, b - 2));
                if (a + 1 <= 8 && a + 1 >= 1)
                    result.Add((a + 1, b - 2));
            }
            return result;
        }
        public static List<(int, int)> Vertical(this int a, int b)
        {
            List<(int, int)> result = new List<(int, int)>();

            if (b + 1 >= 1 && b + 1 <= 8)
            {
                if (a - 2 >= 1 && a - 2 <= 8)
                    result.Add((a - 2, b + 1));
                if (a + 2 <= 8 && a + 2 >= 1)
                    result.Add((a + 2, b + 1));
            }
            if (b - 1 <= 8 && b - 1 >= 1)
            {
                if (a - 2 >= 1 && a - 2 <= 8)
                    result.Add((a - 2, b - 1));
                if (a + 2 <= 8 && b + 2 >= 1)
                    result.Add((a + 2, b - 1));
            }
            return result;
        }
        public static List<(int, int)> Crosswise(this int a, int b)
        {
            var arrayHor = Horizontal(a, b);
            var arrayVert = Vertical(a, b);
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public static bool Equals(this int a, int b, int eF, int eS)
        {
            var knightMoves = Crosswise(a, b);
            var endMoves = Crosswise(eF, eS);
            var result = endMoves.Intersect(knightMoves).ToList();
            return result.Count > 0;
        }
        public static bool Equals(this List<(int, int)> arr, int eF, int eS)
        {
            List<(int, int)> result = new List<(int, int)>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item.Item1, item.Item2));
            }
            var endMoves = Crosswise(eF, eS);
            return endMoves.Intersect(result).ToList().Count > 0;
        }
        public static bool Equals(this List<(int, int)> arr, List<(int, int)> arrEnd)
        {
            List<(int, int)> result = new List<(int, int)>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item.Item1, item.Item2));
            }
            List<(int, int)> resultEnd = new List<(int, int)>();
            foreach (var item in arrEnd)
            {
                resultEnd.AddRange(Crosswise(item.Item1, item.Item2));
            }
            return resultEnd.Intersect(result).ToList().Count > 0;
        }
        public static bool EqualsEnd(this List<(int, int)> arr, List<(int, int)> arrEnd)
        {
            List<(int, int)> result = new List<(int, int)>();
            List<(int, int)> resultStart = new List<(int, int)>();
            foreach (var item in arr)
            {
                result.AddRange(Crosswise(item.Item1, item.Item2));
            }
            foreach (var item in result)
            {
                resultStart.AddRange(Crosswise(item.Item1, item.Item2));
            }
            List<(int, int)> resultEnd = new List<(int, int)>();
            foreach (var item in arrEnd)
            {
                resultEnd.AddRange(Crosswise(item.Item1, item.Item2));
            }
            return resultEnd.Intersect(resultStart).ToList().Count > 0;
        }
    }
}
