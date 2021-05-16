using Figure;
using System.Collections.Generic;
using System.Linq;

namespace GameManager
{
    public class MovesKnight
    {
        #region Property and Feld
        private static readonly List<BaseFigure> _models = new();
        public event Picture SetPicture;
        public event Message MessageForMoveKnight;
        private static Knight _startKnight;
        private static Knight _targetKnight;
        #endregion

        public static int MinKnightCount()
        {
            int count = 1;
            CoordinatePoint coordinatPoint = _startKnight.Coordinate;
            CoordinatePoint coordinatPoint1 = _targetKnight.Coordinate;
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
            _startKnight = new Knight("Knight.Black.1", FColor.Black, _models);
            _models.Add(_startKnight);
            string[] point = coordinate.Split('.');
            var coordinatPoint = new CoordinatePoint(int.Parse(point[0]),int.Parse(point[1]));
            _startKnight.SetPicture += SetFigurePicture;
            _startKnight.MessageForMove += MessageForMove;
            _startKnight.SetFigurePosition(coordinatPoint);
        }
        public void CreateTargetKnight(string coordinate)
        {
            _targetKnight = new Knight("Knight.Black.2", FColor.White, _models);
            _models.Add(_targetKnight);
            string[] point = coordinate.Split('.');
            var coordinatPoint = new CoordinatePoint(int.Parse(point[0]), int.Parse(point[1]));
            _targetKnight.SetPicture += SetFigurePicture;
            _targetKnight.MessageForMove += MessageForMove;
            _targetKnight.SetFigurePosition(coordinatPoint);
        }
        private void SetFigurePicture(object sender, string coordinate)
        {
            SetPicture(this, coordinate);
        }
        private void MessageForMove(object sender, (string,string) coordinate)
        {
            MessageForMoveKnight(this, coordinate);
        }
        public static List<string> GetNamesForReset()
        {
            var positions = new List<string>();
            if (_models.Count != 0)
            {
                foreach (var item in _models)
                {
                    positions.Add(item.Name);
                }
                _models.Clear();
            }
            else
                positions = null;
            return positions;
        }
    }
}
