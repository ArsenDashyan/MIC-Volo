using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    class King
    {
        #region Property and Field
        public string Name { get; set; }
        public int FCoord { get; set; }
        public int SCoord { get; set; }
        public ConsoleColor Color { get; set; }

        #endregion
        public King(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
        public void SetPosition(int numF, int numS)
        {
            DeleteFigure();
            Console.SetCursorPosition(2 + (numF - 1) * 4, 1 + (numS - 1) * 2);
            Console.ForegroundColor = Color;
            Console.WriteLine(this.Name[0]);
            this.FCoord = numF;
            this.SCoord = numS;
            Console.ResetColor();
        }
        private void DeleteFigure()
        {
            if (this.FCoord != 0 && this.SCoord != 0)
                Console.SetCursorPosition(2 + (this.FCoord - 1) * 4, 1 + (this.SCoord - 1) * 2);
            else
                Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = Color;
            Console.WriteLine(" ");
            Console.ResetColor();

        }
        public bool IsMove(int let, int num)
        {
            if (Math.Abs(this.FCoord - let) <= 1 && Math.Abs(this.SCoord - num) <= 1)
                return true;
            else
                return false;
        }
        private List<(int, int)> Horizontal()
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
        private List<(int, int)> Vertical()
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
        private List<(int, int)> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        private List<(int, int)> RightIndex()
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
        private List<(int, int)> LeftIndex()
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
