using System.Collections.Generic;

namespace ChessGame
{
    interface ICrosswise
    {
        List<(int, int)> Vertical();
        List<(int, int)> Horizontal();
        List<(int, int)> Crosswise();
    }
}
