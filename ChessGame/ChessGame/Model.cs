using System;
using System.Linq;

namespace ChessGame
{
    public class Model
    {
        public string Name { get; set; }
        public int FCoord { get; set; }
        public int SCoord { get; set; }

        public Model(string name, int fCorod, int sCoord)
        {
            Name = name;
            FCoord = fCorod;
            SCoord = sCoord;
        }
    }
}
