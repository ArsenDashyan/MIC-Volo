using Coordinats;
using System.Collections.Generic;

namespace ChessGame
{
    interface IAvailableMoves
    {
        List<CoordinatPoint> AvailableMoves();
    }
}
