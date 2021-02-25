using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class King : Figure
    {
        public King(string name, ConsoleColor color) : base(name,  color)
        {

        }

        public bool AvailableMoves(int letter, int number)
        {
            if (Math.Abs(FirstCoord - letter) <= 1 && Math.Abs(SecondCoord - number) <= 1)
            {
                return true;
            }
            return false;
        }
    }
}
