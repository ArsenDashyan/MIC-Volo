using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGameLibrary
{
    public class Rook : FigureBase, ICrosswise, IRandomMove
    {
        public Rook(string name, ConsoleColor color, List<FigureBase> othereFigures) : base(othereFigures)
        {
            Name = name;
            Color = color;
        }
        #region Move
        public List<CoordinatPoint> Vertical()
        {
            List<CoordinatPoint> arr = new List<CoordinatPoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = 1; i <= 8; i++)
            {
                CoordinatPoint CoordinatPointTemp = new CoordinatPoint(this.Coordinate.X, i);
                arr.Add(CoordinatPointTemp);
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                    }
                    else
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.Coordinate)).ToList();
                    }
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatPoint> Horizontal()
        {
            List<CoordinatPoint> arr = new List<CoordinatPoint>();
            var model = othereFigures.Where(c => c != this).ToList();
            for (int i = 1; i <= 8; i++)
            {
                CoordinatPoint CoordinatPointTemp = new CoordinatPoint(i, this.Coordinate.Y);
                arr.Add(CoordinatPointTemp);
            }
            foreach (var item in model)
            {
                if (arr.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) < arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) > arr.IndexOf(item.Coordinate)).ToList();
                    }
                    else
                    {
                        if (arr.IndexOf(this.Coordinate) < arr.IndexOf(item.Coordinate))
                            arr = arr.Where(c => arr.IndexOf(c) <= arr.IndexOf(item.Coordinate)).ToList();
                        else
                            arr = arr.Where(c => arr.IndexOf(c) >= arr.IndexOf(item.Coordinate)).ToList();
                    }
                }
            }
            arr.Remove(this.Coordinate);
            return arr;
        }
        public List<CoordinatPoint> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
            arrayHor.Remove(this.Coordinate);
            return arrayHor;
        }
        public List<CoordinatPoint> AvailableMoves()
        {
            return this.Crosswise();
        }

        #endregion
        public bool IsUnderAttack(CoordinatPoint CoordinatPoint)
        {
            var modelNew = othereFigures.Where(c => c.Color == ConsoleColor.Red).ToList();
            foreach (var item in modelNew)
            {
                IAvailableMoves itemFigur = (IAvailableMoves)item;
                if (itemFigur.AvailableMoves().Contains(CoordinatPoint))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsUnderAttack(CoordinatPoint CoordinatPoint, CoordinatPoint CoordinatPoint1)
        {
            if (CoordinatPoint.Modul(CoordinatPoint1, CoordinatPoint) < 2d)
            {
                return true;
            }
            return false;
        }
        public bool IsProtected()
        {
            var model = othereFigures.Where(c => c != this && c.Color == this.Color).ToList();
            foreach (var item in model)
            {
                IAvailableMoves tempfigur = (IAvailableMoves)item;
                if (tempfigur.AvailableMoves().Contains(this.Coordinate))
                    return true;
            }
            return false;
        }
        public bool IsProtected(CoordinatPoint CoordinatPoint)
        {
            var model = othereFigures.Where(c => c != this && c.Color == this.Color).ToList();
            foreach (var item in model)
            {
                IAvailableMoves tempfigur = (IAvailableMoves)item;
                if (tempfigur.AvailableMoves().Contains(CoordinatPoint))
                    return true;
            }
            return false;
        }
        public CoordinatPoint RandomMove(King king)
        {
            CoordinatPoint temp = null;
            if (ProtectedShax(king, out CoordinatPoint tempForItem))
            {
                temp = tempForItem;
                return temp;
            }
            else if (IsUnderAttackShax(king, out CoordinatPoint tempForItem2))
            {
                temp = tempForItem2;
                return temp;
            }
            else if (IsUnderAttackMax(king, out CoordinatPoint tempForItem3))
            {
                temp = tempForItem3;
                return temp;
            }
            if (temp == null)
            {
                foreach (var item in AvailableMoves())
                {
                    if (!IsUnderAttack(item))
                    {
                        temp = item;
                        break;
                    }
                }
            }
            return temp;
        }
        private bool ProtectedShax(King king, out CoordinatPoint tempForItem)
        {
            CoordinatPoint temp = this.Coordinate;
            foreach (var item in AvailableMoves())
            {
                this.Coordinate = item;
                if (IsProtected(item))
                {
                    if (CoordinatPoint.Modul(item, king.Coordinate) >= 2d)
                    {
                        if (AvailableMoves().Contains(king.Coordinate))
                        {
                            tempForItem = item;
                            this.Coordinate = temp;
                            return true;
                        }
                    }
                }
            }
            this.Coordinate = temp;
            tempForItem = null;
            return false;
        }
        private bool IsUnderAttackShax(King king, out CoordinatPoint tempForItem)
        {
            CoordinatPoint temp = this.Coordinate;
            foreach (var item in AvailableMoves())
            {
                this.Coordinate = item;
                if (!IsUnderAttack(this.Coordinate))
                {
                    if (CoordinatPoint.Modul(item, king.Coordinate) >= 2d)
                    {
                        if (AvailableMoves().Contains(king.Coordinate))
                        {
                            tempForItem = item;
                            this.Coordinate = temp;
                            return true;
                        }
                    }
                }
            }
            this.Coordinate = temp;
            tempForItem = null;
            return false;
        }
        private bool IsUnderAttackMax(King king, out CoordinatPoint tempForItem)
        {
            CoordinatPoint temp = this.Coordinate;
            foreach (var item in AvailableMoves())
            {
                this.Coordinate = item;
                if (!IsUnderAttack(this.Coordinate))
                {
                    if (CoordinatPoint.Modul(item, king.Coordinate) > 3d)
                    {
                        if (AvailableMoves().Count == 14)
                        {
                            tempForItem = item;
                            this.Coordinate = temp;
                            return true;
                        }
                    }
                }
            }
            this.Coordinate = temp;
            tempForItem = null;
            return false;
        }
    }
}
