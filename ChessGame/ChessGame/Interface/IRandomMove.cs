using Coordinats;

namespace ChessGame
{
    interface IRandomMove : IAvailableMoves
    {
        bool IsUnderAttack(CoordinatPoint coordinatPoint);
        bool IsProtected(CoordinatPoint coordinatPoint);
        CoordinatPoint RandomMove(King king);
    }
}
