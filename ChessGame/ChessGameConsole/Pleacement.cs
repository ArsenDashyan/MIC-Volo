using System;
using System.Collections.Generic;
using System.Linq;
using Figure;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameConsole
{
    class Pleacement
    {
        public static int count = 0;
        public static List<BaseFigure> models = new List<BaseFigure>();
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
            models.Add(StringToBaseFigure(corrent));
            StringToBaseFigure(corrent).SetFigurePosition(tupl);
        }
        public static BaseFigure StringToBaseFigure(string word)
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
        public static BaseFigure StringToBaseFigureForBlack(string word)
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
        public static CoordinatePoint InputCoordinats(string figureColor, string figureName)
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
            CoordinatePoint point = new CoordinatePoint(i, j);
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
        public static CoordinatePoint InputCoordinatsForGame(string figureName)
        {
            Console.SetCursorPosition(40, 2);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 2);
            Console.WriteLine($"Please enter a position for Black {figureName}");
            Console.SetCursorPosition(40, 3);
            Console.WriteLine("                                                       ");
            Console.SetCursorPosition(40, 3);
            IAvailableMoves blackFigur = (IAvailableMoves)StringToBaseFigureForBlack(figureName);
            string input = Console.ReadLine();
            int i = input[0].CharToInt();
            int j = Convert.ToInt32(input[1].ToString());
            CoordinatePoint point = new CoordinatePoint(i, j);
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
        private static List<CoordinatePoint> GetPosition()
        {
            List<CoordinatePoint> positions = new List<CoordinatePoint>();
            foreach (var item in models)
            {
                positions.Add(item.Coordinate);
            }
            return positions;
        }
        private static bool IsKingAction(Point point) =>
           (InsideBord(point) && !GetPosition().Contains(point)
            && !DangerPosition(kingBlack).Contains(point)
            && kingBlack.IsMove(point) && (kingBlack.point?.X != point.X | kingBlack.point?.Y != point.Y));
        public static List<CoordinatePoint> DangerPosition(BaseFigure model)
        {
            var result = new List<CoordinatePoint>();
            var modelNew = models.Where(c => c.Color != model.Color);
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                var array = temp.AvailableMoves();
                result.AddRange(array);
            }
            return result;
        }
    }
}
