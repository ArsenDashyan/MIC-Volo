using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public delegate void Picture(object sender, CoordinatePoint e);
    public delegate void Message(object sender, (CoordinatePoint, CoordinatePoint) e);

    public class BaseFigure : ISetPosition
    {
        protected readonly List<BaseFigure> othereFigures;
        #region Property and Feld
        public string Name { get; set; }
        public string Color { get; set; }
        public CoordinatePoint Coordinate { get; set; }
        public event Picture setPicture;
        public event Picture removePicture;
        public event Message messageForMove;

        #endregion

        public BaseFigure(List<BaseFigure> othereFigures)
        {
            this.othereFigures = othereFigures;
        }
        public void SetFigurePosition(CoordinatePoint coordinate)
        {
            RemoveFigurePosition();
            messageForMove(this, (this.Coordinate, coordinate));
            this.Coordinate = coordinate;
            DeleteFigur(this);
            setPicture(this, this.Coordinate);
        }
        public void RemoveFigurePosition()
        {
            if (this.Coordinate != null)
            {
                removePicture(this, this.Coordinate);
            }
        }
        private void DeleteFigur(BaseFigure model)
        {
            var modelTemp = othereFigures.Where(c => c.Color != model.Color);
            foreach (var item in modelTemp)
            {
                if (model.Coordinate == item.Coordinate)
                {
                    ISetPosition tempItem = item;
                    removePicture(item, item.Coordinate);
                    othereFigures.Remove(item);
                    break;
                }
            }
        }
    }
}
