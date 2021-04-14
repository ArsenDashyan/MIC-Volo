using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public delegate void SetPicture(object sender, CoordinatePoint e);
    public class BaseFigure : ISetPosition
    {
        protected readonly List<BaseFigure> othereFigures;
        #region Property and Feld
        public string Name { get; set; }
        public string Color { get; set; }
        public CoordinatePoint Coordinate { get; set; }

        public event SetPicture setPicture;

        #endregion

        public BaseFigure(List<BaseFigure> othereFigures)
        {
            this.othereFigures = othereFigures;
        }
        public void SetFigurePosition(CoordinatePoint coordinate)
        {
            this.Coordinate = coordinate;
            setPicture(this, this.Coordinate);
            DeleteFigur(this);
        }
        private void DeleteFigur(BaseFigure model)
        {
            var modelTemp = othereFigures.Where(c => c.Color != model.Color);
            foreach (var item in modelTemp)
            {
                if (model.Coordinate == item.Coordinate)
                {
                    ISetPosition tempItem = (ISetPosition)item;
                    othereFigures.Remove(item);
                    break;
                }
            }
        }
    }
}
