using System;
using System.Collections.Generic;

namespace ChessGame
{
    class Rook : Model
    {
        public Rook(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
        public List<(int, int)> AvailableMoves()
        {
            return this.Crosswise();
        }
    }
}
