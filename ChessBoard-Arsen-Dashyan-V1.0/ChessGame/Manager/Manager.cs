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
        public static List<Model> models = new List<Model>();
        public static King kingBlack = new King("King", ConsoleColor.Red);
        public static Rook rookBlackL = new Rook("RookBlackL", ConsoleColor.Red);
        public static Rook rookL = new Rook("RookL", ConsoleColor.White);
        public static Rook rookR = new Rook("RookR", ConsoleColor.White);
        public static Bishop bishopL = new Bishop("BishopL", ConsoleColor.White);
        public static Bishop bishopR = new Bishop("BishopR", ConsoleColor.White);
        public static Queen queen = new Queen("Queen", ConsoleColor.White);
        public static King kingWhite = new King("King", ConsoleColor.White);
        public static Knights knight = new Knights("knight", ConsoleColor.White);
        //public static List<Point> movesOfBlackKing = kingBlack?.AvailableMoves().Where(c => !DangerPosition(kingBlack).Contains(c)).ToList();
        //public static List<Point> movesOfWhiteKing = kingWhite?.AvailableMoves().Where(c => !DangerPosition(kingWhite).Contains(c)).ToList();
        #endregion

        #region Chack The Dangerous Positions

        /// <summary>
        /// Chack tke king move
        /// </summary>
        /// <param name="i">Black king first coordinat</param>
        /// <param name="j">Black king second coordinat</param>
        /// <returns>Return true if king coordinat is a permissible</returns>
        private static bool IsKingAction(Point point) =>
           (InsideBord(point) && !GetPosition().Contains(point)
            && !DangerPosition(kingBlack).Contains(point)
            && kingBlack.IsMove(point) && (kingBlack.point?.X != point.X | kingBlack.point?.Y != point.Y));

        /// <summary>
        /// Created a positions wher can a white figures move
        /// </summary>
        /// <returns>Return the Available moves</returns>
        public static List<Point> DangerPosition(Model model)
        {
            List<Point> result = new List<Point>();
            var modelNew = models.Where(c => c.Color != model.Color);
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                var array = temp.AvailableMoves();
                result.AddRange(array);
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
        #endregion

        #region New Game Logic
        public static bool GetMinMovesWithShax(Model figur, out KeyValuePair<Point, (int, Model)> keyValuePair)
        {
            Dictionary<Point, (int, Model)> countList = new Dictionary<Point, (int, Model)>();
            IRandomeMove tempFigur = (IRandomeMove)figur;
            Point temp = figur.point;
            foreach (var item in tempFigur.AvailableMoves())
            {
                figur.point = item;
                if (Point.Modul(item, kingBlack.point) >= 2)
                {
                    if (!tempFigur.IsUnderAttack(figur.point))
                    {
                        if (tempFigur.AvailableMoves().Contains(kingBlack.point))
                            countList.Add(item, (kingBlack.AvailableMovesWithShaxAndBox().Count, figur));
                    }
                }
            }
            figur.point = temp;
            if (countList.Count != 0)
            {
                keyValuePair = countList.OrderBy(k => k.Value.Item1).FirstOrDefault();
                return true;
            }
            else
            {
                keyValuePair = new KeyValuePair<Point, (int, Model)>();
                return false;
            }
        }
        private static void MinCountWithShax()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color && !(c is King)).ToList();
            List<KeyValuePair<Point, (int, Model)>> list = new List<KeyValuePair<Point, (int, Model)>>();
            foreach (var figur in modelNew)
            {
                if (GetMinMovesWithShax(figur, out KeyValuePair<Point, (int, Model)> keyValuePair))
                    list.Add(keyValuePair);
            }
            if (list.Count != 0)
            {
                var minMoves = list.OrderBy(c => c.Value.Item1).FirstOrDefault();
                var maxMoves = list.OrderBy(c => c.Value.Item1).LastOrDefault();
                ISetPosition setFigurFirst = (ISetPosition)minMoves.Value.Item2;
                ISetPosition setFigurLast = (ISetPosition)maxMoves.Value.Item2;
                if (kingBlack.AvailableMovesWithShaxAndBox().Count == 1)
                {
                    IRandomeMove randomeMove = (IRandomeMove)setFigurFirst;
                    if (!randomeMove.IsUnderAttack(minMoves.Key, kingBlack.AvailableMovesWithShaxAndBox()[0]))
                    {
                        setFigurFirst.SetPosition(minMoves.Key);
                        DeleteFigur(minMoves.Value.Item2);
                    }
                    else
                    {
                        setFigurLast.SetPosition(maxMoves.Key);
                        DeleteFigur(maxMoves.Value.Item2);
                    }
                }
                else
                {
                    setFigurFirst.SetPosition(minMoves.Key);
                    DeleteFigur(minMoves.Value.Item2);
                }
            }
            else
            {
                var randomFigur = modelNew[new Random().Next(0, modelNew.Count)];
                IRandomeMove setFigur = (IRandomeMove)randomFigur;
                Point point1 = setFigur.RandomMove(kingBlack);
                randomFigur.SetPosition(point1);
                DeleteFigur(randomFigur);
            }
        }
        private static void AntiBabyGame()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color).ToList();
            List<(double, Model)> destination = new List<(double, Model)>();
            foreach (var figur in modelNew)
            {
                double tempDestination = Point.Modul(figur.point, kingBlack.point);
                destination.Add((tempDestination, figur));
            }
            (double, Model) max = destination.OrderBy(k => k.Item1).FirstOrDefault();
            IRandomeMove tempFigur = (IRandomeMove)max.Item2;
            max.Item2.SetPosition(tempFigur.RandomMove(kingBlack));
            DeleteFigur(max.Item2);
        }
        private static bool UnderAttack()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color).ToList();
            foreach (var figur in modelNew)
            {
                IRandomeMove tempFigur = (IRandomeMove)figur;
                if (tempFigur.IsUnderAttack(figur.point))
                {
                    if (!tempFigur.IsProtected())
                    {
                        Point point1 = tempFigur.RandomMove(kingBlack);
                        figur.SetPosition(point1);
                        DeleteFigur(figur);
                        return true;
                    }
                }
            }
            return false;
        }
        public static void Play()
        {
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
                        MinCountWithShax();
                    else
                        AntiBabyGame();
                }
            }
            Console.SetCursorPosition(40, 8);
            Console.WriteLine("Game over");
        }

        #region Box and Shax
        public static bool GetMinMovesWithBox(Model figur, out KeyValuePair<Point, (int, Model)> keyValuePair)
        {
            Dictionary<Point, (int, Model)> countList = new Dictionary<Point, (int, Model)>();
            IRandomeMove tempFigur = (IRandomeMove)figur;
            Point temp = figur.point;
            foreach (var item in tempFigur.AvailableMoves())
            {
                figur.point = item;
                if (Point.Modul(item, kingBlack.point) >= 2)
                {
                    if (!tempFigur.IsUnderAttack(figur.point))
                        countList.Add(item, (kingBlack.AvailableMovesWithShaxAndBox().Count, figur));
                }
            }
            figur.point = temp;
            if (countList.Count != 0)
            {
                keyValuePair = countList.OrderBy(k => k.Value.Item1).FirstOrDefault();
                return true;
            }
            else
            {
                keyValuePair = new KeyValuePair<Point, (int, Model)>();
                return false;
            }
        }
        private static void MinCountWithBox()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color).Reverse().ToList();
            List<KeyValuePair<Point, (int, Model)>> list = new List<KeyValuePair<Point, (int, Model)>>();
            foreach (var figur in modelNew)
            {
                if (GetMinMovesWithBox(figur, out KeyValuePair<Point, (int, Model)> keyValuePair))
                    list.Add(keyValuePair);
            }
            list = list.Where(c => c.Value.Item1 != 0).ToList();
            var minMoves = list.OrderBy(c => c.Value.Item1).FirstOrDefault();
            var maxMoves = list.OrderBy(c => c.Value.Item1).LastOrDefault();
            ISetPosition setFigurFirst = (ISetPosition)minMoves.Value.Item2;
            ISetPosition setFigurLast = (ISetPosition)maxMoves.Value.Item2;
            if (kingBlack.AvailableMoves().Count == 1)
            {
                IRandomeMove randomeMove = (IRandomeMove)setFigurFirst;
                if (!randomeMove.IsUnderAttack(minMoves.Key, kingBlack.AvailableMovesWithShaxAndBox()[0]))
                {
                    setFigurFirst.SetPosition(minMoves.Key);
                    DeleteFigur(minMoves.Value.Item2);
                }
                else
                {
                    setFigurLast.SetPosition(maxMoves.Key);
                    DeleteFigur(maxMoves.Value.Item2);
                }
            }
            else
            {
                setFigurFirst.SetPosition(minMoves.Key);
                DeleteFigur(minMoves.Value.Item2);
            }
        }
        public static void PlayNewWithShaxAndBox()
        {
            PlacementManager();
            List<Point> list = new List<Point>();
            
            while (kingBlack.AvailableMovesWithShaxAndBox().Count != 0)
            {
                if (kingBlack.AvailableMovesWithShaxAndBox().Count > 1)
                {
                    while (kingBlack.AvailableMovesWithShaxAndBox().Count != 1)
                    {
                        View.ClearText();
                        Console.SetCursorPosition(40, 0);
                        ShowBlackFigurs();
                        Console.SetCursorPosition(40, 1);
                        var temp = Console.ReadLine();
                        var point = InputCoordinatsForGame(temp);
                        list.Add(point);
                        ISetPosition blackFigur = (ISetPosition)StringToModelForBlack(temp);
                        blackFigur.SetPosition(point);
                        DeleteFigur(StringToModelForBlack(temp));
                        if (!UnderAttack())
                        {
                            if (!list.BabyGame())
                                MinCountWithBox();
                            else
                                AntiBabyGame();
                        }
                    }
                }
                else
                {
                    View.ClearText();
                    Console.SetCursorPosition(40, 0);
                    ShowBlackFigurs();
                    Console.SetCursorPosition(40, 1);
                    var temp = Console.ReadLine();
                    var point = InputCoordinatsForGame(temp);
                    list.Add(point);
                    ISetPosition blackFigur = (ISetPosition)StringToModelForBlack(temp);
                    blackFigur.SetPosition(point);
                    DeleteFigur(StringToModelForBlack(temp));
                    if (!UnderAttack())
                    {
                        if (!list.BabyGame())
                            MinCountWithShax();
                        else
                            AntiBabyGame();
                    }
                }
            }
            Console.SetCursorPosition(40, 8);
            Console.WriteLine("Game over");
        }
        public static void DeleteFigur(Model model)
        {
            var modelTemp = models.Where(c => c.Color != model.Color);
            foreach (var item in modelTemp)
            {
                if (model.point.X == item.point.X && model.point.Y == item.point.Y)
                {
                    models.Remove(item);
                    break;
                }
            }
        }
        public static void ShowBlackFigurs()
        {
            var modelsBlack = models.Where(c => c.Color == ConsoleColor.Red).ToList();
            Console.Write("Please enter a Black figur ");
            foreach (var item in modelsBlack)
            {
                Console.Write(item.Name + " ");
            }
        }
        #endregion

        #endregion
    }
}
