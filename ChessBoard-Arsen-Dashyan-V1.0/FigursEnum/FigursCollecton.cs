namespace FigursEnum
{
    public class FigursCollecton
    {
        public enum Figurs : byte
        {
            None = 0b_0000_0000,
            King = 0b_0000_0001,
            Knight = 0b_0000_0010,
            Queen = 0b_0000_0011,
            RookL = 0b_0000_0100,
            RookR = 0b_0000_0101,
            KingB = 0b_0000_0111,
        }
    }
}
