using Coordinates;
using System.Collections.Generic;

namespace Figure
{
    public interface IDiagonal
    {
        List<CoordinatePoint> RightIndex();
        List<CoordinatePoint> LeftIndex();
    }
}
