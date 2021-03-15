using System.Collections.Generic;
using Coordinats;

namespace ChessGame
{
    interface IDiagonal
    {
        List<Point> RightIndex();
        List<Point> LeftIndex();
    }
}
