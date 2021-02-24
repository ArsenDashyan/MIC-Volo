using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class Figure
    {
        public string Name { get; set; }
        public ConsoleColor Color { get; set; }
        public int FirstCoord { get; set; }
        public int SecondCoord { get; set; }

        public Figure(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
        public void SetPosition(int numF, int numS)
        {
            Console.SetCursorPosition(2 + (numF - 1) * 4, 1 + (numS - 1) * 2);
            Console.ForegroundColor = Color;
            Console.WriteLine(this.Name[0]);
            this.FirstCoord = numF;
            this.SecondCoord = numS;
            Console.ResetColor();
        }
        public void DeleteFigure()
        {
            Console.SetCursorPosition(2 + (this.FirstCoord - 1) * 4, 1 + (this.SCoord - 1) * 2);
            Console.ForegroundColor = Color;
            Console.WriteLine(" ");
            Console.ResetColor();
        }
    }
}
