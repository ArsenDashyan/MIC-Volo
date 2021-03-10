using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    public class King : Model, ICrosswise, IDiagonal
    {
        public King(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
        public bool IsMove(int let, int num)
        {
            if (Math.Abs(this.FCoord - let) <= 1 && Math.Abs(this.SCoord - num) <= 1)
                return true;
            else
                return false;
        }
        public List<(int, int)> Horizontal()
        {
            List<(int, int)> arr = new List<(int, int)>();
            if (this.FCoord -1<=8 && this.FCoord - 1>=1)
            {
                arr.Add((this.FCoord - 1, this.SCoord));
            }
            if (this.FCoord + 1 <= 8 && this.FCoord + 1>=1)
            {
                arr.Add((this.FCoord + 1, this.SCoord));
            }
            return arr;
        }
        public List<(int, int)> Vertical()
        {
            List<(int, int)> arr = new List<(int, int)>();
            if (this.SCoord - 1 <= 8 && this.SCoord - 1 >= 1)
            {
                arr.Add((this.FCoord, this.SCoord-1));
            }
            if (this.SCoord + 1 <= 8 && this.SCoord + 1 >= 1)
            {
                arr.Add((this.FCoord, this.SCoord +1));
            }
            return arr;
        }
        public List<(int, int)> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public List<(int, int)> RightIndex()
        {
            List<(int, int)> arr = new List<(int, int)>();
            if (this.FCoord + 1 <=8 && this.SCoord -1 >=1)
            {
                arr.Add((this.FCoord + 1, this.SCoord - 1));
            }
            if (this.FCoord - 1 >=1 && this.SCoord + 1 <= 8)
            {
                arr.Add((this.FCoord - 1, this.SCoord + 1));
            }
            return arr;
        }
        public List<(int, int)> LeftIndex()
        {
            List<(int, int)> arr = new List<(int, int)>();
            if (this.FCoord - 1 >= 1 && this.SCoord - 1 >= 1)
            {
                arr.Add((this.FCoord - 1, this.SCoord - 1));
            }
            if (this.FCoord + 1 <= 8 && this.SCoord + 1 <= 8)
            {
                arr.Add((this.FCoord + 1, this.SCoord + 1));
            }
            return arr;
        }
        public List<(int, int)> AvailableMoves()
        {
            var result = new List<(int, int)>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            result.AddRange(Crosswise());
            result = result.Where(c => c != (this.FCoord, this.SCoord) && !Manager.DangerousPosition().Contains(c)).ToList();
            return result;
        }
    }
}
