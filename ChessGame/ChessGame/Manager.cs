using System;
using System.Linq;
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
            bool isAction = (Math.Abs(king.FCoord - tuple.Item1) <= 1 & Math.Abs(king.SCoord - tuple.Item2) <= 1);

            if (isAction)
            {
                View.ShowBoard(tuple.Item1, tuple.Item2);

                while (count <= 4)
                {
                    Thread.Sleep(1500);
                    View.ShowBoard(tuple.Item1, tuple.Item2, count);
                    if (count != 4)
                    {
                        tuple = GetCoordinats();
                        if ((Math.Abs(king.FCoord - tuple.Item1) <= 1 & Math.Abs(king.SCoord - tuple.Item2) <= 1))
                        {
                            View.ShowBoard(tuple.Item1, tuple.Item2, count);
                        }
                        else
                        {
                            Console.SetCursorPosition(40, 6);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Non correct action");
                            Console.ResetColor();
                            Console.SetCursorPosition(0, 18);
                            break;
                        }
                    }
                    count++;
                }
                Console.WriteLine("\n\nGame over");
            }
            else
            {
                Console.SetCursorPosition(40, 6);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Non correct action");
                Console.ResetColor();
                Console.SetCursorPosition(0, 18);
            }
        }
        /// <summary>
        /// Սև արքայի համար պահանջում է կոորդինատներ
        /// </summary>
        /// <returns>Վերադարձնում է սև արքայի կոորդինատները կորտեժի տեսքով</returns>
        private static (int, int) GetCoordinats()
        {
            Console.SetCursorPosition(40, 0);
            Console.WriteLine("Please enter a letter coordinate");
            Console.SetCursorPosition(40, 1);
            char a = char.Parse(Console.ReadLine());
            int aFirst = GetLetters(a);
            Console.SetCursorPosition(40, 2);
            Console.WriteLine("Please enter a number coordinate");
            Console.SetCursorPosition(40, 3);
            int b = int.Parse(Console.ReadLine());

            if (aFirst > queen.FCoord && b < rookL.SCoord && b < queen.SCoord
                && GetLeftIndex(queen.FCoord, queen.SCoord, aFirst, b) && GetRightIndex(queen.FCoord, queen.SCoord, aFirst, b))
                return (aFirst, b);

            while (!(aFirst > queen.FCoord && b < rookL.SCoord && b < queen.SCoord
                && GetLeftIndex(queen.FCoord, queen.SCoord, aFirst, b) && GetRightIndex(queen.FCoord, queen.SCoord, aFirst, b)))
            {
                Console.SetCursorPosition(40, 4);
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
        /// <summary>
        /// Ստեղծում է սպիտակ թագուհու դեպի աջ անկյունագծային հարվածի տիրույթը
        /// </summary>
        /// <param name="a">Սպիտակ թագուհու սկզբնական առաջին կոորդինատ</param>
        /// <param name="b">Սպիտակ թագուհու սկզբնական երկրորդ կոորդինա</param>
        /// <param name="x">Սև արքայի մուտքային առաջին կոորդինատ</param>
        /// <param name="y">Սև արքայի մուտքային երկրորդ կոորդինատ</param>
        /// <returns>Վերադարձնում է true, եթե արքայի մուտքային կոորդինատները սպիտակ թագուգու աջ անկյունագծի վրա են</returns>
        public static bool GetRightIndex(int a, int b, int x, int y)
        {
            (int, int)[] arri = new (int, int)[13];
            bool result = true;
            int sum = a + b;
            int count = 0;

            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    if (i + j == sum)
                    {
                        arri[count] = (i, j);
                        count++;
                    }
                }
            }
            arri = arri.Where(c => c.Item1 != 0 && c.Item2 != 0).ToArray();
            for (int i = 0; i < arri.Length; i++)
            {
                if ((x, y) == arri[i])
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// Ստեղծում է սպիտակ թագուհու դեպի ձախ հարվածի տիրույթը
        /// </summary>
        /// <param name="a">Սպիտակ թագուհու սկզբնական առաջին կոորդինատ</param>
        /// <param name="b">Սպիտակ թագուհու սկզբնական երկրորդ կոորդինատ</param>
        /// <param name="x">Սև արքայի մուտքային առաջին կոորդինատ</param>
        /// <param name="y">Սև արքայի մուտքային երկրորդ կոորդինատ</param>
        /// <returns>Վերադարձնում է true, եթե արքայի մուտքային կոորդինատները սպիտակ թագուգու ձախ անկյունագծի վրա են</returns>
        public static bool GetLeftIndex(int a, int b, int x, int y)
        {
            (int, int)[] arri = new (int, int)[13];
            int sub = Math.Abs(a - b);
            int count = 0;
            bool result = true;

            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    if (i - j == sub)
                    {
                        arri[count] = (j, i);
                        count++;
                    }
                }
            }
            arri = arri.Where(c => c.Item1 != 0 && c.Item2 != 0).ToArray();
            for (int i = 0; i < arri.Length; i++)
            {
                if ((x, y) == arri[i])
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}
