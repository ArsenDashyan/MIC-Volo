using System;

namespace Utility
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
                    return 808;
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
