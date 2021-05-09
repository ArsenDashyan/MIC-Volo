using System.Collections.Generic;

namespace Figure
{
    public interface IAntiCheck
    {
        List<CoordinatePoint> MovesWithKingIsNotUnderCheck();
    }
}
