using System;
using System.Collections.Generic;
using Coordinats;

namespace Utility
{
    public static class Helper
    {
        public static Point WhenFirstHalf(this List<Point> arr, Point poi, int versia)
        {
            if (versia == 1)
            {
                foreach (var tupl in arr)
                {
                    if (tupl.X - poi.X == 1 && Math.Abs(tupl.Y - poi.Y) > 1)
                    {
                        return tupl;
                    }
                }
            }
            else
            {
                foreach (var point in arr)
                {
                    if (poi.X - point.X == 1 && Math.Abs(point.Y - poi.Y) > 1)
                    {
                        return point;
                    }
                }
            }
            return null;
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
        public static string IntToString(this int a)
        {
            string result = a switch
            {
                1 => "White King",
                2 => "White Queen",
                3 => "White RookL",
                4 => "White RookR",
                5 => "White Knight",
                6 => "Black King",
                _ => ""
            };
            return result;
        }
        public static (string, string) StringSplit(this string word)
        {
            string[] array = new string[2];
            array = word.Split(" ");
            return (array[0], array[1]);
        }
        public static Point WhenFirstHalfOn(this List<Point> arr, Point poi, int versia)
        {
            if (versia ==1)
            {
                foreach (var tupl in arr)
                {
                    if (tupl.X == poi.X && Math.Abs(tupl.Y - poi.Y) > 1)
                    {
                        return tupl;
                    }
                }
            }
            else
            {
                foreach (var tupl in arr)
                {
                    if (tupl.X == poi.X && Math.Abs(tupl.Y - poi.Y) > 1)
                    {
                        return tupl;
                    }
                }
            }
            return null;
        }
        
    }
}
