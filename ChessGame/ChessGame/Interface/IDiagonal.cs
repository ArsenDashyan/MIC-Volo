﻿using Coordinates;
using System.Collections.Generic;

namespace ChessGame
{
    interface IDiagonal
    {
        List<CoordinatePoint> RightIndex();
        List<CoordinatePoint> LeftIndex();
    }
}
