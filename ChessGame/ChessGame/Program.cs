using System;

namespace ChessGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //View.ShowBoard(1, 5);
            Manager manager = new Manager();
            manager.Logic();

            Console.ReadLine();
        }
    }
}
