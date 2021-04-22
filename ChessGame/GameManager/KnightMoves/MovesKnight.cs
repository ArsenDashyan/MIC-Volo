using Figure;
using System.Collections.Generic;

namespace GameManager
{
    public class MovesKnight
    {
        private int count = 1;
        public static List<BaseFigure> models = new List<BaseFigure>();
        public event Picture setPicture;
        public event Message messageForMove;
        private static Knight startKnight;
        private static Knight targetKnight;

        public int MinKnightCount()
        {
            CoordinatePoint coordinatPoint = startKnight.Coordinate;
            CoordinatePoint coordinatPoint1 = targetKnight.Coordinate;
            var knightMoves = KnightMove.Crosswise(coordinatPoint1);
            var endMoves = KnightMove.Crosswise(coordinatPoint);
            if (knightMoves.Contains((coordinatPoint)))
            {
                return count;
            }
            else
            {
                count++;
                if (KnightMove.Equals(coordinatPoint1, coordinatPoint))
                {
                    return count;
                }
                else
                {
                    count++;
                    if (KnightMove.Equals(knightMoves, coordinatPoint))
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
        public void CreateStartKnight(string coordinate)
        {
            startKnight = new Knight("Knight.Black.1", "Black", models);
            models.Add(startKnight);
            string[] point = coordinate.Split('.');
            CoordinatePoint coordinatPoint = new CoordinatePoint(int.Parse(point[0]),int.Parse(point[1]));
            startKnight.setPicture += SetFigurePicture;
            startKnight.messageForMove += MessageForMove;
            startKnight.SetFigurePosition(coordinatPoint);
        }
        public void CreateTargetKnight(string coordinate)
        {
            targetKnight = new Knight("Knight.Black.2", "Black", models);
            models.Add(targetKnight);
            string[] point = coordinate.Split('.');
            CoordinatePoint coordinatPoint = new CoordinatePoint(int.Parse(point[0]), int.Parse(point[1]));
            targetKnight.setPicture += SetFigurePicture;
            targetKnight.messageForMove += MessageForMove;
            targetKnight.SetFigurePosition(coordinatPoint);
        }
        public void SetFigurePicture(object sender, string coordinate)
        {
            setPicture(this, coordinate);
        }
        public void MessageForMove(object sender, (string,string) coordinate)
        {
            messageForMove(this, coordinate);
        }
        public List<string> GetNamesForReset()
        {
            var positions = new List<string>();
            if (models.Count != 0)
            {
                foreach (var item in models)
                {
                    positions.Add(item.Name);
                }
                models.Clear();
            }
            else
            {
                positions.Add("0");
            }
            return positions;
        }
    }
}
