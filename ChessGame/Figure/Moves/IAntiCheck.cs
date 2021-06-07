using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public interface IAntiCheck
    {
        public List<CoordinatePoint> MovesWithKingIsNotUnderCheck(List<BaseFigure> othereFigures, BaseFigure availableMoves)
        {
            var thisKing = (BaseFigure)othereFigures.Where(c => c.Color == availableMoves.Color && c is King).Single();
            var models = othereFigures.Where(f => f.Color != availableMoves.Color);
            var king = (King)thisKing;
            var temp = availableMoves.Coordinate;
            var goodMoves = new List<CoordinatePoint>();
            IAvailableMoves figure = (IAvailableMoves)availableMoves;
            foreach (var item in figure.AvailableMoves(othereFigures))
            {
                if (CheckedCurrentFigure(item, othereFigures, out BaseFigure tempFigure))
                {
                    availableMoves.Coordinate = item;
                    tempFigure.Coordinate = new CoordinatePoint(808, 808);
                    if (!DangerPosition(othereFigures, availableMoves).Contains(thisKing.Coordinate))
                        goodMoves.Add(item);
                    tempFigure.Coordinate = item;
                }
                else
                {
                    availableMoves.Coordinate = item;
                    if (!DangerPosition(othereFigures, availableMoves).Contains(thisKing.Coordinate))
                        goodMoves.Add(item);
                }
            }
            availableMoves.Coordinate = temp;
            return goodMoves;
        }

        /// <summary>
        /// Check the danger position for current king
        /// </summary>
        /// <param name="model">King instance withe or Black</param>
        /// <returns>Return danger position List for current king </returns>
        public List<CoordinatePoint> DangerPosition(List<BaseFigure> othereFigures, BaseFigure baseFigure)
        {
            var modelNew = othereFigures.Where(c => c.Color != baseFigure.Color);
            var result = new List<CoordinatePoint>();
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                var array = temp.AvailableMoves(othereFigures);
                result.AddRange(array);
            }
            return result.Distinct().ToList();
        }

        /// <summary>
        /// Check a current figure with a coordinate
        /// </summary>
        /// <param name="coordinatePoint">Current coordinate</param>
        /// <returns>Return a current figure</returns>
        private static bool CheckedCurrentFigure(CoordinatePoint coordinatePoint, List<BaseFigure> otherFigures, out BaseFigure baseFigure)
        {
            foreach (var item in otherFigures)
            {
                if (item.Coordinate == coordinatePoint)
                {
                    baseFigure = item;
                    return true;
                }
            }
            baseFigure = null;
            return false;
        }
    }
}
