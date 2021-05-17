using System.Collections.Generic;

namespace Figure
{
    public interface IDiagonal
    {
        List<CoordinatePoint> RightIndex(List<BaseFigure> othereFigures);
        List<CoordinatePoint> LeftIndex(List<BaseFigure> othereFigures);
    }
}
