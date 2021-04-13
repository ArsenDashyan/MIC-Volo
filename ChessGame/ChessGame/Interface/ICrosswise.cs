﻿using Coordinates;
using System.Collections.Generic;

namespace ChessGame
{
    interface ICrosswise
    {
        List<CoordinatePoint> Vertical();
        List<CoordinatePoint> Horizontal();
        List<CoordinatePoint> Crosswise();
    }
}
