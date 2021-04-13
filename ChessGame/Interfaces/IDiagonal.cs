using Coordinates;
using System.Collections.Generic;

namespace Interfaces
{
    interface IDiagonal
    {
        List<CoordinatePoint> RightIndex();
        List<CoordinatePoint> LeftIndex();
    }
}
