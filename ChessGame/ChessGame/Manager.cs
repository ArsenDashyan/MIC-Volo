using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ChessGame
{
    class Manager
    {
        public static int count = 1;
        public static Model king = new Model("King", 1, 5);
        public static Model rookL = new Model("RookL", 7, 0);
        public static Model rookR = new Model("RookR", 7, 7);
        public static Model queen = new Model("Queen", 7, 3);
        public void Logic()
        {
            View.ShowBoard(king.FCoord, king.SCoord);

            var tuple = GetCoordinats();
            bool isAction = (Math.Abs(king.FCoord - tuple.Item1) <= 1 && Math.Abs(king.SCoord - tuple.Item2) <= 1);

            if (isAction)
            {
                View.ShowBoard(tuple.Item1, tuple.Item2);

                while (count <= 4)
                {
                    Thread.Sleep(1000);
                    View.ShowBoard(tuple.Item1, tuple.Item2, count);
                    if (count != 4)
                    {
                        tuple = GetCoordinats();
                        if (isAction)
                        {
                            View.ShowBoard(tuple.Item1, tuple.Item2, count);
                        }
                    }
                    count++;
                }
                Console.WriteLine("Game over");
            }
            else
            {
                Console.WriteLine("Non correct action");
            }
        }

        /// <summary>
        /// Consolic uzum e sev arqai nor koordinatnery
        /// </summary>
        /// <returns>veradardznum e sev arqai nor kordinatner </returns>
        public static (int, int) GetCoordinats()
        {
            Console.WriteLine("Please enter a fisrt coordinate");
            int a = int.Parse(Console.ReadLine());

            
            Console.WriteLine("Please enter a second coordinate");
            int b = int.Parse(Console.ReadLine());
            if ((a - 1 < rookR.FCoord && b - 1 > queen.SCoord && a-1 < rookL.FCoord))
                return (a,b);

            while (!(a-1 < rookR.FCoord && b-1 > queen.SCoord && a-1 < rookL.FCoord))
            {
                Console.WriteLine("Please enter a correct coordinats");
                (a,b) = GetCoordinats();
                break;
            }

            return (a, b);
        }
    }
}
