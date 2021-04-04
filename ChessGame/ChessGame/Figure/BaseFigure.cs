using Coordinats;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ChessGame
{
    public class BaseFigure : ISetPosition
    {
        protected readonly List<BaseFigure> othereFigures;
        #region Property and Feld
        public string Name { get; set; }
        public string Color { get; set; }
        public CoordinatPoint Coordinate { get; set; }
        public BitmapImage Bitmap { get; set; }
        public Image FigureImage { get; set; }
        #endregion

        public BaseFigure(List<BaseFigure> othereFigures)
        {
            this.othereFigures = othereFigures;
        }
        public void SetFigurePosition(CoordinatPoint coordinate, Grid grid)
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
        public void RemoveFigureFromBoard(BaseFigure figure, Grid grid)
        {
            if (figure.Coordinate != null)
            {
                this.FigureImage.Source = figure.Bitmap;
                Grid.SetColumn(this.FigureImage, (int)figure.Coordinate.X);
                Grid.SetRow(this.FigureImage, (int)figure.Coordinate.Y);
                grid.Children.Remove(this.FigureImage);
            }
        }
        private void DeleteFigur(BaseFigure model, Grid grid)
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
