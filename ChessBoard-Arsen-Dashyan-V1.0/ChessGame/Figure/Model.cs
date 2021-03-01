using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    public class Model
    {
        #region Property and Feld
        public string Name { get; set; }
        public int FCoord { get; set; }
        public int SCoord { get; set; }
        public ConsoleColor Color { get; set; }
        #endregion

        public Model(string name, ConsoleColor color)
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
        protected void DeleteFigure()
        {
            if (this.FCoord != 0 && this.SCoord != 0)
                Console.SetCursorPosition(2 + (this.FCoord - 1) * 4, 1 + (this.SCoord - 1) * 2);
            else
                Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = Color;
            Console.WriteLine(" ");
            Console.ResetColor();
        }
        protected List<(int, int)> Vertical()
        {
            List<(int, int)> arr = new List<(int, int)>();
            var positionsWithOut = Manager.positions.Where(c => c != (this.FCoord, this.SCoord)).ToList();
            for (int i = 1; i <= 8; i++)
            {
                arr.Add((this.FCoord, i));
            }
            foreach (var item in positionsWithOut)
            {
                if (arr.Contains(item))
                {
                    if (arr.IndexOf((this.FCoord, this.SCoord)) < arr.IndexOf(item))
                    {
                        arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item)).ToList();
                    }
                    else
                    {
                        arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item)).ToList();
                    }
                }

            }
            return arr;
        }
        protected List<(int, int)> Horizontal()
        {
            List<(int, int)> arr = new List<(int, int)>();
            var positionsWithOut = Manager.positions.Where(c => c != (this.FCoord, this.SCoord)).ToList();
            for (int i = 1; i <= 8; i++)
            {
                arr.Add((i, this.SCoord));
            }
            foreach (var item in positionsWithOut)
            {
                if (arr.Contains(item))
                {
                    if (arr.IndexOf((this.FCoord, this.SCoord)) < arr.IndexOf(item))
                    {
                        arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item)).ToList();
                    }
                    else
                    {
                        arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item)).ToList();
                    }
                }

            }
            return arr;
        }
        public List<(int, int)> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
            arrayHor = arrayHor.Where(c => c != (this.FCoord, this.SCoord)).ToList();
            return arrayHor;
        }
    }
}
