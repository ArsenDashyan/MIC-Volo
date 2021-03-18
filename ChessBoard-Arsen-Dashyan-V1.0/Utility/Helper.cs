using System;
using System.Collections.Generic;
using System.Linq;
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
        public static bool BabyGame(this List<Point> list)
        {
            List<Point> tempOne = new List<Point>();
            List<Point> tempTwo = new List<Point>();
            if (list.Count>=6)
            {
                for (int i = list.Count-1; i >= list.Count - 5; i-=2)
                {
                    tempOne.Add(list[i]);
                }
                for (int i = list.Count - 2; i >= list.Count - 5; i -= 2)
                {
                    tempTwo.Add(list[i]);
                }
            }
            var tempEndOne = tempOne.Distinct().ToList();
            var tempEndTwo = tempTwo.Distinct().ToList();
            if (tempEndOne.Count == 1 && tempEndTwo.Count == 1)
            {
                    return true;
            }
            return false;
        }

        #region Draft
        //Tagavori dirqin hamarjeq qayler
        //public static List<Point> KingAvailableMoves(this Point point)
        //{
        //    List<Point> list = new List<Point>();
        //    int versia = KingPositionInBord(point);
        //    switch (versia)
        //    {
        //        case 1:
        //        case 4:
        //            for (int i = point.X+1; i <= 8; i++)
        //            {
        //                var tempPoint = new Point(i, point.Y);
        //                list.Add(tempPoint);
        //            }
        //            break;
        //        case 2:
        //        case 3:
        //            for (int i = point.X-1; i >= 1; i--)
        //            {
        //                var tempPoint = new Point(i, point.Y);
        //                list.Add(tempPoint);
        //            }
        //            break;
        //    }
        //    return list;
        //}

        //Tagavori dirq yst qarordneri
        //public static int KingPositionInBord(this Point point)
        //{
        //    int versia = 0;
        //    if (point.X >=5 && point.X <=8)
        //    {
        //        versia = point.Y >= 5 && point.Y <= 8 ? 4 : 1;
        //        return versia; 
        //    }
        //    else
        //    {
        //        versia = point.Y >= 1 && point.Y <= 4 ? 2 : 3;
        //        return versia;
        //    }
        //}
        #endregion
    }
}
