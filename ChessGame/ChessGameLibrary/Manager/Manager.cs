using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGameLibrary
{
    class Manager
    {
        #region Property and Feld
        private const int leftSize = 8;
        private const int rightSize = 1;
        private readonly IChessBoard board;
        public int count = 0;
        public List<FigureBase> models = new List<FigureBase>();
        public King kingBlack = new King("King", ConsoleColor.Red);
        public Rook rookBlackL = new Rook("RookBlackL", ConsoleColor.Red);
        public Rook rookL = new Rook("RookL", ConsoleColor.White);
        public Rook rookR = new Rook("RookR", ConsoleColor.White);
        public Bishop bishopL = new Bishop("BishopL", ConsoleColor.White);
        public Bishop bishopR = new Bishop("BishopR", ConsoleColor.White);
        public Queen queen = new Queen("Queen", ConsoleColor.White);
        public King kingWhite = new King("King", ConsoleColor.White);
        public Knights knight = new Knights("knight", ConsoleColor.White);
        public List<Point> movesOfBlackKing = kingBlack.AvailableMoves().Where(c => !DangerPosition(kingBlack).Contains(c)).ToList();
        #endregion

        public Manager(IChessBoard board)
        {
            this.board = board;
        }
                

        #region Chack The Dangerous Positions

        /// <summary>
        /// Chack tke king move
        /// </summary>
        /// <param name="i">Black king first coordinat</param>
        /// <param name="j">Black king second coordinat</param>
        /// <returns>Return true if king coordinat is a permissible</returns>
        private bool IsKingAction(Point point) =>
           (InsideBord(point) && !GetPosition().Contains(point)
            && !DangerPosition(kingBlack).Contains(point)
            && kingBlack.IsMove(point) && kingBlack.Coordinate != point);

        /// <summary>
        /// Created a positions wher can a white figures move
        /// </summary>
        /// <returns>Return the Available moves</returns>
        public List<Point> DangerPosition(FigureBase model)
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
        private bool InsideBord(Point point) =>
           (point.X <= leftSize && point.X >= rightSize && point.Y >= rightSize && point.Y <= leftSize);
        #endregion

        #region New Game Logic
        public bool GetMinMovesWithShax(FigureBase figur, out KeyValuePair<Point, (int, FigureBase)> keyValuePair)
        {
            Dictionary<Point, (int, FigureBase)> countList = new Dictionary<Point, (int, FigureBase)>();
            IRandomMove tempFigur = (IRandomMove)figur;
            Point temp = figur.Coordinate;
            foreach (var item in tempFigur.AvailableMoves())
            {
                figur.Coordinate = item;
                if (Point.Modul(item, kingBlack.Coordinate) >= 2)
                {
                    if (!tempFigur.IsUnderAttack(figur.Coordinate))
                    {
                        if (tempFigur.AvailableMoves().Contains(kingBlack.Coordinate))
                            countList.Add(item, (movesOfBlackKing.Count, figur));
                    }
                }
            }
            figur.Coordinate = temp;
            if (countList.Count != 0)
            {
                keyValuePair = countList.OrderBy(k => k.Value.Item1).FirstOrDefault();
                return true;
            }
            else
            {
                keyValuePair = new KeyValuePair<Point, (int, FigureBase)>();
                return false;
            }
        }
        private void MinCountWithShax()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color && !(c is King)).ToList();
            List<KeyValuePair<Point, (int, FigureBase)>> list = new List<KeyValuePair<Point, (int, FigureBase)>>();
            foreach (var figur in modelNew)
            {
                if (GetMinMovesWithShax(figur, out KeyValuePair<Point, (int, FigureBase)> keyValuePair))
                    list.Add(keyValuePair);
            }
            if (list.Count != 0)
            {
                var minMoves = list.OrderBy(c => c.Value.Item1).FirstOrDefault();
                var maxMoves = list.OrderBy(c => c.Value.Item1).LastOrDefault();
                ISetPosition setFigurFirst = (ISetPosition)minMoves.Value.Item2;
                ISetPosition setFigurLast = (ISetPosition)maxMoves.Value.Item2;
                if (movesOfBlackKing.Count == 1)
                {
                    IRandomMove randomeMove = (IRandomMove)setFigurFirst;
                    if (!randomeMove.IsUnderAttack(minMoves.Key, movesOfBlackKing[0]))
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
                IRandomMove setFigur = (IRandomMove)randomFigur;
                Point point1 = setFigur.RandomMove(kingBlack);
                this randomFigur.SetPosition(point1);
                DeleteFigur(randomFigur);
            }
        }
        private void AntiBabyGame()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color).ToList();
            List<(double, FigureBase)> destination = new List<(double, FigureBase)>();
            foreach (var figur in modelNew)
            {
                double tempDestination = Point.Modul(figur.Coordinate, kingBlack.Coordinate);
                destination.Add((tempDestination, figur));
            }
            (double, FigureBase) max = destination.OrderBy(k => k.Item1).FirstOrDefault();
            IRandomMove tempFigur = (IRandomMove)max.Item2;
            max.Item2.SetPosition(tempFigur.RandomMove(kingBlack));
            DeleteFigur(max.Item2);
        }
        private bool UnderAttack()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color).ToList();
            foreach (var figur in modelNew)
            {
                IRandomMove tempFigur = (IRandomMove)figur;
                if (tempFigur.IsUnderAttack(figur.Coordinate))
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
        public void Play()
        {
            PlacementManager();
            List<Point> list = new List<Point>();
            while (movesOfBlackKing.Count != 0)
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
        public static bool GetMinMovesWithBox(FigureBase figur, out KeyValuePair<Point, (int, FigureBase)> keyValuePair)
        {
            Dictionary<Point, (int, FigureBase)> countList = new Dictionary<Point, (int, FigureBase)>();
            IRandomMove tempFigur = (IRandomMove)figur;
            Point temp = figur.Coordinate;
            foreach (var item in tempFigur.AvailableMoves())
            {
                figur.Coordinate = item;
                if (Point.Modul(item, kingBlack.Coordinate) >= 2)
                {
                    if (!tempFigur.IsUnderAttack(figur.Coordinate))
                        countList.Add(item, (movesOfBlackKing.Count, figur));
                }
            }
            figur.Coordinate = temp;
            if (countList.Count != 0)
            {
                keyValuePair = countList.OrderBy(k => k.Value.Item1).FirstOrDefault();
                return true;
            }
            else
            {
                keyValuePair = new KeyValuePair<Point, (int, FigureBase)>();
                return false;
            }
        }
        private static void MinCountWithBox()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color).Reverse().ToList();
            List<KeyValuePair<Point, (int, FigureBase)>> list = new List<KeyValuePair<Point, (int, FigureBase)>>();
            foreach (var figur in modelNew)
            {
                if (GetMinMovesWithBox(figur, out KeyValuePair<Point, (int, FigureBase)> keyValuePair))
                    list.Add(keyValuePair);
            }
            list = list.Where(c => c.Value.Item1 != 0).ToList();
            var minMoves = list.OrderBy(c => c.Value.Item1).FirstOrDefault();
            var maxMoves = list.OrderBy(c => c.Value.Item1).LastOrDefault();
            ISetPosition setFigurFirst = (ISetPosition)minMoves.Value.Item2;
            ISetPosition setFigurLast = (ISetPosition)maxMoves.Value.Item2;
            if (movesOfBlackKing.Count == 1)
            {
                IRandomMove randomeMove = (IRandomMove)setFigurFirst;
                if (!randomeMove.IsUnderAttack(minMoves.Key, movesOfBlackKing[0]))
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

            while (movesOfBlackKing.Count != 0)
            {
                if (movesOfBlackKing.Count > 1)
                {
                    while (movesOfBlackKing.Count != 1)
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
        public static void DeleteFigur(FigureBase model)
        {
            var modelTemp = models.Where(c => c.Color != model.Color);
            foreach (var item in modelTemp)
            {
                if (model.Coordinate.X == item.Coordinate.X && model.Coordinate.Y == item.Coordinate.Y)
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

        #region Input metods
        public static bool FigurSelection(out int result)
        {
            View.ShowFigurs(10);
            Console.SetCursorPosition(40, 10);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 10);
            string exit = Console.ReadLine();
            if (exit.ToLower() != "e")
            {
                bool isPars = int.TryParse(exit, out int res);
                if (isPars)
                {
                    result = res;
                    return true;
                }
                else
                {
                    result = 0;
                    return false;
                }
            }
            else
            {
                result = 0;
                return false;
            }
        }
        public static bool FigurSelection(int number, out int result)
        {
            View.ShowFigurs(number);
            Console.SetCursorPosition(40, 10);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 10);
            string exit = Console.ReadLine();
            if (exit.ToLower() != "e")
            {
                bool isPars = int.TryParse(exit, out int res);
                if (isPars)
                {
                    result = res;
                    return true;
                }
                else
                {
                    result = 0;
                    return false;
                }
            }
            else
            {
                result = 0;
                return false;
            }
        }
        public static void Placement(int numberOfFigur)
        {
            string corrent = numberOfFigur.IntToString();
            var tuplFigur = corrent.StringSplit();

            var tupl = InputCoordinats(tuplFigur.Item1, tuplFigur.Item2);
            models.Add(StringToModel(corrent));
            StringToModel(corrent).SetPosition(tupl);
        }
        public static FigureBase StringToModel(string word)
        {
            var tuplFigur = word.StringSplit();
            string figur = tuplFigur.Item2.ToLower() + tuplFigur.Item1;
            switch (figur)
            {
                case "kingWhite":
                    return kingWhite;
                case "kingBlack":
                    return kingBlack;
                case "rooklWhite":
                    return rookL;
                case "rookrWhite":
                    return rookR;
                case "queenWhite":
                    return queen;
                case "knightWhite":
                    return knight;
                case "bishoplWhite":
                    return bishopL;
                case "bishoprWhite":
                    return bishopL;
                case "rooklBlack":
                    return rookBlackL;
            }
            return null;
        }
        public static FigureBase StringToModelForBlack(string word)
        {
            switch (word.ToLower())
            {
                case "rookl":
                    return rookBlackL;
                case "king":
                    return kingBlack;
            }
            return null;
        }
        public static void PlacementManager()
        {
            View.Board();
            bool isFigur = FigurSelection(out int result);
            if (isFigur)
            {
                Placement(result);
                count++;
                while (count != View.figurs.Count)
                {
                    isFigur = FigurSelection(result, out int res);
                    if (isFigur)
                    {
                        Placement(res);
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        public static Point InputCoordinats(string figureColor, string figureName)
        {
            Console.SetCursorPosition(40, 12);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 12);
            Console.WriteLine($"Please enter a position for {figureColor} {figureName}");
            Console.SetCursorPosition(40, 13);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 13);
            string input = Console.ReadLine();
            int i = input[0].CharToInt();
            int j = Convert.ToInt32(input[1].ToString());
            Point point = new Point(i, j);
            bool isEqual;
            if (figureColor == "Black" && figureName.ToLower() == "king")
                isEqual = IsKingAction(point);
            else
                isEqual = (InsideBord(point) && !GetPosition().Contains(point));
            if (isEqual)
            {
                if (figureColor != "Black")
                {
                    return point;
                }
            }
            while (!isEqual)
            {
                Console.SetCursorPosition(40, 15);
                Console.WriteLine("                                                       ");
                Console.SetCursorPosition(40, 15);
                Console.WriteLine("Non correct position!");
                point = InputCoordinats(figureColor, figureName);
                break;
            }
            return point;
        }
        public static Point InputCoordinatsForGame(string figureName)
        {
            Console.SetCursorPosition(40, 2);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 2);
            Console.WriteLine($"Please enter a position for Black {figureName}");
            Console.SetCursorPosition(40, 3);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 3);
            IAvailableMoves blackFigur = (IAvailableMoves)StringToModelForBlack(figureName);
            string input = Console.ReadLine();
            int i = input[0].CharToInt();
            int j = Convert.ToInt32(input[1].ToString());
            Point point = new Point(i, j);
            bool isEqual;
            if (figureName.ToLower() == "king")
                isEqual = IsKingAction(point);
            else
                isEqual = InsideBord(point) && blackFigur.AvailableMoves().Contains(point);
            if (isEqual)
                return point;
            while (!isEqual)
            {
                Console.SetCursorPosition(40, 5);
                Console.WriteLine("                                                       ");
                Console.SetCursorPosition(40, 5);
                Console.WriteLine("Non correct position!");
                point = InputCoordinatsForGame(figureName);
                break;
            }
            return point;
        }
        private static List<Point> GetPosition()
        {
            List<Point> positions = new List<Point>();
            foreach (var item in models)
            {
                positions.Add(item.Coordinate);
            }
            return positions;
        }

        #region Knight Play
        public static void KnightWhitePlay()
        {
            PlacementManager();
            var knightMoves = knight.AvailableMoves();
            foreach (var item in knightMoves)
            {
                SetPositionCoolor(item);
            }
            var tupl = InputCoordinats("End", "");
            Console.SetCursorPosition(40, 13);
            Console.WriteLine($"Your need a {knight.MinCount(tupl)} move");
        }
        public static void SetPositionCoolor(Point point)
        {
            Console.SetCursorPosition(2 + (point.X - 1) * 4, 1 + (point.Y - 1) * 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine('*');
            Console.ResetColor();
        }
        #endregion
        #endregion
    }
}
