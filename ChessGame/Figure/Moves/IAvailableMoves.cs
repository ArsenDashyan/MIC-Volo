using System.Collections.Generic;

namespace Figure
{
    public interface IAvailableMoves
    {
        List<CoordinatePoint> AvailableMoves(List<BaseFigure> othereFigures);
    }
}
