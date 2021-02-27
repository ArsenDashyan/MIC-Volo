using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var result = new List<(int, int)>();
            result.AddRange(Crosswise());
            return result;
        }
    }
}
