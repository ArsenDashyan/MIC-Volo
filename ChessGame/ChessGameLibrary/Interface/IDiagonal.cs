using System.Collections.Generic;
using Coordinats;

namespace ChessGameLibrary
{
    interface IDiagonal
    {
        List<Point> RightIndex();
        List<Point> LeftIndex();
    }
}
