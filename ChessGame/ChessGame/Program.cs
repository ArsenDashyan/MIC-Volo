using System;

namespace ChessGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager();
            manager.Logic();

            Console.ReadLine();
        }
    }
}
