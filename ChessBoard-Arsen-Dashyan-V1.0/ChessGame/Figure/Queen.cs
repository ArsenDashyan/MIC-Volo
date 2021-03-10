using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    public class Queen : Model, IDiagonal, ICrosswise
    {
        public Queen(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
        public List<(int, int)> Vertical()
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
        public List<(int, int)> Horizontal()
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
        public List<(int, int)> RightIndex()
        {
            List<(int, int)> arr = new List<(int, int)>();
            var positionsWithOutQueen = Manager.positions.Where(c => c != (this.FCoord, this.SCoord)).ToList();
            int sum = this.FCoord + this.SCoord;
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (i + j == sum)
                    {
                        arr.Add((i, j));
                    }
                }
            }

            foreach (var item in positionsWithOutQueen)
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
        public List<(int, int)> LeftIndex()
        {
            List<(int, int)> arr = new List<(int, int)>();
            int sub = this.FCoord - this.SCoord;
            var positionsWithOutQueen = Manager.positions.Where(c => c != (this.FCoord, this.SCoord)).ToList();

            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    if (i - j == sub)
                    {
                        arr.Add((i, j));
                    }
                }
            }
            foreach (var item in positionsWithOutQueen)
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
        public List<(int, int)> AvailableMoves()
        {
            var result = new List<(int, int)>();
            result.AddRange(RightIndex());
            result.AddRange(LeftIndex());
            result.AddRange(Crosswise());
            result = result.Where(c => c != (this.FCoord, this.SCoord)).ToList();
            return result;
        }
    }
}
