using Coordinats;
using System.Windows.Controls;

namespace ChessGame
{
    interface ISetPosition
    {
        public void SetFigurePosition(CoordinatPoint coordinate, Grid grid);
        public void RemoveFigureFromBoard(BaseFigure figure, Grid grid);
    }
}
