using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public class ValidMoves
    {

        /// <summary>
        /// Check the danger position for current king
        /// </summary>
        /// <param name="model">King instance withe or Black</param>
        /// <returns>Return danger position List for current king </returns>
        private static List<CoordinatePoint> DangerPosition(BaseFigure model)
        {
            var modelNew = model.othereFigures.Where(c => c.Color != model.Color);
            var result = new List<CoordinatePoint>();
            foreach (var item in modelNew)
            {
                var temp = (IAvailableMoves)item;
                var array = temp.AvailableMoves();
                result.AddRange(array);
            }
            return result;
        }

        /// <summary>
        /// Check the available moves for current king 
        /// </summary>
        /// <returns>Return the list</returns>
        public static List<CoordinatePoint> GetCurrentKingMoves(King king)
        {
            var currentKing = (IAvailableMoves)king;
            var result = new List<CoordinatePoint>();
            foreach (var item in currentKing.AvailableMoves())
            {
                if (!DangerPosition(king).Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }
       
    }
}
