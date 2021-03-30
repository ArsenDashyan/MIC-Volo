using Coordinats;
using System.Collections.Generic;

namespace ChessGame
{
    interface ICrosswise
    {
        List<CoordinatPoint> Vertical();
        List<CoordinatPoint> Horizontal();
        List<CoordinatPoint> Crosswise();
    }
}
