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
        public event Picture SetFigurePicture;
        public event Picture RemovePicture;
        public event Message MessageForMove;

        #endregion

        public BaseFigure(string name, FColor fColor)
        {
            Color = fColor;
            Name = name;
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
            SetFigurePicture(this, coordinate.ToString() + '.' + this.Name);
        }
        public void RemoveFigurePosition()
        {
            if (this.Coordinate != null)
            {
                this.isMoved = true;
                RemovePicture(this, this.Coordinate.ToString() + '.' + this.Name);
            }
        }
        public override string ToString()
        {
            return $"{this.Name}|{this.Coordinate}|{this.isMoved}|{this.isProtected}";
        }
    }
}
