using Figure;
using Helper;

namespace MovesFKnight
{
    public class MovesKnight
    {
        private int count = 1;
        public int MinKnightCount(CoordinatePoint coordinatPoint, CoordinatePoint coordinatPoint1)
        {
            var knightMoves = KnightMove.Crosswise(coordinatPoint1);
            var endMoves = KnightMove.Crosswise(coordinatPoint);
            if (knightMoves.Contains((coordinatPoint)))
            {
                return count;
            }
            else
            {
                count++;
                if (KnightMove.Equals(coordinatPoint1, coordinatPoint))
                {
                    return count;
                }
                else
                {
                    count++;
                    if (KnightMove.Equals(knightMoves, coordinatPoint))
                    {
                        return count;
                    }
                    else
                    {
                        count++;
                        if (KnightMove.Equals(knightMoves, endMoves))
                        {
                            return count;
                        }
                        else
                        {
                            count++;
                            if (KnightMove.EqualsEnd(knightMoves, endMoves))
                            {
                                return count;
                            }
                            else
                            {
                                return 6;
                            }
                        }
                    }
                }
            }
        }
    }
}
