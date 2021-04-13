using Coordinates;
using System.Collections.Generic;

namespace Interfaces
{
    interface ICrosswise
    {
        List<CoordinatePoint> Vertical();
        List<CoordinatePoint> Horizontal();
        List<CoordinatePoint> Crosswise();
    }
}
