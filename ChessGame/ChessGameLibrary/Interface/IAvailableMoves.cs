using System.Collections.Generic;
using Coordinats;

namespace ChessGameLibrary
{ 
    interface IAvailableMoves
    {
        List<Point> AvailableMoves();
    }
}
