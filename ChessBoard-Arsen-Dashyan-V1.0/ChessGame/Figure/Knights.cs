using System;
using System.Collections.Generic;
using Utility;
using static Utility.KnightMove;

namespace ChessGame
{
    public class Knights : Model, ICrosswise
    {
        static int count = 1;
        public Knights(string name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }
        public List<(int, int)> Horizontal()
        {
            List<(int, int)> result = new List<(int, int)>();

            if (this.SCoord + 2 >= 1 && this.SCoord + 2 <= 8)
            {
                if (this.FCoord - 1 >= 1 && this.FCoord - 1 <= 8)
                    result.Add((this.FCoord - 1, this.SCoord + 2));
                if (this.FCoord + 1 <= 8 && this.FCoord + 1 >= 1)
                    result.Add((this.FCoord + 1, this.SCoord + 2));
            }
            if (this.SCoord - 2 <= 8 && this.SCoord - 2 >= 1)
            {
                if (this.FCoord - 1 >= 1 && this.FCoord - 1 <= 8)
                    result.Add((this.FCoord - 1, this.SCoord - 2));
                if (this.FCoord + 1 <= 8 && this.FCoord + 1 >= 1)
                    result.Add((this.FCoord + 1, this.SCoord - 2));
            }
            return result;
        }
        public List<(int, int)> Vertical()
        {
            List<(int, int)> result = new List<(int, int)>();

            if (this.SCoord + 1 >= 1 && this.SCoord + 1 <= 8)
            {
                if (this.FCoord - 2 >= 1 && this.FCoord - 2 <= 8)
                    result.Add((this.FCoord - 2, this.SCoord + 1));
                if (this.FCoord + 2 <= 8 && this.FCoord + 2 >= 1)
                    result.Add((this.FCoord + 2, this.SCoord + 1));
            }
            if (this.SCoord - 1 <= 8 && this.SCoord - 1 >= 1)
            {
                if (this.FCoord - 2 >= 1 && this.FCoord - 2 <= 8)
                    result.Add((this.FCoord - 2, this.SCoord - 1));
                if (this.FCoord + 2 <= 8 && this.FCoord + 2 >= 1)
                    result.Add((this.FCoord + 2, this.SCoord - 1));
            }
            return result;
        }
        public List<(int, int)> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
            return arrayHor;
        }
        public List<(int, int)> AvailableMoves()
        {
            return this.Crosswise();
        }
        public int MinCount(int eF, int eS)
        {
            var knightMoves = KnightMove.Crosswise(this.FCoord, this.SCoord);
            var endMoves = KnightMove.Crosswise(eF, eS);
            if (knightMoves.Contains((eF, eS)))
            {
                return count;
            }
            else
            {
                count++;
                if (KnightMove.Equals(this.FCoord, this.SCoord, eF, eS))
                {
                    return count;
                }
                else
                {
                    count++;
                    if (KnightMove.Equals(knightMoves, eF, eS))
                    {
                        return count;
                    }
                    else
                    {
                        count++;
                        if (KnightMove.Equals(knightMoves, endMoves))
                        {
                            return count;
                        }
                        else
                        {
                            count++;
                            if (KnightMove.EqualsEnd(knightMoves, endMoves))
                            {
                                return count;
                            }
                            else
                            {
                                return 6;
                            }
                        }
                    }
                }
            }
        }
    }
}
