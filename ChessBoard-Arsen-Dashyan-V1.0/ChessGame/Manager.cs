using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ChessGame
{
    class Manager
    {
        #region Property and Field
        private static int count = 1;
        private static List<(int letter, int number)> positions = new List<(int, int)>();
        private const int queenFigurActionMaxLenght = 13;
        private const int FigurMoveLenght = 14;
        private const int boardLeftSize = 8;
        private const int boardRightSize = 1;
        public static Model king = new Model("King", 5, 1, ConsoleColor.Red);
        public static Model rookL = new Model("Rook", 7, 0, ConsoleColor.White);
        public static Model rookR = new Model("Rook", 7, 7, ConsoleColor.White);
        public static Model queen = new Model("Queen", 7, 3, ConsoleColor.White);
        public static Model kingW = new Model("King", 7, 4, ConsoleColor.White);

        #endregion

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

        #region Queen and Rook Dangerous ocupancy
        /// <summary>
        /// Ստեղծում է տագուհու և նավակների հորիզոնական և ուղղահայաց հարվածի տիրույթները
        /// </summary>
        /// <param name="a">սկզբնական առաջին կոորդինատ</param>
        /// <param name="b">սկզբնական երկրորդ կոորդինա</param>
        /// <param name="x">Սև արքայի մուտքային առաջին կոորդինատ</param>
        /// <param name="y">Սև արքայի մուտքային երկրորդ կոորդինատ</param>
        /// <returns>Վերադարձնում է true, եթե արքայի մուտքային կոորդինատները ֆիգուրի ուղղահայաց և հորիզոնական հարվածի վրա են</returns>
        public static bool GetHorizontalVerticalIndex(int a, int b, int x, int y)
        {
            (int, int)[] arri = new (int, int)[FigurMoveLenght];
            int count = 0;
            bool result = true;

            for (int i = 1; i <= boardLeftSize; i++)
            {
                if (i != a)
                {
                    arri[count] = (i, b);
                    count++;
                }
            }
            for (int i = 1; i <= boardLeftSize; i++)
            {
                if (i != b)
                {
                    arri[count] = (a, i);
                    count++;
                }
            }
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
        /// Ստեղծում է սպիտակ թագուհու դեպի աջ անկյունագծային հարվածի տիրույթը
        /// </summary>
        /// <param name="a">Սպիտակ թագուհու սկզբնական առաջին կոորդինատ</param>
        /// <param name="b">Սպիտակ թագուհու սկզբնական երկրորդ կոորդինա</param>
        /// <param name="x">Սև արքայի մուտքային առաջին կոորդինատ</param>
        /// <param name="y">Սև արքայի մուտքային երկրորդ կոորդինատ</param>
        /// <returns>Վերադարձնում է true, եթե արքայի մուտքային կոորդինատները սպիտակ թագուգու աջ անկյունագծի վրա են</returns>
        public static bool GetRightIndex(int a, int b, int x, int y)
        {
            (int, int)[] arri = new (int, int)[queenFigurActionMaxLenght];
            bool result = true;
            int sum = a + b;
            int count = 0;

            for (int i = 1; i <= boardLeftSize; i++)
            {
                for (int j = 1; j <= boardLeftSize; j++)
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
            (int, int)[] arri = new (int, int)[queenFigurActionMaxLenght];
            int sub = 0;
            int count = 0;
            bool result = true;

            if (a < b)
            {
                sub = Math.Abs(a - b);
                for (int i = 1; i <= boardLeftSize; i++)
                {
                    for (int j = 1; j <= boardLeftSize; j++)
                    {
                        if (i - j == sub)
                        {
                            arri[count] = (j, i);
                            count++;
                        }
                    }
                }
            }
            else
            {
                sub = a - b;
                for (int i = 1; i <= 8; i++)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        if (i - j == sub)
                        {
                            arri[count] = (i, j);
                            count++;
                        }
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

        #endregion

        #region ForStartGame
        /// <summary>
        /// Խաղի սկզբում քարերը տեղադրում է ըստ տրված կոորդինատների
        /// </summary>
        public static void GetCoordForFigur()
        {
            var tupl = GetCoord("White", "King");
            Manager.kingW.SetPosition(tupl.letter, tupl.number);

            tupl = GetCoord("White", "Quuen");
            Manager.queen.SetPosition(tupl.letter, tupl.number);

            tupl = GetCoord("White", "Rook");
            Manager.rookL.SetPosition(tupl.letter, tupl.number);

            tupl = GetCoord("White", "Rook");
            Manager.rookR.SetPosition(tupl.letter, tupl.number);

            tupl = GetCoord("Black", "King");
            Manager.king.SetPosition(tupl.letter, tupl.number);
        }

        /// <summary>
        /// Օգտագործողից ստանում և ստուգում է քարերի կոորդինատները
        /// </summary>
        /// <param name="figureName">Խաղաքարի անունը</param>
        /// <param name="figureColor">Խաղաքարի գույնը</param>
        /// <returns>Վերադարձնում է քարի կոորդինատը կորտեժի տեսքով</returns>
        public static (int letter, int number) GetCoord(string figureColor, string figureName)
        {
            Console.SetCursorPosition(40, 0);
            Console.WriteLine($"Please enter a position for {figureColor} {figureName}");
            Console.SetCursorPosition(40, 1);
            string input = Console.ReadLine();
            int i = GetLetters(input[0]);
            int j = Convert.ToInt32(input[1].ToString());
            bool isEqual;
            if (figureColor == "Black" && figureName == "King")
            {
                isEqual = (i <= boardLeftSize && i >= boardRightSize && j >= boardRightSize
                          && j <= boardLeftSize && !positions.Contains((i, j)) && GetRightIndex(queen.FCoord, queen.SCoord, i, j)
                          && GetLeftIndex(queen.FCoord, queen.SCoord, i, j) && GetHorizontalVerticalIndex(queen.FCoord, queen.SCoord, i, j)
                          && GetHorizontalVerticalIndex(rookL.FCoord, rookL.SCoord, i, j) && GetHorizontalVerticalIndex(rookR.FCoord, rookR.SCoord, i, j)
                          && (Math.Abs(i - kingW.FCoord) > 1 || Math.Abs(j - kingW.SCoord) > 1));
            }
            else
            {
                isEqual = (i <= boardLeftSize && i >= boardRightSize && j >= boardRightSize
                           && j <= boardLeftSize && !positions.Contains((i, j)));
            }

            if (isEqual)
            {
                positions.Add((i, j));
                return (i, j);
            }
            while (!isEqual)
            {
                Console.SetCursorPosition(40, 3);
                Console.WriteLine("You write non correct position!!!");
                (i, j) = GetCoord(figureColor, figureName);
                break;
            }
            return (i, j);
        }

        #endregion
    }
}
