using System.Collections.Generic;
using System.Linq;

namespace Figure
{
    public delegate void Picture(object sender, string e);
    public delegate void Message(object sender, (string, string) e);
    public class BaseFigure : ISetPosition
    {
        public readonly List<BaseFigure> othereFigures;

        #region Property and Feld
        public string Name { get; set; }
        public FColor Color { get; set; }

        public bool isMoved = false;

        public bool isProtected = false;
        public CoordinatePoint Coordinate { get; set; }
        public event Picture SetPicture;
        public event Picture RemovePicture;
        public event Picture DeletePicture;
        public event Message MessageForMove;

        #endregion

        public BaseFigure(List<BaseFigure> othereFigures)
        {
            this.othereFigures = othereFigures;
        }
        public void SetFigurePosition(CoordinatePoint coordinate)
        {
            RemoveFigurePosition();
            string targetCoordinate = coordinate.ToString() + '.' + this.Name;
            string currenrCoordinate;
            if (this.Coordinate == null)
            {
                MessageForMove(this, (string.Empty, targetCoordinate));
                this.Coordinate = coordinate;
                currenrCoordinate = this.Coordinate.ToString() + '.' + this.Name;
            }
            else
            {
                currenrCoordinate = this.Coordinate.ToString() + '.' + this.Name;
                MessageForMove(this, (currenrCoordinate, targetCoordinate));
                this.Coordinate = coordinate;
            }
            DeleteFigur(this);
            SetPicture(this, targetCoordinate);
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
        private void DeleteFigur(BaseFigure model)
        {
            var modelTemp = othereFigures.Where(c => c.Color != model.Color);
            foreach (var item in modelTemp)
            {
                if (model.Coordinate == item.Coordinate)
                {
                    var tempItem = item;
                    string itemCoordinate = item.Coordinate.ToString() + '.' + item.Name;
                    RemovePicture(item, itemCoordinate);
                    DeletePicture(item, itemCoordinate);
                    othereFigures.Remove(item);
                    break;
                }
            }
        }
    }
}
