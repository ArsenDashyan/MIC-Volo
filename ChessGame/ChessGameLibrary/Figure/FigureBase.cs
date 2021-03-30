using System;
using System.Collections.Generic;
using Coordinats;
using static System.Net.Mime.MediaTypeNames;

namespace ChessGameLibrary
{
   public class FigureBase
    {
        protected readonly List<FigureBase> othereFigures;
        #region Property and Feld
        public string Name { get; set; }
        public ConsoleColor Color { get; set; }
        public CoordinatPoint Coordinate { get; set; }
        #endregion

        public FigureBase(List<FigureBase> othereFigures)
        {
            this.othereFigures = othereFigures;
        }
    }
}
