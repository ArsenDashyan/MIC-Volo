using System.Collections.Generic;
using Coordinats;

namespace ChessGame
{ 
    interface IAvailableMoves
    {
        List<Point> AvailableMoves();
    }
}
