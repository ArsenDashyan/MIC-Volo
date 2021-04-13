﻿using Coordinates;
using System.Collections.Generic;

namespace ChessGame
{
    interface IAvailableMoves
    {
        List<CoordinatePoint> AvailableMoves();
    }
}
