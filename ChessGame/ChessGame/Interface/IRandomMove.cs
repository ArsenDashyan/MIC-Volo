using Coordinats;

namespace ChessGame
{
    interface IRandomMove : IAvailableMoves
    {
        bool IsUnderAttack(CoordinatPoint CoordinatPoint);
        bool IsUnderAttack(CoordinatPoint CoordinatPoint, CoordinatPoint CoordinatPoint1);
        bool IsProtected();
        CoordinatPoint RandomMove(King king);
    }
}
