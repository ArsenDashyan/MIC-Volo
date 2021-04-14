using System.Collections.Generic;

namespace Figure
{
    public interface ICrosswise
    {
        List<CoordinatePoint> Vertical();
        List<CoordinatePoint> Horizontal();
        List<CoordinatePoint> Crosswise();
    }
}
