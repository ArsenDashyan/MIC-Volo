using System.Collections.Generic;
using Coordinats;

namespace ChessGame
{
    interface ICrosswise
    {
        List<Point> Vertical();
        List<Point> Horizontal();
        List<Point> Crosswise();
    }
}
