using Coordinates;

namespace ChessGame
{
    interface IRandomMove : IAvailableMoves
    {
        bool IsUnderAttack(CoordinatePoint coordinatPoint);
        bool IsProtected(CoordinatePoint coordinatPoint);
        CoordinatePoint RandomMove(King king);
    }
}
