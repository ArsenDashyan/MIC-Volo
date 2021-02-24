using System;
using System.Collections.Generic;
using System.Threading;

namespace ChessGame
{
    partial class Manager
    {
        #region Property and Field
        public static List<(int letter, int number)> positions = new List<(int, int)>();
        private const int boardLeftSize = 8;
        private const int boardRightSize = 1;
        public static King king = new King("King", ConsoleColor.Red);
        public static Rook rookL = new Rook("Rook", ConsoleColor.White);
        public static Rook rookR = new Rook("Rook", ConsoleColor.White);
        public static Queen queen = new Queen("Queen", ConsoleColor.White);
        public static King kingW = new King("King", ConsoleColor.White);
        #endregion
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

        #region ForStartGame
        /// <summary>
        /// Խաղի սկզբում քարերը տեղադրում է ըստ տրված կոորդինատների
        /// </summary>
        public static void Placement()
        {
            var tupl = InputCoordinats("White", "King");
            kingW.SetPosition(tupl.let, tupl.num);

            tupl = InputCoordinats("White", "Queen");
            queen.SetPosition(tupl.let, tupl.num);

            tupl = InputCoordinats("White ", "Rook");
            rookL.SetPosition(tupl.let, tupl.num);

            tupl = InputCoordinats("White ", "Rook");
            rookR.SetPosition(tupl.let, tupl.num);

            tupl = InputCoordinats("Black", " King");
            king.SetPosition(tupl.let, tupl.num);
        }

        /// <summary>
        /// Օգտագործողից ստանում և ստուգում է քարերի կոորդինատները
        /// </summary>
        /// <param name="figureName">Խաղաքարի անունը</param>
        /// <param name="figureColor">Խաղաքարի գույնը</param>
        /// <returns>Վերադարձնում է քարի կոորդինատը կորտեժի տեսքով</returns>
        public static (int let, int num) InputCoordinats(string figureColor, string figureName)
        {
            Console.SetCursorPosition(40, 0);
            Console.WriteLine($"Please enter a position for {figureColor} {figureName}");
            Console.SetCursorPosition(40, 1);
            string input = Console.ReadLine();
            int i = GetLetters(input[0]);
            int j = Convert.ToInt32(input[1].ToString());
            bool isEqual;
            if (figureColor == "Black")
                isEqual = IsKingAction(i, j);
            else
                isEqual = (InsideBord(i, j) && !positions.Contains((i, j)));

            if (isEqual)
            {
                if (figureColor != "Black")
                    positions.Add((i, j));
                return (i, j);
            }
            while (!isEqual)
            {
                Console.SetCursorPosition(40, 3);
                Console.WriteLine("Non correct position!");
                (i, j) = InputCoordinats(figureColor, figureName);
                break;
            }
            return (i, j);
        }
        #region Chack The Dangerous Positions
        private static bool IsKingAction(int i, int j)
        {
            if (InsideBord(i, j) && !positions.Contains((i, j)) && !DangerousPosition().Contains((i, j)) && WhiteKingOcupancy(i, j))
                return true;
            else
                return false;
        }
        private static List<(int, int)> DangerousPosition()
        {
            List<(int, int)> result = new List<(int, int)>();
            var arrayQueen = queen.AvAvailableMoves();
            var arrayRookL = rookL.AvAvailableMoves();
            var arrayRookR = rookR.AvAvailableMoves();
            result.AddRange(arrayQueen);
            result.AddRange(arrayRookL);
            result.AddRange(arrayRookR);

            return result;
        }
        private static bool InsideBord(int i, int j)
        {
            if (i <= boardLeftSize && i >= boardRightSize && j >= boardRightSize && j <= boardLeftSize)
                return true;
            else
                return false;
        }
        private static bool WhiteKingOcupancy(int i, int j)
        {
            if ((Math.Abs(i - kingW.FCoord) > 1 || Math.Abs(j - kingW.SCoord) > 1))
                return true;
            else
                return false;
        }

        #endregion

        #endregion

        #region Random Game

        public static void Logic()
        {
            View.Board();
            Placement();

            while (king.FCoord != 1)
            {
                KingPosition();
            }
        }

        public static void KingPosition()
        {
            var tupl = InputCoordinats("Black", "King");
            if (king.IsMove(tupl.let, tupl.num))
            {
                if (tupl.let <= 4)
                {
                    FirstHalf(tupl.let, tupl.num);
                }
                else
                {
                    SecondHalf(tupl.let, tupl.num);
                }
            }
        }

        public static (int, int) WhenFirstHalf(List<(int, int)> arr, int i, int j)
        {
            foreach (var tupl in arr)
            {
                if (tupl.Item1 - i == 1 && Math.Abs(tupl.Item2 - j) > 1)
                {
                    return (tupl.Item1, tupl.Item2);
                }
            }
            return (0, 0);
        }
        public static (int, int) WhenSecondHalf(List<(int, int)> arr, int i, int j)
        {
            foreach (var tupl in arr)
            {
                if (i - tupl.Item1 == 1 && Math.Abs(tupl.Item2 - j) > 1)
                {
                    return (tupl.Item1, tupl.Item2);
                }
            }
            return (0, 0);
        }
        private static void FirstHalf(int a, int b)
        {
            king.SetPosition(a, b);
            int figureRandom = new Random().Next(1, 4);
            var tuplL = WhenFirstHalf(rookL.AvAvailableMoves(), a, b);
            var tuplR = WhenFirstHalf(rookR.AvAvailableMoves(), a, b);
            var tuplQ = WhenFirstHalf(queen.AvAvailableMoves(), a, b);

            switch (figureRandom)
            {
                case 1:
                    if (tuplL != (0, 0))
                        rookL.SetPosition(tuplL.Item1, tuplL.Item2);
                    else
                        goto case 2;
                    break;
                case 2:
                    if (tuplR != (0, 0))
                        rookR.SetPosition(tuplR.Item1, tuplR.Item2);
                    else
                        goto case 3;
                    break;
                case 3:
                    if (tuplQ != (0, 0))
                        queen.SetPosition(tuplQ.Item1, tuplQ.Item2);
                    else
                        goto case 1;
                    break;
            }
        }
        private static void SecondHalf(int a, int b)
        {
            king.SetPosition(a, b);
            int figureRandom = new Random().Next(1, 4);
            var tuplL = WhenSecondHalf(rookL.AvAvailableMoves(), a, b);
            var tuplR = WhenSecondHalf(rookR.AvAvailableMoves(), a, b);
            var tuplQ = WhenSecondHalf(queen.AvAvailableMoves(), a, b);

            switch (figureRandom)
            {
                case 1:
                    if (tuplL != (0, 0))
                        rookL.SetPosition(tuplL.Item1, tuplL.Item2);
                    else
                        goto case 2;
                    break;
                case 2:
                    if (tuplR != (0, 0))
                        rookR.SetPosition(tuplR.Item1, tuplR.Item2);
                    else
                        goto case 3;
                    break;
                case 3:
                    if (tuplQ != (0, 0))
                        queen.SetPosition(tuplQ.Item1, tuplQ.Item2);
                    else
                        goto case 1;
                    break;
            }
        }

        #endregion
    }
}
