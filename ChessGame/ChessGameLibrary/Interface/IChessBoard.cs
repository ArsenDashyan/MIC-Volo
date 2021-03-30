using Coordinats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameLibrary
{
    public interface IChessBoard
    {
        void SetFigurePosition(FigureBase figure, CoordinatPoint coordinate);

        void RemoveFigureFromBoard(FigureBase figure);

        void WriteGameText(string text);
    }
}
