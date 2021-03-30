﻿using System.Collections.Generic;
using Coordinats;

namespace ChessGameLibrary
{
    interface ICrosswise
    {
        List<CoordinatPoint> Vertical();
        List<CoordinatPoint> Horizontal();
        List<CoordinatPoint> Crosswise();
    }
}
