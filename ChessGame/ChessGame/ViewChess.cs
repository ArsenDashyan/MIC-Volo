using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    class ViewChess
    {
        public static void Show(int a, int b)
        {
            Board();
            ManagerCoordinats.king.SetPosition(a,b);
            ManagerCoordinats.rookL.SetPosition(1,8);
            ManagerCoordinats.rookR.SetPosition(8,8);
            ManagerCoordinats.queen.SetPosition(4,8);
            ManagerCoordinats.kingG.SetPosition(5,8);
        }

        public static void ShowBoard(int a, int b, int x)
        {
            Board();
            switch (x)
            {
                case 1:
                    ManagerCoordinats.king.SetPosition(a, b);
                    ManagerCoordinats.rookL.SetPosition(1, 3);
                    ManagerCoordinats.rookR.SetPosition(8, 8);
                    ManagerCoordinats.queen.SetPosition(4, 8);
                    ManagerCoordinats.kingG.SetPosition(5, 8);
                    break;
                case 2:
                    ManagerCoordinats.king.SetPosition(a, b);
                    ManagerCoordinats.rookL.SetPosition(1, 3);
                    ManagerCoordinats.rookR.SetPosition(8, 8);
                    ManagerCoordinats.queen.SetPosition(4, 3);
                    ManagerCoordinats.kingG.SetPosition(5, 8);
                    break;
                case 3:
                    ManagerCoordinats.king.SetPosition(a, b);
                    ManagerCoordinats.rookL.SetPosition(1, 3);
                    ManagerCoordinats.rookR.SetPosition(8, 8);
                    ManagerCoordinats.queen.SetPosition(4, 2);
                    ManagerCoordinats.kingG.SetPosition(5, 8);
                    break;
                case 4:
                    ManagerCoordinats.king.SetPosition(a, b);
                    ManagerCoordinats.rookL.SetPosition(1, 1);
                    ManagerCoordinats.rookR.SetPosition(8, 8);
                    ManagerCoordinats.queen.SetPosition(4, 2);
                    ManagerCoordinats.kingG.SetPosition(5, 8);
                    break;
            }
        }
        private static void Board()
        {
            Console.Clear();
            Console.WriteLine(@"+---+---+---+---+---+---+---+---+");
            for (int i = 1; i <9; i++)
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
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write('*');
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.SetCursorPosition(2 + (i - 1) * 4, 1 + (j - 1) * 2);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write('*');
                        Console.ResetColor();
                    }
                }
            }
        }
    }
}
