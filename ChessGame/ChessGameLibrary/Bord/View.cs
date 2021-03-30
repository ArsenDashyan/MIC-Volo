using System;
using System.Collections.Generic;

namespace ChessGameLibrary
{
    public class View
    {
        public static List<string> figurs = new List<string>() { "White King   - 1", "White Queen  - 2", "White RookL  - 3",
                                                                 "White RookR  - 4", "White Knight - 5", "Black King   - 6",
                                                                  "White BishopL  - 7","White BishopR - 8","Black RookL - 9"};
        public static void Board()
        {
            Console.WriteLine(@"+---+---+---+---+---+---+---+---+");
            for (int i = 1; i < 9; i++)
            {
                Console.WriteLine($"|   |   |   |   |   |   |   |   |   {i}");
                Console.WriteLine(@"+---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("  A   B   C   D   E   F   G   H");
        }
        public static void ShowFigurs(int corrent)
        {
            Console.SetCursorPosition(40, 0);
            Console.WriteLine("Please enter a figur | For exit enter a e");
            if (corrent <= figurs.Count)
            {
                figurs[corrent - 1] = "Filled          ";
            }
            foreach (var item in figurs)
            {
                Console.SetCursorPosition(40, figurs.IndexOf(item) + 1);
                Console.WriteLine(item);
            }
        }
        public static void ClearText()
        {
            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(40, i);
                Console.WriteLine("                                              ");
            }
        }
    }
}
