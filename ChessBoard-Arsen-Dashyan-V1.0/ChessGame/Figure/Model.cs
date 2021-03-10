using System;

namespace ChessGame
{
    public class Model
    {
        #region Property and Feld
        public string Name { get; set; }
        public int FCoord { get; set; }
        public int SCoord { get; set; }
        public ConsoleColor Color { get; set; }
        #endregion
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
        protected void DeleteFigure()
        {
            if (this.FCoord != 0 && this.SCoord != 0)
                Console.SetCursorPosition(2 + (this.FCoord - 1) * 4, 1 + (this.SCoord - 1) * 2);
            else
                Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = Color;
            Console.WriteLine(" ");
            Console.ResetColor();
        }
    }
}
