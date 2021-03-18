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
            && WhiteKingOcupancy(point) && kingBlack.IsMove(point)
            && (kingBlack.point?.X != point.X | kingBlack.point?.Y != point.Y));

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
                if (item is Knights)
                {
                    var arrayKnight = knight.AvailableMoves();
                    result.AddRange(arrayKnight);
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

        #region New Game Logic
        private static void KingMinCount()
        {
            var modelNew = models.Where(c => !(c is King)).ToList();
            Dictionary<Point, (int, Model)> countListQ = new Dictionary<Point, (int, Model)>();
            Dictionary<Point, (int, Model)> countListRL = new Dictionary<Point, (int, Model)>();
            Dictionary<Point, (int, Model)> countListRR = new Dictionary<Point, (int, Model)>();
            Dictionary<Point, (int, Model)> countListKnight = new Dictionary<Point, (int, Model)>();
            foreach (var figur in modelNew)
            {
                if (figur is Queen queen)
                {
                    Point temp = queen.point;
                    foreach (var item in queen.AvailableMoves())
                    {
                        queen.point = item;
                        if (Point.Modul(item, kingBlack.point) >= 2)
                        {
                            if (queen.AvailableMoves().Contains(kingBlack.point))
                            {
                                countListQ.Add(item, (kingBlack.AvailableMoves().Count, queen));
                            }
                        }
                    }
                    queen.point = temp;
                }
                if (ReferenceEquals(figur, rookL))
                {
                    Point temp = rookL.point;
                    foreach (var item in rookL.AvailableMoves())
                    {
                        rookL.point = item;
                        if (Point.Modul(item, kingBlack.point) >= 2)
                        {
                            if (rookL.AvailableMoves().Contains(kingBlack.point))
                            {
                                countListRL.Add(item, (kingBlack.AvailableMoves().Count, rookL));
                            }
                        }
                    }
                    rookL.point = temp;
                }
                if (ReferenceEquals(figur, rookR))
                {
                    Point temp = rookR.point;
                    foreach (var item in rookR.AvailableMoves())
                    {
                        rookR.point = item;
                        if (Point.Modul(item, kingBlack.point) >= 2)
                        {
                            if (rookR.AvailableMoves().Contains(kingBlack.point))
                            {
                                countListRR.Add(item, (kingBlack.AvailableMoves().Count, rookR));
                            }
                        }
                    }
                    rookR.point = temp;
                }
                if (ReferenceEquals(figur, knight))
                {
                    Point temp = knight.point;
                    foreach (var item in knight.AvailableMoves())
                    {
                        knight.point = item;
                        if (Point.Modul(item, kingBlack.point) >= 2)
                        {
                            if (knight.AvailableMoves().Contains(kingBlack.point))
                            {
                                countListRR.Add(item, (kingBlack.AvailableMoves().Count, knight));
                            }
                        }
                    }
                    knight.point = temp;
                }
            }
            List<KeyValuePair<Point, (int, Model)>> listEnd = new List<KeyValuePair<Point, (int, Model)>>();
            KeyValuePair<Point, (int, Model)> queenMin;
            KeyValuePair<Point, (int, Model)> rookLMin;
            KeyValuePair<Point, (int, Model)> rookRMin;
            KeyValuePair<Point, (int, Model)> knightMin;
            if (countListQ.Count != 0)
            {
                queenMin = countListQ.OrderBy(k => k.Value.Item1).FirstOrDefault();
                listEnd.Add(queenMin);
            }
            if (countListRL.Count != 0)
            {
                rookLMin = countListRL.OrderBy(k => k.Value.Item1).FirstOrDefault();
                listEnd.Add(rookLMin);
            }
            if (countListRR.Count != 0)
            {
                rookRMin = countListRR.OrderBy(k => k.Value.Item1).FirstOrDefault();
                listEnd.Add(rookRMin);
            }
            if (countListKnight.Count != 0)
            {
                knightMin = countListKnight.OrderBy(k => k.Value.Item1).FirstOrDefault();
                listEnd.Add(knightMin);
            }
            var minMoves = listEnd.OrderBy(c => c.Value.Item1).First();
            if (minMoves.Value.Item2 is Queen)
            {
                queen.SetPosition(minMoves.Key);
            }
            else if (ReferenceEquals(minMoves.Value.Item2, rookL))
            {
                rookL.SetPosition(minMoves.Key);
            }
            else if (ReferenceEquals(minMoves.Value.Item2, rookR))
            {
                rookR.SetPosition(minMoves.Key);
            }
            else if (ReferenceEquals(minMoves.Value.Item2, knight))
            {
                knight.SetPosition(minMoves.Key);
            }
        }
        private static void AntiBabyGame()
        {
            var modelNew = models.Where(c => !(c is King)).ToList();
            List<(int, Model)> destination = new List<(int, Model)>();
            int queenDestination = 0;
            int rookLDestination = 0;
            int rookRDestination = 0;
            int knightDestination = 0;
            foreach (var figur in modelNew)
            {
                if (figur is Queen queen)
                {
                    queenDestination = Point.Modul(queen.point, kingBlack.point);
                    destination.Add((queenDestination, queen));
                }
                if (ReferenceEquals(figur, rookL))
                {
                    rookLDestination = Point.Modul(rookL.point, kingBlack.point);
                    destination.Add((rookLDestination, rookL));
                }
                if (ReferenceEquals(figur, rookR))
                {
                    rookRDestination = Point.Modul(rookR.point, kingBlack.point);
                    destination.Add((rookRDestination, rookR));
                }
                if (ReferenceEquals(figur, knight))
                {
                    knightDestination = Point.Modul(knight.point, kingBlack.point);
                    destination.Add((knightDestination, knight));
                }
            }
            (int,Model) max = destination.OrderBy(k => k.Item1).LastOrDefault();
            if (max.Item2 is Queen)
            {
                queen.SetPosition(queen.RandomMove(kingBlack));
            }
            else if (ReferenceEquals(max.Item2, rookL))
            {
                rookL.SetPosition(rookL.RandomMove(kingBlack));
            }
            else if (ReferenceEquals(max.Item2, rookR))
            {
                rookR.SetPosition(rookR.RandomMove(kingBlack));
            }
            else if (ReferenceEquals(max.Item2, knight))
            {
                knight.SetPosition(knight.RandomMove(kingBlack));
            }
        }
        private static bool UnderAttack()
        {
            var modelNew = models.Where(c => !(c is King)).ToList();
            foreach (var figur in modelNew)
            {
                if (figur is Queen queen)
                {
                    if (queen.IsUnderAttack(kingBlack))
                    {
                        if (!queen.IsProtected())
                        {
                            Point point1 = queen.RandomMove(kingBlack);
                            queen.SetPosition(point1);
                            return true;
                        }
                    }
                }
                if (ReferenceEquals(figur, rookL))
                {
                    if (rookL.IsUnderAttack(kingBlack))
                    {
                        if (!rookL.IsProtected())
                        {
                            Point point1 = rookL.RandomMove(kingBlack);
                            rookL.SetPosition(point1);
                            return true;
                        }
                    }
                }
                if (ReferenceEquals(figur, rookR))
                {
                    if (rookR.IsUnderAttack(kingBlack))
                    {
                        if (!rookR.IsProtected())
                        {
                            Point point1 = rookR.RandomMove(kingBlack);
                            rookL.SetPosition(point1);
                            return true;
                        }
                    }
                }
                if (ReferenceEquals(figur, knight))
                {
                    if (knight.IsUnderAttack(kingBlack))
                    {
                        if (!knight.IsProtected())
                        {
                            Point point1 = knight.RandomMove(kingBlack);
                            knight.SetPosition(point1);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static void Play()
        {
            View.Board();
            PlacementManager();
            List<Point> list = new List<Point>();
            while (kingBlack.AvailableMoves().Count != 0)
            {
                View.ClearText();
                var point = InputCoordinats("Black", "King");
                list.Add(point);
                kingBlack.SetPosition(point);
                if (!UnderAttack())
                {
                    if (!list.BabyGame())
                        KingMinCount();
                    else
                        AntiBabyGame();
                }
            }
            Console.SetCursorPosition(40, 8);
            Console.WriteLine("Game over");
        }
        #endregion

        #region Draft
        // Tagavori xodery sahmanapakox etod
        //private static void KingMinCount(List<Model> models)
        //{
        //    var modelNew = models.Where(c => !(c is King)).ToList();
        //    Dictionary<Point, (int, Model)> countListQ = new Dictionary<Point, (int, Model)>();
        //    Dictionary<Point, (int, Model)> countListRL = new Dictionary<Point, (int, Model)>();
        //    Dictionary<Point, (int, Model)> countListRR = new Dictionary<Point, (int, Model)>();
        //    foreach (var figur in modelNew)
        //    {
        //        if (figur is Queen queen)
        //        {
        //            Point temp = queen.point;
        //            var list = queen.AvailableMoves();
        //            foreach (var item in list)
        //            {
        //                queen.point = item;
        //                if (Math.Sqrt((item.X - kingBlack.point.X) * (item.X - kingBlack.point.X) +
        //                    (item.Y - kingBlack.point.Y) * (item.Y - kingBlack.point.Y)) > 2)
        //                {
        //                    countListQ.Add(item, (kingBlack.AvailableMoves().Count, queen));
        //                }
        //            }
        //            queen.point = temp;
        //        }
        //        if (ReferenceEquals(figur, rookL))
        //        {
        //            Point temp = rookL.point;
        //            var listRookL = rookL.AvailableMoves();
        //            foreach (var item in listRookL)
        //            {
        //                rookL.point = item;
        //                if (Math.Sqrt((item.X - kingBlack.point.X) * (item.X - kingBlack.point.X) +
        //                    (item.Y - kingBlack.point.Y) * (item.Y - kingBlack.point.Y)) > 2)
        //                {
        //                    countListRL.Add(item, (kingBlack.AvailableMoves().Count, rookL));
        //                }
        //            }
        //            rookL.point = temp;
        //        }
        //        if (ReferenceEquals(figur, rookR))
        //        {
        //            Point temp = rookR.point;
        //            var listRookR = rookR.AvailableMoves();
        //            foreach (var item in listRookR)
        //            {
        //                rookR.point = item;
        //                if (Math.Sqrt((item.X - kingBlack.point.X) * (item.X - kingBlack.point.X) +
        //                    (item.Y - kingBlack.point.Y) * (item.Y - kingBlack.point.Y)) > 2)
        //                {
        //                    countListRR.Add(item, (kingBlack.AvailableMoves().Count, rookR));
        //                }
        //            }
        //            rookR.point = temp;
        //        }
        //    }
        //    List<KeyValuePair<Point, (int, Model)>> listEnd = new List<KeyValuePair<Point, (int, Model)>>();
        //    KeyValuePair<Point, (int, Model)> queenMin;
        //    KeyValuePair<Point, (int, Model)> rookLMin;
        //    KeyValuePair<Point, (int, Model)> rookRMin;
        //    if (countListQ.Count != 0)
        //    {
        //        queenMin = countListQ.OrderBy(k => k.Value.Item1).FirstOrDefault();
        //        listEnd.Add(queenMin);
        //    }
        //    if (countListRL.Count != 0)
        //    {
        //        rookLMin = countListRL.OrderBy(k => k.Value.Item1).FirstOrDefault();
        //        listEnd.Add(rookLMin);
        //    }
        //    if (countListRR.Count != 0)
        //    {
        //        rookRMin = countListRR.OrderBy(k => k.Value.Item1).FirstOrDefault();
        //        listEnd.Add(rookRMin);
        //    }
        //    var min = listEnd.OrderBy(c => c.Value.Item1).First();
        //    if (min.Value.Item2 is Queen)
        //    {
        //        queen.SetPosition(min.Key);
        //    }
        //    else if (ReferenceEquals(min.Value.Item2, rookL))
        //    {
        //        rookL.SetPosition(min.Key);
        //    }
        //    else if (ReferenceEquals(min.Value.Item2, rookL))
        //    {
        //        rookR.SetPosition(min.Key);
        //    }
        //}

        //Tagavori dirqin hamapatasxn sahmanapakum
        //private static void KingPostionCount(List<Model> models)
        //{
        //    var modelNew = models.Where(c => !(c is King)).ToList();
        //    var listOfKing = kingBlack.point.KingAvailableMoves();
        //    foreach (var figur in modelNew)
        //    {
        //        if (figur is Queen queen)
        //        {
        //            var list = queen.AvailableMoves();
        //            Point temp = null;
        //            Point tempfigur = queen.point;
        //            foreach (var item in list)
        //            {
        //                queen.point = item;
        //                if (Math.Sqrt((item.X - kingBlack.point.X) * (item.X - kingBlack.point.X) +
        //                    (item.Y - kingBlack.point.Y) * (item.Y - kingBlack.point.Y)) == 2)
        //                {
        //                    foreach (var moves in listOfKing)
        //                    {
        //                        if (kingBlack.AvailableMoves().Contains(moves))
        //                        {
        //                            temp = item;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            queen.point = tempfigur;
        //            if (temp != null)
        //            {
        //                queen.SetPosition(temp);
        //                break;
        //            }

        //        }
        //        if (ReferenceEquals(figur, rookL))
        //        {
        //            Point temp = null;
        //            Point tempfigur = rookL.point;
        //            var listRookL = rookL.AvailableMoves();
        //            foreach (var item in listRookL)
        //            {
        //                rookL.point = item;
        //                if (Math.Sqrt((item.X - kingBlack.point.X) * (item.X - kingBlack.point.X) +
        //                    (item.Y - kingBlack.point.Y) * (item.Y - kingBlack.point.Y)) == 2)
        //                {
        //                    foreach (var moves in listOfKing)
        //                    {
        //                        if (kingBlack.AvailableMoves().Contains(moves))
        //                        {
        //                            temp = item;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            rookL.point = tempfigur;
        //            if (temp != null)
        //            {
        //                rookL.SetPosition(temp);
        //                break;
        //            }
        //        }
        //        if (ReferenceEquals(figur, rookR))
        //        {
        //            var listRookR = rookR.AvailableMoves();
        //            Point temp = null;
        //            Point tempfigur = rookL.point;
        //            foreach (var item in listRookR)
        //            {
        //                rookR.point = item;
        //                if (Math.Sqrt((item.X - kingBlack.point.X) * (item.X - kingBlack.point.X) +
        //                    (item.Y - kingBlack.point.Y) * (item.Y - kingBlack.point.Y)) == 2)
        //                {
        //                    foreach (var moves in listOfKing)
        //                    {
        //                        if (kingBlack.AvailableMoves().Contains(moves))
        //                        {
        //                            temp = item;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            rookR.point = tempfigur;
        //            if (temp != null)
        //            {
        //                rookR.SetPosition(temp);
        //                break;
        //            }
        //        }
        //    }
        //}

        #region Random Game Logic
        //public static bool NextTo(Point point, int versia)
        //{
        //    var modelNew = models.Where(c => !(c is King)).ToList();
        //    if (!MoveNextTo(modelNew, point, versia))
        //    {
        //        Point point1 = queen.RandomMove(kingBlack);
        //        queen.SetPosition(point1);
        //    }
        //    return false;
        //}
        //public static bool OnFigur(Point point, int versia)
        //{
        //    var modelNew = models.Where(c => !(c is King)).ToList();
        //    Model temp = null;
        //    foreach (var item in modelNew)
        //    {
        //        if (item.point.X == kingBlack.point.X + 1)
        //        {
        //            temp = item;
        //        }
        //    }
        //    modelNew.Remove(temp);
        //    if (!MoveOnFigur(modelNew, point, versia))
        //    {
        //        Point point1 = queen.RandomMove(kingBlack);
        //        queen.SetPosition(point1);
        //    }
        //    return true;
        //}
        //public static bool MoveOnFigur(List<Model> modelNew, Point point, int versia)
        //{
        //    bool isMove = false;
        //    foreach (var item in modelNew)
        //    {
        //        if (item is Queen queen)
        //        {
        //            Point tuplQ = queen.AvailableMoves().WhenFirstHalfOn(point, versia);
        //            if (tuplQ != null)
        //            {
        //                queen.SetPosition(tuplQ);
        //                isMove = true;
        //                break;
        //            }
        //        }
        //        if (ReferenceEquals(item, rookL))
        //        {
        //            Point tuplL = rookL.AvailableMoves().WhenFirstHalfOn(point, versia);
        //            if (tuplL != null)
        //            {
        //                rookL.SetPosition(tuplL);
        //                isMove = true;
        //                break;
        //            }
        //        }
        //        if (ReferenceEquals(item, rookR))
        //        {
        //            Point tuplR = rookR.AvailableMoves().WhenFirstHalfOn(point, versia);
        //            if (tuplR != null)
        //            {
        //                rookR.SetPosition(tuplR);
        //                isMove = true;
        //                break;
        //            }
        //        }
        //    }
        //    return isMove;
        //}
        //public static bool MoveNextTo(List<Model> modelNew, Point point, int versia)
        //{
        //    bool isMove = false;
        //    foreach (var item in modelNew)
        //    {
        //        if (item is Queen queen)
        //        {
        //            Point tuplQ = queen.AvailableMoves().WhenFirstHalf(point, versia);
        //            if (tuplQ != null)
        //            {
        //                queen.SetPosition(tuplQ);
        //                isMove = true;
        //                break;
        //            }
        //        }
        //        if (ReferenceEquals(item, rookL))
        //        {
        //            Point tuplL = rookL.AvailableMoves().WhenFirstHalf(point, versia);
        //            if (tuplL != null)
        //            {
        //                rookL.SetPosition(tuplL);
        //                isMove = true;
        //                break;
        //            }
        //        }
        //        if (ReferenceEquals(item, rookR))
        //        {
        //            Point tuplR = rookR.AvailableMoves().WhenFirstHalf(point, versia);
        //            if (tuplR != null)
        //            {
        //                rookL.SetPosition(tuplR);
        //                isMove = true;
        //                break;
        //            }
        //        }
        //    }
        //    return isMove;
        //}
        //public static void Play()
        //{
        //    View.Board();
        //    PlacementManager();
        //    bool isRun = true;
        //    while (kingBlack.AvailableMoves().Count != 0)
        //    {
        //        View.ClearText();
        //        var point = InputCoordinats("Black", "King");
        //        kingBlack.SetPosition(point);
        //        if (point.X <= 4)
        //        {
        //            if (kingBlack.IsMove(point))
        //            {
        //                if (isRun)
        //                    isRun = NextTo(point, 1);
        //                else
        //                    isRun = OnFigur(point, 1);
        //            }
        //        }
        //        else
        //        {
        //            if (kingBlack.IsMove(point))
        //            {
        //                if (isRun)
        //                    isRun = NextTo(point, 2);
        //                else
        //                    isRun = OnFigur(point, 2);
        //            }
        //        }
        //    }
        //    Console.SetCursorPosition(40, 8);
        //    Console.WriteLine("Game over");
        //}
        #endregion
        #endregion
    }
}
