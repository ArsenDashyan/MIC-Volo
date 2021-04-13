using Coordinates;
using System.Windows.Controls;

namespace Figure
{
    public interface ISetPosition
    {
        public void SetFigurePosition(CoordinatePoint coordinate, Grid grid);
        public void RemoveFigureFromBoard(BaseFigure figure, Grid grid);
    }
}
