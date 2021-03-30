using System;
using Coordinats;
using static System.Net.Mime.MediaTypeNames;

namespace ChessGameLibrary
{
   public class FigureBase
    {
        #region Property and Feld
        public string Name { get; set; }
        public ConsoleColor Color { get; set; }
        public Point Coordinate { get; set; }
        #endregion
    }
}
