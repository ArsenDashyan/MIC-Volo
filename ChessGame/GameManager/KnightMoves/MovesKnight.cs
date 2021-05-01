using Figure;
using System.Collections.Generic;

namespace GameManager
{
    public class MovesKnight
    {
        #region Property and Feld
        private static List<BaseFigure> models = new();
        public event Picture setPicture;
        public event Message messageForMove;
        private static Knight startKnight;
        private static Knight targetKnight;
        private int count = 1;
        #endregion

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
            startKnight = new Knight("Knight.Black.1", FColor.Black, models);
            models.Add(startKnight);
            string[] point = coordinate.Split('.');
            var coordinatPoint = new CoordinatePoint(int.Parse(point[0]),int.Parse(point[1]));
            startKnight.setPicture += SetFigurePicture;
            startKnight.messageForMove += MessageForMove;
            startKnight.SetFigurePosition(coordinatPoint);
        }
        public void CreateTargetKnight(string coordinate)
        {
            targetKnight = new Knight("Knight.Black.2", FColor.White, models);
            models.Add(targetKnight);
            string[] point = coordinate.Split('.');
            var coordinatPoint = new CoordinatePoint(int.Parse(point[0]), int.Parse(point[1]));
            targetKnight.setPicture += SetFigurePicture;
            targetKnight.messageForMove += MessageForMove;
            targetKnight.SetFigurePosition(coordinatPoint);
        }
        private void SetFigurePicture(object sender, string coordinate)
        {
            setPicture(this, coordinate);
        }
        private void MessageForMove(object sender, (string,string) coordinate)
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
