using Coordinats;

namespace ChessGame
{
    interface IRandomMove : IAvailableMoves
    {
        bool IsUnderAttack(CoordinatPoint CoordinatPoint);
        bool IsProtected();
        CoordinatPoint RandomMove(King king);
    }
}
