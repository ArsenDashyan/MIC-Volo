using ChessGameLibrary;
using Coordinats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameConsole
{
    class ConsoleBoard : IChessBoard
    {
        public void RemoveFigureFromBoard(FigureBase figure)
        {
            if (figure == null)
                throw new ArgumentNullException(nameof(figure));

            if (figure.Coordinate != null)
            {
                Console.SetCursorPosition(2 + (figure.Coordinate.X - 1) * 4, 1 + (figure.Coordinate.Y - 1) * 2);
                Console.ForegroundColor = figure.Color;
                Console.WriteLine(" ");
                Console.ResetColor();
            }
        }

        public void SetFigurePosition(FigureBase figure, Point coordinate)
        {
            if (figure == null)
                throw new ArgumentNullException(nameof(figure));

            if (coordinate == null)
                throw new ArgumentNullException(nameof(coordinate));

            RemoveFigureFromBoard(figure);
            Console.SetCursorPosition(2 + (coordinate.X - 1) * 4, 1 + (coordinate.Y - 1) * 2);
            Console.ForegroundColor = figure.Color;
            Console.WriteLine(figure.Name[0]);
            figure.Coordinate = coordinate;
            Console.ResetColor();

        }

    }
}
