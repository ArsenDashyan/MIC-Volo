using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    class View
    {
        private static List<string> figurs = new List<string>() { "White King   - 1", "White Queen  - 2", "White RookL  - 3",
                                                                 "White RookR  - 4", "White Knight - 5", "Black King   - 6"};
        public static void Board()
        {
            Console.Clear();
            Console.WriteLine(@"+---+---+---+---+---+---+---+---+");
            for (int i = 1; i < 9; i++)
            {
                Console.WriteLine($"|   |   |   |   |   |   |   |   |   {i}");
                Console.WriteLine(@"+---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("  A   B   C   D   E   F   G   H");
        }

        //private static void BoardColor()
        //{
        //    for (int i = 1; i < 9; i++)
        //    {
        //        for (int j = 1; j < 9; j++)
        //        {
        //            if ((i + j) % 2 == 0)
        //            {
        //                Console.SetCursorPosition(2 + (i - 1) * 4, 1 + (j - 1) * 2);
        //                Console.BackgroundColor = ConsoleColor.White;
        //                Console.Write(" ");
        //                Console.ResetColor();
        //            }
        //            else
        //            {
        //                Console.SetCursorPosition(2 + (i - 1) * 4, 1 + (j - 1) * 2);
        //                Console.BackgroundColor = ConsoleColor.Black;
        //                Console.Write(' ');
        //                Console.ResetColor();
        //            }
        //        }
        //    }
        //}

        public static void ShowFigurs(int corrent)
        {
            Console.SetCursorPosition(40, 0);
            Console.WriteLine("Please enter a figur | For exit enter a e");
            if (corrent <= figurs.Count)
            {
                figurs[corrent -1] = "Filled          ";
            }
            foreach (var item in figurs)
            {
                Console.SetCursorPosition(40, figurs.IndexOf(item) + 1);
                Console.WriteLine(item);
            }
        }
    }
}
