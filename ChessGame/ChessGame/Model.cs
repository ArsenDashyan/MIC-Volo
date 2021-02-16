using System;
using System.Linq;

namespace ChessGame
{
    public class Model
    {
        public string Name { get; set; }
        public int FCoord { get; set; }
        public int SCoord { get; set; }
        public ConsoleColor Color { get; set; }

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
            char ch = this.Name[0];
            Console.WriteLine(ch);
            this.FCoord = numF;
            this.SCoord = numS;
            Console.ResetColor();
        }
        /// <summary>
        /// Տվյալ քարի դիրքը
        /// </summary>
        /// <returns>Վերադարձնում է տվյալ քարի դիրքի երկու կոորդինաատ կորտեժի տեսքով </returns>
        public (int,int) GetPosition()
        {
            return (this.FCoord, this.SCoord);
        }
    }
}
