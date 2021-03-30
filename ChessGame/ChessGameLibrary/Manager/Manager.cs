using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGameLibrary
{
    public class Manager
    {
        #region Property and Feld
        private const int leftSize = 8;
        private const int rightSize = 1;
        private readonly IChessBoard board;
        public int count = 0;
        public List<FigureBase> models = new List<FigureBase>();
        public King kingBlack;
        public Rook rookBlackL;
        public Rook rookL;
        public Rook rookR;
        public Bishop bishopL;
        public Bishop bishopR;
        public Queen queen;
        public King kingWhite;
        public Knights knight;
        public List<CoordinatPoint> movesOfBlackKing => kingBlack.AvailableMoves().Where(c => !DangerPosition(kingBlack).Contains(c)).ToList();


        #endregion

        public Manager(IChessBoard board)
        {
            this.board = board;
            kingBlack = new King("King", ConsoleColor.Red, models);
            rookBlackL = new Rook("RookBlackL", ConsoleColor.Red, models);
            rookL = new Rook("RookL", ConsoleColor.White, models);
            rookR = new Rook("RookR", ConsoleColor.White, models);
            bishopL = new Bishop("BishopL", ConsoleColor.White, models);
            bishopR = new Bishop("BishopR", ConsoleColor.White, models);
            queen = new Queen("Queen", ConsoleColor.White, models);
            kingWhite = new King("King", ConsoleColor.White, models);
            knight = new Knights("knight", ConsoleColor.White, models);
        }
                

        #region Chack The Dangerous Positions

        /// <summary>
        /// Chack tke king move
        /// </summary>
        /// <param name="i">Black king first coordinat</param>
        /// <param name="j">Black king second coordinat</param>
        /// <returns>Return true if king coordinat is a permissible</returns>
        private bool IsKingAction(CoordinatPoint CoordinatPoint) =>
           (InsideBord(CoordinatPoint) && !GetPosition().Contains(CoordinatPoint)
            && !DangerPosition(kingBlack).Contains(CoordinatPoint)
            && kingBlack.IsMove(CoordinatPoint) && kingBlack.Coordinate != CoordinatPoint);

        /// <summary>
        /// Created a positions wher can a white figures move
        /// </summary>
        /// <returns>Return the Available moves</returns>
        public List<CoordinatPoint> DangerPosition(FigureBase model)
        {
            List<CoordinatPoint> result = new List<CoordinatPoint>();
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
        private bool InsideBord(CoordinatPoint CoordinatPoint) =>
           (CoordinatPoint.X <= leftSize && CoordinatPoint.X >= rightSize && CoordinatPoint.Y >= rightSize && CoordinatPoint.Y <= leftSize);
        #endregion

        #region New Game Logic
        public bool GetMinMovesWithShax(FigureBase figur, out KeyValuePair<CoordinatPoint, (int, FigureBase)> keyValuePair)
        {
            Dictionary<CoordinatPoint, (int, FigureBase)> countList = new Dictionary<CoordinatPoint, (int, FigureBase)>();
            IRandomMove tempFigur = (IRandomMove)figur;
            CoordinatPoint temp = figur.Coordinate;
            foreach (var item in tempFigur.AvailableMoves())
            {
                figur.Coordinate = item;
                if (CoordinatPoint.Modul(item, kingBlack.Coordinate) >= 2)
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
                keyValuePair = new KeyValuePair<CoordinatPoint, (int, FigureBase)>();
                return false;
            }
        }
        private void MinCountWithShax()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color && !(c is King)).ToList();
            List<KeyValuePair<CoordinatPoint, (int, FigureBase)>> list = new List<KeyValuePair<CoordinatPoint, (int, FigureBase)>>();
            foreach (var figur in modelNew)
            {
                if (GetMinMovesWithShax(figur, out KeyValuePair<CoordinatPoint, (int, FigureBase)> keyValuePair))
                    list.Add(keyValuePair);
            }
            if (list.Count != 0)
            {
                var minMoves = list.OrderBy(c => c.Value.Item1).FirstOrDefault();
                var maxMoves = list.OrderBy(c => c.Value.Item1).LastOrDefault();
                FigureBase setFigurFirst = minMoves.Value.Item2;
                FigureBase setFigurLast = maxMoves.Value.Item2;
                if (movesOfBlackKing.Count == 1)
                {
                    IRandomMove randomeMove = (IRandomMove)setFigurFirst;
                    if (!randomeMove.IsUnderAttack(minMoves.Key, movesOfBlackKing[0]))
                    {
                        board.SetFigurePosition(setFigurFirst,minMoves.Key);
                        DeleteFigur(minMoves.Value.Item2);
                    }
                    else
                    {
                        board.SetFigurePosition(setFigurLast, maxMoves.Key);
                        DeleteFigur(maxMoves.Value.Item2);
                    }
                }
                else
                {
                    board.SetFigurePosition(setFigurFirst, minMoves.Key);
                    DeleteFigur(minMoves.Value.Item2);
                }
            }
            else
            {
                var randomFigur = modelNew[new Random().Next(0, modelNew.Count)];
                IRandomMove setFigur = (IRandomMove)randomFigur;
                CoordinatPoint CoordinatPoint1 = setFigur.RandomMove(kingBlack);
                board.SetFigurePosition(randomFigur,CoordinatPoint1);
                DeleteFigur(randomFigur);
            }
        }
        private void AntiBabyGame()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color).ToList();
            List<(double, FigureBase)> destination = new List<(double, FigureBase)>();
            foreach (var figur in modelNew)
            {
                double tempDestination = CoordinatPoint.Modul(figur.Coordinate, kingBlack.Coordinate);
                destination.Add((tempDestination, figur));
            }
            (double, FigureBase) max = destination.OrderBy(k => k.Item1).FirstOrDefault();
            IRandomMove tempFigur = (IRandomMove)max.Item2;
            board.SetFigurePosition(max.Item2,tempFigur.RandomMove(kingBlack));
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
                        CoordinatPoint CoordinatPoint1 = tempFigur.RandomMove(kingBlack);
                        board.SetFigurePosition(figur,CoordinatPoint1);
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
            List<CoordinatPoint> list = new List<CoordinatPoint>();
            while (movesOfBlackKing.Count != 0)
            {
                View.ClearText();
                var CoordinatPoint = InputCoordinats("Black", "King");
                list.Add(CoordinatPoint);
                board.SetFigurePosition(kingBlack,CoordinatPoint);
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
        public bool GetMinMovesWithBox(FigureBase figur, out KeyValuePair<CoordinatPoint, (int, FigureBase)> keyValuePair)
        {
            Dictionary<CoordinatPoint, (int, FigureBase)> countList = new Dictionary<CoordinatPoint, (int, FigureBase)>();
            IRandomMove tempFigur = (IRandomMove)figur;
            CoordinatPoint temp = figur.Coordinate;
            foreach (var item in tempFigur.AvailableMoves())
            {
                figur.Coordinate = item;
                if (CoordinatPoint.Modul(item, kingBlack.Coordinate) >= 2)
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
                keyValuePair = new KeyValuePair<CoordinatPoint, (int, FigureBase)>();
                return false;
            }
        }
        private void MinCountWithBox()
        {
            var modelNew = models.Where(c => c.Color != kingBlack.Color).Reverse().ToList();
            List<KeyValuePair<CoordinatPoint, (int, FigureBase)>> list = new List<KeyValuePair<CoordinatPoint, (int, FigureBase)>>();
            foreach (var figur in modelNew)
            {
                if (GetMinMovesWithBox(figur, out KeyValuePair<CoordinatPoint, (int, FigureBase)> keyValuePair))
                    list.Add(keyValuePair);
            }
            list = list.Where(c => c.Value.Item1 != 0).ToList();
            var minMoves = list.OrderBy(c => c.Value.Item1).FirstOrDefault();
            var maxMoves = list.OrderBy(c => c.Value.Item1).LastOrDefault();
           FigureBase setFigurFirst = minMoves.Value.Item2;
            FigureBase setFigurLast = maxMoves.Value.Item2;
            if (movesOfBlackKing.Count == 1)
            {
                IRandomMove randomeMove = (IRandomMove)setFigurFirst;
                if (!randomeMove.IsUnderAttack(minMoves.Key, movesOfBlackKing[0]))
                {
                    board.SetFigurePosition(setFigurFirst,minMoves.Key);
                    DeleteFigur(minMoves.Value.Item2);
                }
                else
                {
                    board.SetFigurePosition(setFigurLast,maxMoves.Key);
                    DeleteFigur(maxMoves.Value.Item2);
                }
            }
            else
            {
                board.SetFigurePosition(setFigurFirst,minMoves.Key);
                DeleteFigur(minMoves.Value.Item2);
            }
        }
        public void PlayNewWithShaxAndBox()
        {
            PlacementManager();
            List<CoordinatPoint> list = new List<CoordinatPoint>();

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
                        var CoordinatPoint = InputCoordinatsForGame(temp);
                        list.Add(CoordinatPoint);
                        FigureBase blackFigur = StringToModelForBlack(temp);
                        board.SetFigurePosition(blackFigur,CoordinatPoint);
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
                    var CoordinatPoint = InputCoordinatsForGame(temp);
                    list.Add(CoordinatPoint);
                    FigureBase blackFigur = StringToModelForBlack(temp);
                    board.SetFigurePosition(blackFigur,CoordinatPoint);
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
        public void DeleteFigur(FigureBase model)
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
        public void ShowBlackFigurs()
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
        public bool FigurSelection(out int result)
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
        public bool FigurSelection(int number, out int result)
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
        public void Placement(int numberOfFigur)
        {
            string corrent = numberOfFigur.IntToString();
            var tuplFigur = corrent.StringSplit();

            var tupl = InputCoordinats(tuplFigur.Item1, tuplFigur.Item2);
            models.Add(StringToModel(corrent));
            board.SetFigurePosition(StringToModel(corrent),tupl);
        }
        public FigureBase StringToModel(string word)
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
        public FigureBase StringToModelForBlack(string word)
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
        public void PlacementManager()
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
        public CoordinatPoint InputCoordinats(string figureColor, string figureName)
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
            CoordinatPoint CoordinatPoint = new CoordinatPoint(i, j);
            bool isEqual;
            if (figureColor == "Black" && figureName.ToLower() == "king")
                isEqual = IsKingAction(CoordinatPoint);
            else
                isEqual = (InsideBord(CoordinatPoint) && !GetPosition().Contains(CoordinatPoint));
            if (isEqual)
            {
                if (figureColor != "Black")
                {
                    return CoordinatPoint;
                }
            }
            while (!isEqual)
            {
                Console.SetCursorPosition(40, 15);
                Console.WriteLine("                                                       ");
                Console.SetCursorPosition(40, 15);
                Console.WriteLine("Non correct position!");
                CoordinatPoint = InputCoordinats(figureColor, figureName);
                break;
            }
            return CoordinatPoint;
        }
        public CoordinatPoint InputCoordinatsForGame(string figureName)
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
            CoordinatPoint CoordinatPoint = new CoordinatPoint(i, j);
            bool isEqual;
            if (figureName.ToLower() == "king")
                isEqual = IsKingAction(CoordinatPoint);
            else
                isEqual = InsideBord(CoordinatPoint) && blackFigur.AvailableMoves().Contains(CoordinatPoint);
            if (isEqual)
                return CoordinatPoint;
            while (!isEqual)
            {
                Console.SetCursorPosition(40, 5);
                Console.WriteLine("                                                       ");
                Console.SetCursorPosition(40, 5);
                Console.WriteLine("Non correct position!");
                CoordinatPoint = InputCoordinatsForGame(figureName);
                break;
            }
            return CoordinatPoint;
        }
        private List<CoordinatPoint> GetPosition()
        {
            List<CoordinatPoint> positions = new List<CoordinatPoint>();
            foreach (var item in models)
            {
                positions.Add(item.Coordinate);
            }
            return positions;
        }

        #region Knight Play
        public void KnightWhitePlay()
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
        public void SetPositionCoolor(CoordinatPoint CoordinatPoint)
        {
            Console.SetCursorPosition(2 + (CoordinatPoint.X - 1) * 4, 1 + (CoordinatPoint.Y - 1) * 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine('*');
            Console.ResetColor();
        }
        #endregion
        #endregion
    }
}
