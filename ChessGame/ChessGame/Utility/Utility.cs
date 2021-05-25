using System;
using System.Collections.Generic;

namespace ChessGame
{
    public static class Utility
    {
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
        /// Change the coordinat letter in int
        /// </summary>
        /// <param name="ch">Input Letter</param>
        /// <param name="number">Output convert number</param>
        /// <returns>Return true if a letter convert to numbre and false when not convert</returns>
        public static int CharToInt(this char ch)
        {
            return Char.ToLower(ch) switch
            {
                'a' => 0,
                'b' => 1,
                'c' => 2,
                'd' => 3,
                'e' => 4,
                'f' => 5,
                'g' => 6,
                'h' => 7,
                _ => 808,
            };
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
                "Queen" => "Resources/Pictures/White.Queen.png",
                "King" => "Resources/Pictures/White.King.png",
                "Bishop" => "Resources/Pictures/White.Bishop.png",
                "Rook" => "Resources/Pictures/White.Rook.png",
                "Knight" => "Resources/Pictures/White.Knight.png",
                "Pawn" => "Resources/Pictures/White.Pawn.png",
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
                "Queen" => "Resources/Pictures/Black.Queen.png",
                "King" => "Resources/Pictures/Black.King.png",
                "Bishop" => "Resources/Pictures/Black.Bishop.png",
                "Rook" => "Resources/Pictures/Black.Rook.png",
                "Knight" => "Resources/Pictures/Black.Knight.png",
                "Pawn" => "Resources/Pictures/Black.Pawn.png",
                _ => ""
            };
            return result;
        }

        /// <summary>
        /// Universal filtr method for IEnumerable collections
        /// </summary>
        /// <typeparam name="T">Type for collectios</typeparam>
        /// <param name="list">Collections name</param>
        /// <param name="func">Funcon for filtr</param>
        /// <returns></returns>
        public static bool Filtr<T>(this IEnumerable<T> list, Func<T, bool> func, out T temp)
        {
            foreach (var item in list)
            {
                if (func(item))
                {
                    temp = item;
                    return true;
                }
            }
            temp = default;
            return false;
        }
    }
}
