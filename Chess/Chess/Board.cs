using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class ChessBoard
    {
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
            BoardColor();
        }
        private static void BoardColor()
        {
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        Console.SetCursorPosition(2 + (i - 1) * 4, 1 + (j - 1) * 2);
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write(" ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.SetCursorPosition(2 + (i - 1) * 4, 1 + (j - 1) * 2);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(' ');
                        Console.ResetColor();
                    }
                }
            }
        }
    }
}
