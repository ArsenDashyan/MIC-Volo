using ChessGameLibrary;
using System;

namespace ChessGameConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleBoard board = new ConsoleBoard();
            var manager = new Manager(board);
            manager.PlayNewWithShaxAndBox();
        }
    }
}
