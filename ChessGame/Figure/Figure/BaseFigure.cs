using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public delegate void Picture(object sender, string e);
    public delegate void Message(object sender, (string, string) e);
    public abstract class BaseFigure : ISetPosition
    {
        #region Property and Feld
        public string Name { get; set; }
        public FColor Color { get; set; }
        public bool isMoved = false;
        public bool isProtected = false;
        public CoordinatePoint Coordinate { get; set; }
        public event Picture SetPicture;
        public event Picture RemovePicture;
        public event Message MessageForMove;

        #endregion

        public BaseFigure(FColor fColor)
        {
            Color = fColor;
        }
        public void SetFigurePosition(CoordinatePoint coordinate)
        {
            RemoveFigurePosition();
            if (this.Coordinate == null)
                MessageForMove(this, (string.Empty, coordinate.ToString() + '.' + this.Name));
            else
                MessageForMove(this, (this.Coordinate.ToString() + '.' + this.Name,
                                      coordinate.ToString() + '.' + this.Name));
            this.Coordinate = coordinate;
            SetPicture(this, coordinate.ToString() + '.' + this.Name);
        }
        public void RemoveFigurePosition()
        {
            if (this.Coordinate != null)
            {
                this.isMoved = true;
                string currenrCoordinate = this.Coordinate.ToString() + '.' + this.Name;
                RemovePicture(this, currenrCoordinate);
            }
        }

    }
}
