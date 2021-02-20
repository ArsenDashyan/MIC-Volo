using System;
using System.Collections.Generic;
using System.Text;

namespace ChessFigurePosition
{
    class FigurCoord
    {
        public static List<(int letter, int number)> positions = new List<(int, int)>();
        public const int boardLeftSize = 8;
        public const int boardRightSize = 1;
        public static void GetCoordForFigur()
        {
            var tupl = GetCoordForStart("White King");
            Manager.kingW.SetPosition(tupl.letter,tupl.number);

            tupl = GetCoordForStart("White Quuen");
            Manager.queen.SetPosition(tupl.letter, tupl.number);

            tupl = GetCoordForStart("White Left Rook");
            Manager.rookL.SetPosition(tupl.letter, tupl.number);

            tupl = GetCoordForStart("White Right Rook");
            Manager.rookR.SetPosition(tupl.letter, tupl.number);
        }
        private static int GetLetters(char ch)
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
        public static (int letter, int number) GetCoordForStart(string figureName)
        {
            Console.SetCursorPosition(0, 18);
            Console.WriteLine($"Please enter a position for {figureName}");
            string input = Console.ReadLine();
            int i = GetLetters(input[0]);
            int j = Convert.ToInt32(input[1].ToString());
            bool isEqual = (i <= boardLeftSize && i >= boardRightSize && j >= boardRightSize 
                           && j <= boardLeftSize && !positions.Contains((i, j)));

            if (isEqual)
            {
                positions.Add((i, j));
                return (i, j);
            }
            while (!isEqual)
            {
                Console.WriteLine("You write non correct position!!!");
                (i, j) = GetCoordForStart(figureName);
                break;
            }
            return (i, j);
        }
    }
}
