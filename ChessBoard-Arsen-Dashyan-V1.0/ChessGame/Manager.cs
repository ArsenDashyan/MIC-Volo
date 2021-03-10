using System;
using System.Collections.Generic;
using Utility;

namespace ChessGame
{
    public partial class Manager
    {
        #region Property and Feld
        private const int leftSize = 8;
        private const int rightSize = 1;
        public static int count = 0;
        public static King kingBlack = new King("King", ConsoleColor.Red);
        public static Rook rookL = new Rook("RookL", ConsoleColor.White);
        public static Rook rookR = new Rook("RookR", ConsoleColor.White);
        public static Queen queen = new Queen("Queen", ConsoleColor.White);
        public static King kingWhite = new King("King", ConsoleColor.White);
        public static Knights knight = new Knights("knight", ConsoleColor.White);
        public static List<(int letter, int number)> positions = new List<(int, int)>()
        {(queen.FCoord,queen.SCoord),(rookL.FCoord,rookL.SCoord),(rookR.FCoord, rookR.SCoord),
        (kingBlack.FCoord,kingBlack.SCoord),(kingWhite.FCoord,kingWhite.SCoord),(knight.FCoord,knight.SCoord) };
        #endregion

        #region Chack The Dangerous Positions

        /// <summary>
        /// Chack tke king move
        /// </summary>
        /// <param name="i">Black king first coordinat</param>
        /// <param name="j">Black king second coordinat</param>
        /// <returns>Return true if king coordinat is a permissible</returns>
        private static bool IsKingAction(int i, int j)
        {
            if (InsideBord(i, j) && !positions.Contains((i, j)) && !DangerousPosition().Contains((i, j)) && WhiteKingOcupancy(i, j) && (kingBlack.FCoord, kingBlack.SCoord) != (i, j))
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
            if ((Math.Abs(i - kingWhite.FCoord) > 1 || Math.Abs(j - kingWhite.SCoord) > 1))
                return true;
            else
                return false;
        }
        #endregion

        #region Random Game Logic

        /// <summary>
        /// Play the Game
        /// </summary>
        public static void Play()
        {
            View.Board();
            PlacementManager();
            do
            {
                if (kingBlack.AvailableMoves().Count != 0)
                {
                    KingPosition();
                    if (kingBlack.FCoord == 1 || kingBlack.FCoord == 8)
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

            } while (kingBlack.FCoord != 1 || kingBlack.FCoord != 8);

            if (kingBlack.AvailableMoves().Count != 0)
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
            if (kingBlack.IsMove(tupl.let, tupl.num))
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
            if (kingBlack.IsMove(tupl.let, tupl.num))
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
            kingBlack.SetPosition(a, b);
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
            kingBlack.SetPosition(a, b);
            int figureRandom = new Random().Next(2, 4);
            var tuplQ = qu.AvailableMoves().EndPosition(a, b);
            var tuplR = ro.AvailableMoves().EndPosition(a, b);

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
            kingBlack.SetPosition(a, b);
            int figureRandom = new Random().Next(2, 4);
            var tuplQ = qu.AvailableMoves().EndPosition(a, b);
            var tuplR = ro.AvailableMoves().EndPosition(a, b);

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
