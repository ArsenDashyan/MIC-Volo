using System;
using Utility;

namespace ChessGame
{
    public partial class Manager
    {
        public static bool FigurSelection(out int result)
        {
            View.ShowFigurs(10);
            Console.SetCursorPosition(40, 8);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 8);
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
            Console.SetCursorPosition(40, 8);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 8);
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
            StringToModel(corrent).SetPosition(tupl.let, tupl.num);
        }
        public static Model StringToModel(string word)
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
                while (count != 6)
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
        public static (int let, int num) InputCoordinats(string figureColor, string figureName)
        {
            Console.SetCursorPosition(40, 10);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 10);
            Console.WriteLine($"Please enter a position for {figureColor} {figureName}");
            Console.SetCursorPosition(40, 11);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 11);
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
                Console.SetCursorPosition(40, 13);
                Console.WriteLine("                                                       ");
                Console.SetCursorPosition(40, 13);
                Console.WriteLine("Non correct position!");
                (i, j) = InputCoordinats(figureColor, figureName);
                break;
            }
            return (i, j);
        }

        #region Knight Play
        public static void KnightWhitePlay()
        {
            PlacementManager();
            var knightMoves = knight.AvailableMoves();
            foreach (var item in knightMoves)
            {
                SetPositionCoolor(item.Item1, item.Item2);
            }
            var tupl = InputCoordinats("End", "");
            Console.SetCursorPosition(40, 13);
            Console.WriteLine($"Your need a {knight.MinCount(tupl.let, tupl.num)} move");
        }
        public static void SetPositionCoolor(int numF, int numS)
        {
            Console.SetCursorPosition(2 + (numF - 1) * 4, 1 + (numS - 1) * 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine('*');
            Console.ResetColor();
        }
        #endregion
    }
}
