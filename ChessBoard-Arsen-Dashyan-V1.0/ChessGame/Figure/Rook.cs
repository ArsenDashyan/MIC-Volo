using System;
using System.Collections.Generic;

namespace ChessGame
{
    class Rook : Model
    {
        public Rook(string name, ConsoleColor color) :base (name,color)
        {

        }
        public List<(int, int)> AvailableMoves()
        {
            return this.Crosswise();
        }
    }
}
