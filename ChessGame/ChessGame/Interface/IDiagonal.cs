using Coordinats;
using System.Collections.Generic;

namespace ChessGame
{
    interface IDiagonal
    {
        List<CoordinatPoint> RightIndex();
        List<CoordinatPoint> LeftIndex();
    }
}
