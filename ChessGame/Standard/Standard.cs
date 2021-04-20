using Figure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StandardGame
{
    public class Standard
    {
        public readonly List<BaseFigure> figures = new();
        public static List<BaseFigure> models = new();
        private string currentFigureColor;
        private BaseFigure CurentKing => (BaseFigure)figures.Where(c => c.Color == currentFigureColor && c is King).Single();

        public Standard(string currentFigureColor)
        {
            this.currentFigureColor = currentFigureColor;
            figures = GetAllFigures();
            models = figures;
        }

        public static BaseFigure CheckTheBaseFigure(CoordinatePoint coordinatePoint)
        {
            BaseFigure baseFigure = null;
            foreach (var item in models)
            {
                if (item.Coordinate == coordinatePoint)
                {
                    baseFigure = item;
                    break;
                }
            }
            return baseFigure;
        }

        public bool IsValidCoordinate(CoordinatePoint coordinatePoint)
        {
            BaseFigure tempFigure = null;
            foreach (var item in figures)
            {
                if (item.Coordinate == coordinatePoint)
                {
                    tempFigure = item;
                    break;
                }
            }
            IAvailableMoves figure = (IAvailableMoves)tempFigure;
            if (figure.AvailableMoves().Contains(coordinatePoint))
            {
                return true;
            }
            return false;
            
        }
        private List<BaseFigure> GetAllFigures()
        {
            Queen queenWhite = new("Queen.White.1", "White", figures);
            figures.Add(queenWhite);
            King kingWhite = new("King.White.1", "White", figures);
            figures.Add(kingWhite);
            Knight knightWhite1 = new("Knight.White.1", "White", figures);
            figures.Add(knightWhite1);
            Knight knightWhite2 = new("Knight.White.2", "White", figures);
            figures.Add(knightWhite2);
            Bishop bishopWhite1 = new("Bishop.White.1", "White", figures);
            figures.Add(bishopWhite1);
            Bishop bishopWhite2 = new("Bishop.White.2", "White", figures);
            figures.Add(bishopWhite2);
            Rook rookWhite1 = new("Rook.White.1", "White", figures);
            figures.Add(rookWhite1);
            Rook rookWhite2 = new("Rook.White.2", "White", figures);
            figures.Add(rookWhite2);
            Pawn pawnWhite1 = new("Pawn.White.1", "White", figures);
            figures.Add(pawnWhite1);
            Pawn pawnWhite2 = new("Pawn.White.2", "White", figures);
            figures.Add(pawnWhite2);
            Pawn pawnWhite3 = new("Pawn.White.3", "White", figures);
            figures.Add(pawnWhite3);
            Pawn pawnWhite4 = new("Pawn.White.4", "White", figures);
            figures.Add(pawnWhite4);
            Pawn pawnWhite5 = new("Pawn.White.5", "White", figures);
            figures.Add(pawnWhite5);
            Pawn pawnWhite6 = new("Pawn.White.6", "White", figures);
            figures.Add(pawnWhite6);
            Pawn pawnWhite7 = new("Pawn.White.7", "White", figures);
            figures.Add(pawnWhite7);
            Pawn pawnWhite8 = new("Pawn.White.8", "White", figures);
            figures.Add(pawnWhite8);

            Queen queenBlack = new("Queen.Black.1", "Black", figures);
            figures.Add(queenBlack);
            King kingBlack = new("King.Black.1", "Black", figures);
            figures.Add(kingBlack);
            Knight knightBlack1 = new("Knight.Black.1", "Black", figures);
            figures.Add(knightBlack1);
            Knight knightBlack2 = new("Knight.Black.2", "Black", figures);
            figures.Add(knightBlack2);
            Bishop bishopBlack1 = new("Bishop.Black.1", "Black", figures);
            figures.Add(bishopBlack1);
            Bishop bishopBlack2 = new("Bishop.Black.2", "Black", figures);
            figures.Add(bishopBlack2);
            Rook rookBlack1 = new("Rook.Black.1", "Black", figures);
            figures.Add(rookBlack1);
            Rook rookBlack2 = new("Rook.Black.2", "Black", figures);
            figures.Add(rookBlack2);
            Pawn pawnBlack1 = new("Pawn.Black.1", "Black", figures);
            figures.Add(pawnBlack1);
            Pawn pawnBlack2 = new("Pawn.Black.2", "Black", figures);
            figures.Add(pawnBlack2);
            Pawn pawnBlack3 = new("Pawn.Black.3", "Black", figures);
            figures.Add(pawnBlack3);
            Pawn pawnBlack4 = new("Pawn.Black.4", "Black", figures);
            figures.Add(pawnBlack4);
            Pawn pawnBlack5 = new("Pawn.Black.5", "Black", figures);
            figures.Add(pawnBlack5);
            Pawn pawnBlack6 = new("Pawn.Black.6", "Black", figures);
            figures.Add(pawnBlack6);
            Pawn pawnBlack7 = new("Pawn.Black.7", "Black", figures);
            figures.Add(pawnBlack7);
            Pawn pawnBlack8 = new("Pawn.Black.8", "Black", figures);
            figures.Add(pawnBlack8);

            return figures;
        }

        /// <summary>
        /// Check the available moves for current king 
        /// </summary>
        /// <returns>Return the list</returns>
        private List<CoordinatePoint> GetCurrentKingMoves()
        {
            IRandomMove currentKing = (IRandomMove)CurentKing;
            var result = new List<CoordinatePoint>();
            foreach (var item in currentKing.AvailableMoves())
            {
                if (!DangerPosition(CurentKing).Contains(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Check the danger position for current king
        /// </summary>
        /// <param name="model">King instance withe or Black</param>
        /// <returns>Return danger position List for current king </returns>
        private List<CoordinatePoint> DangerPosition(BaseFigure model)
        {
            var modelNew = figures.Where(c => c.Color != model.Color);
            var result = new List<CoordinatePoint>();
            foreach (var item in modelNew)
            {
                var temp = (IDangerMoves)item;
                var array = temp.DangerMoves();
                result.AddRange(array);
            }
            return result;
        }
    }
}
