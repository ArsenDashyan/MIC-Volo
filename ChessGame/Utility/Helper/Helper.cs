using Coordinats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class Helper
    {
        /// <summary>
        /// Check the game is baby or no
        /// </summary>
        /// <param name="list">Current figur last fore moves</param>
        /// <returns>Return true if figure move is baby</returns>
        public static bool BabyGame(this List<CoordinatPoint> list)
        {
            var tempOne = new List<CoordinatPoint>();
            var tempTwo = new List<CoordinatPoint>();
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
        /// Change the coordinat letter in int
        /// </summary>
        /// <param name="ch">Input Letter</param>
        /// <param name="number">Output convert number</param>
        /// <returns>Return true if a letter convert to numbre and false when not convert</returns>
        public static bool CharToInt(this char ch, out int number)
        {
            switch (Char.ToLower(ch))
            {
                case 'a':
                    number = 1;
                    return true;
                case 'b':
                    number = 2;
                    return true;
                case 'c':
                    number = 3;
                    return true;
                case 'd':
                    number = 4;
                    return true;
                case 'e':
                    number = 5;
                    return true;
                case 'f':
                    number = 6;
                    return true;
                case 'g':
                    number = 7;
                    return true;
                case 'h':
                    number = 8;
                    return true;
                default:
                    number = 808;
                    return false;
            }
        }

        /// <summary>
        /// Check current king position half
        /// </summary>
        /// <param name="coordinat">Current king coordinate</param>
        /// <returns>Return the half with inside current king</returns>
        public static int GetCurrentKingHalf(this CoordinatPoint coordinat)
        {
            if (coordinat.X >=4 && coordinat.Y <=3)
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
        public static bool Filtr<T>(this IEnumerable<T> list, Func<T,bool> func)
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
                  yield  return item;
            }
        }

        /// <summary>
        /// Change the white figure image
        /// </summary>
        /// <param name="str">Figure</param>
        /// <returns>Return image path for figure instance</returns>
        public static string WhiteFigurePath(this string str)
        {
            string result = str switch
            {
                "Queen" => "Resources/White.Queen.png",
                "King" => "Resources/White.King.png",
                "Bishop" => "Resources/White.Bishop.png",
                "Rook" => "Resources/White.Rook.png",
                "Knight" => "Resources/White.Knight.png",
                "Pawn" => "Resources/White.Pawn.png",
                _ => ""
            };
            return result;
        }

        /// <summary>
        /// Change the black figure image
        /// </summary>
        /// <param name="str">Figure</param>
        /// <returns>Return image path for figure instance</returns>
        public static string BlackFigurePath(this string str)
        {
            string result = str switch
            {
                "Queen" => "Resources/Black.Queen.png",
                "King" => "Resources/Black.King.png",
                "Bishop" => "Resources/Black.Bishop.png",
                "Rook" => "Resources/Black.Rook.png",
                "Knight" => "Resources/Black.Knight.png",
                "Pawn" => "Resources/Black.Pawn.png",
                _ => ""
            };
            return result;
        }
    }
}
