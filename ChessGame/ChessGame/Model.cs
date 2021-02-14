using System;
using System.Linq;

namespace ChessGame
{
    public class Model
    {
        public  string Name { get; set; }
        public  int FCoord { get; set; }
        public  int SCoord { get; set; }

        public  Model(string name, int fCorod, int sCoord)
        {
            Name = name;
            FCoord = fCorod;
            SCoord = sCoord;
        }
        public  bool Move(int a, int b)
        {
            if (Math.Abs(this.FCoord - a) <= 1 && Math.Abs(this.SCoord - b) <= 1)
            {
                this.FCoord = a;
                this.SCoord = b;
                return true;
            }
            return false;
        }
    }
}
