﻿using Coordinates;
using System.Collections.Generic;

namespace ChessGame
{
    interface IDangerMoves
    {
        public List<CoordinatePoint> DangerMoves();
    }
}
