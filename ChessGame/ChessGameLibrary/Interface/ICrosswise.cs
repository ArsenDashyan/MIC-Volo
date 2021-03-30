using System.Collections.Generic;
using Coordinats;

namespace ChessGameLibrary
{
    interface ICrosswise
    {
        List<Point> Vertical();
        List<Point> Horizontal();
        List<Point> Crosswise();
    }
}
