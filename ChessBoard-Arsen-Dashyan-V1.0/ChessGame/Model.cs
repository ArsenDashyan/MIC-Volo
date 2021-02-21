using System;

namespace ChessGame
{
    public class Model
    {
        #region Property and Field
        public string Name { get; set; }
        public int FCoord { get; set; }
        public int SCoord { get; set; }
        public ConsoleColor Color { get; set; }

        #endregion

        public Model(string name, int fCorod, int sCoord, ConsoleColor color)
        {
            Name = name;
            FCoord = fCorod;
            SCoord = sCoord;
            Color = color;
        }

        /// <summary>
        /// Քարի դիրքի կոորդինատների տեղադրում
        /// </summary>
        /// <param name="numF">Քարի առաջին կոորրդինատ՝ տառ</param>
        /// <param name="numS">Քարի երկրորդ կոորրդինատ՝ թիվ</param>
        public void SetPosition(int numF, int numS)
        {
            Console.SetCursorPosition(2 +(numF-1)*4,1+(numS-1)*2);
            Console.ForegroundColor = Color;
            Console.WriteLine(this.Name[0]);
            this.FCoord = numF;
            this.SCoord = numS;
            Console.ResetColor();
        }
    }
}
