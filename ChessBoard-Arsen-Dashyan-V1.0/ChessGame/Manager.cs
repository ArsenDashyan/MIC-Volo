using System;
using System.Collections.Generic;
using Utility;

namespace ChessGame
{
    partial class Manager
    {
        #region Property and Feld
        public static List<(int letter, int number)> positions = new List<(int, int)>();
        private const int leftSize = 8;
        private const int rightSize = 1;
        public static King king = new King("King", ConsoleColor.Red);
        public static Rook rookL = new Rook("RookL", ConsoleColor.White);
        public static Rook rookR = new Rook("RookR", ConsoleColor.White);
        public static Queen queen = new Queen("Queen", ConsoleColor.White);
        public static King kingW = new King("King", ConsoleColor.White);
        #endregion

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
            int i = input[0].CharToInt();
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

        /// <summary>
        /// Chack tke king move
        /// </summary>
        /// <param name="i">Black king first coordinat</param>
        /// <param name="j">Black king second coordinat</param>
        /// <returns>Return true if king coordinat is a permissible</returns>
        private static bool IsKingAction(int i, int j)
        {
            if (InsideBord(i, j) && !positions.Contains((i, j)) && !DangerousPosition().Contains((i, j)) && WhiteKingOcupancy(i, j) && (king.FCoord,king.SCoord) != (i,j))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Created a positions whithe wer can a white figures move
        /// </summary>
        /// <returns>Return the Available moves</returns>
        public static List<(int, int)> DangerousPosition()
        {
            List<(int, int)> result = new List<(int, int)>();
            var arrayQueen = queen.AvailableMoves();
            var arrayRookL = rookL.AvailableMoves();
            var arrayRookR = rookR.AvailableMoves();
            result.AddRange(arrayQueen);
            result.AddRange(arrayRookL);
            result.AddRange(arrayRookR);

            return result;
        }

        /// <summary>
        /// Chack the coordinat inside bord or no
        /// </summary>
        /// <param name="i">First Coordinat</param>
        /// <param name="j">Second Coordinat</param>
        /// <returns>Return true when input coordinats inside bord</returns>
        private static bool InsideBord(int i, int j)
        {
            if (i <= leftSize && i >= rightSize && j >= rightSize && j <= leftSize)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Chack black king and white king positions
        /// </summary>
        /// <param name="i">Black king first coordinat</param>
        /// <param name="j">Black king second coordinat</param>
        /// <returns>Return true when black and white king away</returns>
        private static bool WhiteKingOcupancy(int i, int j)
        {
            if ((Math.Abs(i - kingW.FCoord) > 1 || Math.Abs(j - kingW.SCoord) > 1))
                return true;
            else
                return false;
        }

        #endregion

        #endregion

        #region Random Game Logic

        /// <summary>
        /// Play the Game
        /// </summary>
        public static void Play()
        {
            View.Board();
            Placement();
            do
            {
                if (king.AvailableMoves().Count != 0)
                {
                    KingPosition();
                    if (king.FCoord == 1 || king.FCoord == 8)
                    {
                        break;
                    }
                }
                else
                {
                    Console.SetCursorPosition(40, 8);
                    Console.WriteLine("Game over");
                    break;
                }

            } while (king.FCoord != 1 || king.FCoord != 8);

            if (king.AvailableMoves().Count != 0)
            {
                PositionEnd();
            }
            else
            {
                Console.SetCursorPosition(40, 8);
                Console.WriteLine("Game over");
                return;
            }
        }

        /// <summary>
        /// Chack the Black king position in bord
        /// </summary>
        public static void KingPosition()
        {
            var tupl = InputCoordinats("Black", "King");
            if (king.IsMove(tupl.let, tupl.num))
            {
                if (tupl.let <= 4)
                    Half(tupl.let, tupl.num, 1);
                else
                    Half(tupl.let, tupl.num, 2);
            }
        }

        /// <summary>
        /// Start the finish game when black king in first or end position
        /// </summary>
        public static void PositionEnd()
        {
            var tupl = InputCoordinats("Black", "King");
            if (king.IsMove(tupl.let, tupl.num))
            {
                if (rookL.FCoord == 2 || rookL.FCoord == 7)
                {
                    EndGame(tupl.let, tupl.num, queen, rookR);
                    return;
                }
                if (rookR.FCoord == 2 || rookR.FCoord == 7)
                {
                    EndGame(tupl.let, tupl.num, queen, rookL);
                    return;
                }
                if (queen.FCoord == 2 || queen.FCoord == 7)
                {
                    EndGame(tupl.let, tupl.num, rookR, rookL);
                    return;
                }
            }
        }

        /// <summary>
        /// Chack the versia for king positions and random moving white figure
        /// </summary>
        /// <param name="a">Black king first coordinat</param>
        /// <param name="b">Black king second coordinat</param>
        /// <param name="vers">King position in bord half</param>
        private static void Half(int a, int b, int vers)
        {
            king.SetPosition(a, b);
            int figureRandom = new Random().Next(1, 4);
            var tuplL = (0, 0);
            var tuplR = (0, 0);
            var tuplQ = (0, 0);
            if (vers == 1)
            {
                tuplL = rookL.AvailableMoves().WhenFirstHalf(a, b);
                tuplR = rookR.AvailableMoves().WhenFirstHalf(a, b);
                tuplQ = queen.AvailableMoves().WhenFirstHalf(a, b);
            }
            else
            {
                tuplL = rookL.AvailableMoves().WhenSecondHalf(a, b);
                tuplR = rookR.AvailableMoves().WhenSecondHalf(a, b);
                tuplQ = queen.AvailableMoves().WhenSecondHalf(a, b);
            }
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

        /// <summary>
        /// For end game chack white figure coordinat avialable move 
        /// </summary>
        /// <param name="a">black king first coordinat</param>
        /// <param name="b">black king second coordinat</param>
        /// <param name="qu">Queen instanse</param>
        /// <param name="ro">Rook instanse</param>
        private static void EndGame(int a, int b, Queen qu, Rook ro)
        {
            king.SetPosition(a, b);
            int figureRandom = new Random().Next(2, 4);
            var tuplQ = qu.AvailableMoves().EndPosition(a);
            var tuplR = ro.AvailableMoves().EndPosition(a);

            switch (figureRandom)
            {
                case 2:
                    if (tuplR != (0, 0))
                        ro.SetPosition(tuplR.Item1, tuplR.Item2);
                    else
                        goto case 3;
                    break;
                case 3:
                    if (tuplQ != (0, 0))
                        qu.SetPosition(tuplQ.Item1, tuplQ.Item2);
                    else
                        goto case 2;
                    break;
            }
            Console.SetCursorPosition(40, 8);
            Console.WriteLine("Game over");
        }

        /// <summary>
        /// For end game chack white figure coordinat avialable move 
        /// </summary>
        /// <param name="a">black king first coordinat</param>
        /// <param name="b">black king second coordinat</param>
        /// <param name="qu">Rook instanse</param>
        /// <param name="ro">Rook instanse</param>
        private static void EndGame(int a, int b, Rook qu, Rook ro)
        {
            king.SetPosition(a, b);
            int figureRandom = new Random().Next(2, 4);
            var tuplQ = qu.AvailableMoves().EndPosition(a);
            var tuplR = ro.AvailableMoves().EndPosition(a);

            switch (figureRandom)
            {
                case 2:
                    if (tuplR != (0, 0))
                        ro.SetPosition(tuplR.Item1, tuplR.Item2);
                    else
                        goto case 3;
                    break;
                case 3:
                    if (tuplQ != (0, 0))
                        qu.SetPosition(tuplQ.Item1, tuplQ.Item2);
                    else
                        goto case 2;
                    break;
            }
            Console.SetCursorPosition(40, 8);
            Console.WriteLine("Game over");
        }
        #endregion
    }
}
