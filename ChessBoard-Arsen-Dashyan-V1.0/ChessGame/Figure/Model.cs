using System;
using Coordinats;

namespace ChessGame
{
    public class Model
    {
        #region Property and Feld
        public string Name { get; set; }
        public ConsoleColor Color { get; set; }
        public Point point { get; set; }
        #endregion
        public void SetPosition(Point point)
        {
            DeleteFigure();
            Console.SetCursorPosition(2 + (point.X - 1) * 4, 1 + (point.Y - 1) * 2);
            this.point = point;
            Console.ForegroundColor = Color;
            Console.WriteLine(this.Name[0]);
            Console.ResetColor();
        }
        public void DeleteFigure()
        {
            if (this.point != null)
            {
                Console.SetCursorPosition(2 + (this.point.X - 1) * 4, 1 + (this.point.Y - 1) * 2);
                Console.ForegroundColor = Color;
                Console.WriteLine(" ");
                Console.ResetColor();
            }
        }
    }
}
