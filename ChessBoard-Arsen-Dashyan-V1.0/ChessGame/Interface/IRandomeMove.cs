using Coordinats;

namespace ChessGame
{
    interface IRandomeMove
    {
        bool IsUnderAttack(King king);
        bool IsProtected();
        bool IsProtected(Point point);
        Point RandomMove(King king);
    }
}
