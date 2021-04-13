﻿using Coordinates;
using System.Windows.Controls;

namespace ChessGame
{
    interface ISetPosition
    {
        public void SetFigurePosition(CoordinatePoint coordinate, Grid grid);
        public void RemoveFigureFromBoard(BaseFigure figure, Grid grid);
    }
}
