using System.Collections.Generic;

namespace ChessGame
{
    interface IDiagonal
    {
        List<(int, int)> RightIndex();
        List<(int, int)> LeftIndex();
    }
}
