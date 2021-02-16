using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGame
{
    class View
    {
        /// <summary>
        /// Ցուցադրում է խաղադաշտի սկզբնական տեսքը
        /// </summary>
        /// <param name="a">Խաղը սկսելուց սև արքաի առաջին կոորդինատ</param>
        /// <param name="b">Խաղը սկսելուց սև արքաի երկրորդ կոորդինատ</param>
        public static void ShowBoard(int a, int b)
        {
            Board();
            Manager.king.SetPosition(a,b);
            Manager.rookL.SetPosition(1,8);
            Manager.rookR.SetPosition(8,8);
            Manager.queen.SetPosition(4,8);
            Manager.kingG.SetPosition(5,8);
        }
        /// <summary>
        /// Ցուցադրում է խաղադաշտի տեսքը խաղի ժամանակ, ըստ կատարված քայլերի
        /// </summary>
        /// <param name="a">Սև արքայի ստացվող առաջին կոորդինատ</param>
        /// <param name="b">Սև արքայի ստացվող երկրորդ կոորդինատ</param>
        /// <param name="versia">Ըստ արքայի կատարած խաղի որոշվող քայլերի հաջորդականություն</param>
        public static void ShowBoard(int a, int b, int versia)
        {
            Board();
            switch (versia)
            {
                case 1:
                    Manager.king.SetPosition(a, b);
                    Manager.rookL.SetPosition(1, 3);
                    Manager.rookR.SetPosition(8, 8);
                    Manager.queen.SetPosition(4, 8);
                    Manager.kingG.SetPosition(5, 8);
                    break;
                case 2:
                    Manager.king.SetPosition(a, b);
                    Manager.rookL.SetPosition(1, 3);
                    Manager.rookR.SetPosition(8, 8);
                    Manager.queen.SetPosition(4, 3);
                    Manager.kingG.SetPosition(5, 8);
                    break;
                case 3:
                    Manager.king.SetPosition(a, b);
                    Manager.rookL.SetPosition(1, 2);
                    Manager.rookR.SetPosition(8, 8);
                    Manager.queen.SetPosition(4, 3);
                    Manager.kingG.SetPosition(5, 8);
                    break;
                case 4:
                    if (a== 5 && b ==1)
                    {
                        Manager.king.SetPosition(a, b);
                        Manager.rookL.SetPosition(1, 2);
                        Manager.rookR.SetPosition(8, 1);
                        Manager.queen.SetPosition(4, 3);
                        Manager.kingG.SetPosition(5, 8);
                    }
                    else
                    {
                        Manager.king.SetPosition(a, b);
                        Manager.rookL.SetPosition(1, 2);
                        Manager.rookR.SetPosition(8, 8);
                        Manager.queen.SetPosition(4, 1);
                        Manager.kingG.SetPosition(5, 8);
                    }
                    break;
            }
        }
        /// <summary>
        /// Ստեղծում է խաղադաշտ
        /// </summary>
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
        /// <summary>
        /// Ստեղծում է դաշտի գույները
        /// </summary>
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
