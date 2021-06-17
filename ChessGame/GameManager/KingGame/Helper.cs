using Figure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameManager
{
    public static class Helper
    {
        /// <summary>
        /// Check the game is baby or no
        /// </summary>
        /// <param name="list">Current figur last fore moves</param>
        /// <returns>Return true if figure move is baby</returns>
        public static bool BabyGame(this List<CoordinatePoint> list)
        {
            var tempOne = new List<CoordinatePoint>();
            var tempTwo = new List<CoordinatePoint>();
            if (list.Count >= 4)
            {
                for (int i = list.Count - 1; i >= list.Count - 3; i -= 2)
                {
                    tempOne.Add(list[i]);
                }
                for (int i = list.Count - 2; i >= list.Count - 3; i -= 2)
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

        /// <summary>
        /// Check current king position half
        /// </summary>
        /// <param name="coordinat">Current king coordinate</param>
        /// <returns>Return the half with inside current king</returns>
        public static int GetCurrentKingHalf(this CoordinatePoint coordinat)
        {
            if (coordinat.X >= 4 && coordinat.Y <= 3)
            {
                return 1;
            }
            else if (coordinat.X >= 4 && coordinat.Y >= 4)
            {
                return 4;
            }
            else if (coordinat.X <= 3 && coordinat.Y <= 3)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        /// <summary>
        /// Universal filtr method for IEnumerable collections
        /// </summary>
        /// <typeparam name="T">Type for collectios</typeparam>
        /// <param name="list">Collections name</param>
        /// <param name="func">Funcon for filtr</param>
        /// <returns></returns>
        public static bool Filtr<T>(this IEnumerable<T> list, Func<T, bool> func)
        {
            foreach (var item in list)
            {
                if (func(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Universal filtr method for IEnumerable collections
        /// </summary>
        /// <typeparam name="T">Type for collectios</typeparam>
        /// <param name="list">Collections name</param>
        /// <param name="func">Funcon for filtr</param>
        /// <returns></returns>
        public static IEnumerable<T> FiltrFor<T>(this IEnumerable<T> list, Func<T, bool> func)
        {
            foreach (var item in list)
            {
                if (func(item))
                    yield return item;
            }
        }

        /// <summary>
        /// Change the coordinat letter in int
        /// </summary>
        /// <param name="ch">Input Letter</param>
        /// <param name="number">Output convert number</param>
        /// <returns>Return true if a letter convert to numbre and false when not convert</returns>
        public static int CharToInt(this char ch)
        {
            switch (Char.ToLower(ch))
            {
                case 'a':
                    return 0;
                case 'b':
                    return 1;
                case 'c':
                    return 2;
                case 'd':
                    return 3;
                case 'e':
                    return 4;
                case 'f':
                    return 5;
                case 'g':
                    return 6;
                case 'h':
                    return 7;
                default:
                    return 999;

            }
        }
        public static FColor StringToEnum(this string color)
        {
            switch (color)
            {
                case "White":
                    return FColor.White;
                case "Black":
                    return FColor.Black;
                default:
                    return FColor.Black;
            }
        }
        public static CoordinatePoint StringToCoordinatPoint(this string coordinate)
        {
            var temp = coordinate.Split('.');
            var number = temp[0] switch
            {
                "a" => 0,
                "b" => 1,
                "c" => 2,
                "d" => 3,
                "e" => 4,
                "f" => 5,
                "g" => 6,
                "h" => 7,
                _ => 808,
            };
            return new CoordinatePoint(number, int.Parse(temp[1])-1);
        }
        public static CoordinatePoint GetCoordinateByString(this string path)
        {
            string[] strCurrent = path.Split('.');
            return new CoordinatePoint(int.Parse(strCurrent[0]), int.Parse(strCurrent[1]));
        }

    }
}
