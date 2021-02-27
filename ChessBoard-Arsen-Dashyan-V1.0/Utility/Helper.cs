using System;
using System.Collections.Generic;

namespace Utility
{
    public static class Helper
    {
        public static (int, int) EndPosition(this List<(int, int)> arr, int i)
        {
            foreach (var item in arr)
            {
                if (item.Item1 == i)
                {
                    return (item);
                }
            }
            return (0, 0);
        }
        public static (int, int) WhenFirstHalf(this List<(int, int)> arr, int i, int j)
        {
            foreach (var tupl in arr)
            {
                if (tupl.Item1 - i == 1 && Math.Abs(tupl.Item2 - j) > 1)
                {
                    return (tupl.Item1, tupl.Item2);
                }
            }
            return (0, 0);
        }
        public static (int, int) WhenSecondHalf(this List<(int, int)> arr, int i, int j)
        {
            foreach (var tupl in arr)
            {
                if (i - tupl.Item1 == 1 && Math.Abs(tupl.Item2 - j) > 1)
                {
                    return (tupl.Item1, tupl.Item2);
                }
            }
            return (0, 0);
        }
        public static int CharToInt(this char ch)
        {
            switch (Char.ToLower(ch))
            {
                case 'a':
                    return 1;
                case 'b':
                    return 2;
                case 'c':
                    return 3;
                case 'd':
                    return 4;
                case 'e':
                    return 5;
                case 'f':
                    return 6;
                case 'g':
                    return 7;
                case 'h':
                    return 8;
            }
            return 0;
        }
    }
}
