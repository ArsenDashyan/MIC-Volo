using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChessGame
{
    class Manager
    {
        public static int count = 1;
        public static Model king = new Model("King", 5, 1, ConsoleColor.Red);
        public static Model rookL = new Model("RookL", 7, 0, ConsoleColor.White);
        public static Model rookR = new Model("RookR", 7, 7, ConsoleColor.White);
        public static Model queen = new Model("Queen", 7, 3, ConsoleColor.White);
        public static Model kingG = new Model("King", 7, 4, ConsoleColor.White);

        /// <summary>
        /// Խաղային լոգիկա, որը կազմակերպում է խաղի ընթացքը
        /// </summary>
        public static void ChessLogic()
        {
            View.ShowBoard(king.FCoord, king.SCoord);

            var tuple = GetCoordinats();
            bool isAction = (Math.Abs(king.FCoord - tuple.Item1) <= 1 && Math.Abs(king.SCoord - tuple.Item2) <= 1);

            if (isAction)
            {
                View.ShowBoard(tuple.Item1, tuple.Item2);

                while (count <= 4)
                {
                    Thread.Sleep(1000);
                    View.ShowBoard(tuple.Item1, tuple.Item2, count);
                    if (count != 4)
                    {
                        tuple = GetCoordinats();
                        if (isAction)
                        {
                            View.ShowBoard(tuple.Item1, tuple.Item2, count);
                        }
                    }
                    count++;
                }
                Console.WriteLine("\n\nGame over");
            }
            else
            {
                Console.WriteLine("Non correct action");
            }
        }
        /// <summary>
        /// Սև արքայի համար պահանջում է կոորդինատներ
        /// </summary>
        /// <returns>Վերադարձնում է սև արքայի կոորդինատները կորտեժի տեսքով</returns>
        private static (int, int) GetCoordinats()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Please enter a letter coordinate");
            char a = char.Parse(Console.ReadLine());
            int aFirst = GetLetters(a);

            Console.WriteLine("Please enter a number coordinate");
            int b = int.Parse(Console.ReadLine());
            
            if ((aFirst > queen.FCoord && b < rookL.SCoord && b < queen.SCoord ))
                return (aFirst, b);

            while (!(aFirst > queen.FCoord && b < rookL.SCoord && b < queen.SCoord))
            {
                Console.WriteLine("Please enter a correct coordinats");
                (aFirst, b) = GetCoordinats();
                break;
            }

            return (aFirst, b);
        }
        /// <summary>
        /// Խաղացողի մուտքային տառի վերածում թվի
        /// </summary>
        /// <param name="ch">Ներմուծվող տառ</param>
        /// <returns>Վերադարձնում է թվային արժեք սև արքայի առաջին կոորդինատի համար</returns>
        private static int GetLetters(char ch)
        {
            switch (ch)
            {
                case 'A':
                case 'a':
                    return 1;
                case 'B':
                case 'b':
                    return 2;
                case 'C':
                case 'c':
                    return 3;
                case 'D':
                case 'd':
                    return 4;
                case 'E':
                case 'e':
                    return 5;
                case 'F':
                case 'f':
                    return 6;
                case 'G':
                case 'g':
                    return 7;
                case 'H':
                case 'h':
                    return 8;
            }
            return 0;
        }
    }
}
