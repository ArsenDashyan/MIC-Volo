using System.Collections.Generic;

namespace Figure
{
    public interface ICrosswise
    {
        List<CoordinatePoint> Vertical(List<BaseFigure> othereFigures);
        List<CoordinatePoint> Horizontal(List<BaseFigure> othereFigures);
        List<CoordinatePoint> Crosswise(List<BaseFigure> othereFigures);
    }
}
