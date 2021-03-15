using System;
using System.Collections.Generic;
using Utility;
using Coordinats;
using System.Linq;

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
        public static List<Model> models = new List<Model>();
        public static List<Point> positions = new List<Point>()
        {queen.point,rookL?.point,rookR?.point,kingBlack.point,kingWhite.point,knight.point };
        #endregion

        #region Chack The Dangerous Positions

        /// <summary>
        /// Chack tke king move
        /// </summary>
        /// <param name="i">Black king first coordinat</param>
        /// <param name="j">Black king second coordinat</param>
        /// <returns>Return true if king coordinat is a permissible</returns>
        private static bool IsKingAction(Point point) =>
           (InsideBord(point) && !positions.Contains(point)
            && !DangerousPosition().Contains(point)
            && WhiteKingOcupancy(point)
            && kingBlack.point != point);

        /// <summary>
        /// Created a positions whithe wer can a white figures move
        /// </summary>
        /// <returns>Return the Available moves</returns>
        public static List<Point> DangerousPosition()
        {
            List<Point> result = new List<Point>();
            var modelNew = models.Where(c => !(c is King));
            foreach (var item in modelNew)
            {
                if (item is Queen queen)
                {
                    var array = queen.AvailableMoves();
                    result.AddRange(array);
                }
                if (item is Rook rook)
                {
                    var arrayR = rook.AvailableMoves();
                    result.AddRange(arrayR);
                }
            }
            return result;
        }

        /// <summary>
        /// Chack the coordinat inside bord or no
        /// </summary>
        /// <param name="i">First Coordinat</param>
        /// <param name="j">Second Coordinat</param>
        /// <returns>Return true when input coordinats inside bord</returns>
        private static bool InsideBord(Point point) =>
           (point.X <= leftSize && point.X >= rightSize && point.Y >= rightSize && point.Y <= leftSize);

        /// <summary>
        /// Chack black king and white king positions
        /// </summary>
        /// <param name="i">Black king first coordinat</param>
        /// <param name="j">Black king second coordinat</param>
        /// <returns>Return true when black and white king away</returns>
        private static bool WhiteKingOcupancy(Point point) =>
            (Math.Abs(point.X - kingWhite.point.X) > 1 || Math.Abs(point.Y - kingWhite.point.Y) > 1);
        #endregion

        #region Random Game Logic
        public static bool NextTo(Point point, int versia)
        {
            var modelNew = models.Where(c => !(c is King)).ToList();
            if (!MoveNextTo(modelNew, point, versia))
            {
                Point point1 = queen.AvailableMoves().RandomMove();
                queen.SetPosition(point1);
            }
            return false;
        }
        public static bool OnFigur(Point point, int versia)
        {
            var modelNew = models.Where(c => !(c is King)).ToList();
            Model temp = null;
            foreach (var item in modelNew)
            {
                if (item.point.X == kingBlack.point.X + 1)
                {
                    temp = item;
                }
            }
            modelNew.Remove(temp);
            if (!MoveOnFigur(modelNew, point, versia))
            {
                Point point1 = queen.AvailableMoves().RandomMove();
                queen.SetPosition(point1);
            }
            return true;
        }
        public static bool MoveOnFigur(List<Model> modelNew, Point point, int versia)
        {
            bool isMove = false;
            foreach (var item in modelNew)
            {
                if (item is Queen queen)
                {
                    Point tuplQ = queen.AvailableMoves().WhenFirstHalfOn(point, versia);
                    if (tuplQ != null)
                    {
                        queen.SetPosition(tuplQ);
                        isMove = true;
                        break;
                    }
                }
                if (ReferenceEquals(item, rookL))
                {
                    Point tuplL = rookL.AvailableMoves().WhenFirstHalfOn(point, versia);
                    if (tuplL != null)
                    {
                        rookL.SetPosition(tuplL);
                        isMove = true;
                        break;
                    }
                }
                if (ReferenceEquals(item, rookR))
                {
                    Point tuplR = rookR.AvailableMoves().WhenFirstHalfOn(point, versia);
                    if (tuplR != null)
                    {
                        rookR.SetPosition(tuplR);
                        isMove = true;
                        break;
                    }
                }
            }
            return isMove;
        }
        public static bool MoveNextTo(List<Model> modelNew, Point point, int versia)
        {
            bool isMove = false;
            foreach (var item in modelNew)
            {
                if (item is Queen queen)
                {
                    Point tuplQ = queen.AvailableMoves().WhenFirstHalf(point, versia);
                    if (tuplQ != null)
                    {
                        queen.SetPosition(tuplQ);
                        isMove = true;
                        break;
                    }
                }
                if (ReferenceEquals(item, rookL))
                {
                    Point tuplL = rookL.AvailableMoves().WhenFirstHalf(point, versia);
                    if (tuplL != null)
                    {
                        rookL.SetPosition(tuplL);
                        isMove = true;
                        break;
                    }
                }
                if (ReferenceEquals(item, rookR))
                {
                    Point tuplR = rookR.AvailableMoves().WhenFirstHalf(point, versia);
                    if (tuplR != null)
                    {
                        rookL.SetPosition(tuplR);
                        isMove = true;
                        break;
                    }
                }
            }
            return isMove;
        }
        public static void PlayNew()
        {
            View.Board();
            PlacementManager();
            bool isRun = true;
            while (kingBlack.AvailableMoves().Count != 0)
            { 
                View.ClearText();
                var point = InputCoordinats("Black", "King");
                kingBlack.SetPosition(point);
                if (point.X <= 4)
                {
                    if (kingBlack.IsMove(point))
                    {
                        if (isRun)
                            isRun = NextTo(point, 1);
                        else
                            isRun = OnFigur(point, 1);
                    }
                }
                else
                {
                    if (kingBlack.IsMove(point))
                    {
                        if (isRun)
                            isRun = NextTo(point, 2);
                        else
                            isRun = OnFigur(point, 2);
                    }
                }
            } 
            Console.SetCursorPosition(40, 8);
            Console.WriteLine("Game over");
        }
        #endregion
    }
}
