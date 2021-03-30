using Coordinats;

namespace ChessGameLibrary
{ 
    interface IRandomMove : IAvailableMoves
    {
        bool IsUnderAttack(Point point);
        bool IsUnderAttack(Point point, Point point1);
        bool IsProtected();
        Point RandomMove(King king);
    }
}
