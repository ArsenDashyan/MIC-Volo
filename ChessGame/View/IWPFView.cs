using Coordinates;
using System;
using System.Linq;

namespace View
{
    public interface IWPFView
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public BitmapImage Bitmap { get; set; }
        public Image FigureImage { get; set; }
        public CoordinatePoint Coordinate { get; set; }
        public void SetFigurePosition(CoordinatePoint coordinate, Grid grid)
        {
            RemoveFigureFromBoard(this, grid);
            this.FigureImage = new Image();
            this.FigureImage.Source = this.Bitmap;
            Grid.SetColumn(FigureImage, (int)coordinate.X);
            Grid.SetRow(FigureImage, (int)coordinate.Y);
            this.Coordinate = coordinate;
            grid.Children.Add(FigureImage);
            FigureImage.Name = this.Name;
            DeleteFigur(this, grid);
        }
        public void RemoveFigureFromBoard(object figure, Grid grid)
        {
            if (figure.Coordinate != null)
            {
                this.FigureImage.Source = figure.Bitmap;
                Grid.SetColumn(this.FigureImage, (int)figure.Coordinate.X);
                Grid.SetRow(this.FigureImage, (int)figure.Coordinate.Y);
                grid.Children.Remove(this.FigureImage);
            }
        }
        public void DeleteFigur(Object model, Grid grid)
        {
            var modelTemp = othereFigures.Where(c => c.Color != model.Color);
            foreach (var item in modelTemp)
            {
                if (model.Coordinate == item.Coordinate)
                {
                    ISetPosition tempItem = (ISetPosition)item;
                    tempItem.RemoveFigureFromBoard(item, grid);
                    othereFigures.Remove(item);
                    break;
                }
            }
        }
    }
}
