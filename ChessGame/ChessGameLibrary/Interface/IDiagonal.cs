﻿using System.Collections.Generic;
using Coordinats;

namespace ChessGameLibrary
{
    interface IDiagonal
    {
        List<CoordinatPoint> RightIndex();
        List<CoordinatPoint> LeftIndex();
    }
}
