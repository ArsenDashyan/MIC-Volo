﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame
{
    public class Model
    {
        #region Property and Field
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
        private List<(int, int)> VerticalIndex()
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
                        arr.Add((this.FCoord, this.SCoord));
                    }
                    else
                    {
                        arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item)).ToList();
                        arr.Add((this.FCoord, this.SCoord));
                    }
                }

            }
            return arr;
        }
        private List<(int, int)> HorizontalIndex()
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
                        arr.Add((this.FCoord, this.SCoord));
                    }
                    else
                    {
                        arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item)).ToList();
                        arr.Add((this.FCoord, this.SCoord));
                    }
                }

            }
            return arr;
        }
        public List<(int, int)> HorizontalVertical()
        {
            var arrayHor = this.HorizontalIndex();
            var arrayVert = this.VerticalIndex();
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
    }
}
