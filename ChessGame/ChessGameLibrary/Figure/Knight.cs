﻿using System;
using System.Collections.Generic;
using System.Linq;
using Coordinats;

namespace ChessGameLibrary
{
    public class Knights : FigureBase, ICrosswise, IRandomMove
    {
        static int count = 1;
        public Knights(string name, ConsoleColor color, List<FigureBase> othereFigures) : base(othereFigures)
        {
            Name = name;
            Color = color;
        }

        #region Move
        public List<CoordinatPoint> Horizontal()
        {
            List<CoordinatPoint> result = new List<CoordinatPoint>();
            var model =othereFigures.Where(c => c != this).ToList();
            if (this.Coordinate.Y + 2 >= 1 && this.Coordinate.Y + 2 <= 8)
            {
                if (this.Coordinate.X - 1 >= 1 && this.Coordinate.X - 1 <= 8)
                    result.Add(new CoordinatPoint(this.Coordinate.X - 1, this.Coordinate.Y + 2));
                if (this.Coordinate.X + 1 <= 8 && this.Coordinate.X + 1 >= 1)
                    result.Add(new CoordinatPoint(this.Coordinate.X + 1, this.Coordinate.Y + 2));
            }
            if (this.Coordinate.Y - 2 <= 8 && this.Coordinate.Y - 2 >= 1)
            {
                if (this.Coordinate.X - 1 >= 1 && this.Coordinate.X - 1 <= 8)
                    result.Add(new CoordinatPoint(this.Coordinate.X - 1, this.Coordinate.Y - 2));
                if (this.Coordinate.X + 1 <= 8 && this.Coordinate.X + 1 >= 1)
                    result.Add(new CoordinatPoint(this.Coordinate.X + 1, this.Coordinate.Y - 2));
            }
            foreach (var item in model)
            {
                if (result.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        if (result.IndexOf(this.Coordinate) < result.IndexOf(item.Coordinate))
                            result = result.Where(c => result.IndexOf(c) < result.IndexOf(item.Coordinate)).ToList();
                        else
                            result = result.Where(c => result.IndexOf(c) > result.IndexOf(item.Coordinate)).ToList();
                    }
                    else
                    {
                        if (result.IndexOf(this.Coordinate) < result.IndexOf(item.Coordinate))
                            result = result.Where(c => result.IndexOf(c) <= result.IndexOf(item.Coordinate)).ToList();
                        else
                            result = result.Where(c => result.IndexOf(c) >= result.IndexOf(item.Coordinate)).ToList();
                    }
                }
            }
            return result;
        }
        public List<CoordinatPoint> Vertical()
        {
            List<CoordinatPoint> result = new List<CoordinatPoint>();
            var model =othereFigures.Where(c => c != this).ToList();
            if (this.Coordinate.Y + 1 >= 1 && this.Coordinate.Y + 1 <= 8)
            {
                if (this.Coordinate.X - 2 >= 1 && this.Coordinate.X - 2 <= 8)
                    result.Add(new CoordinatPoint(this.Coordinate.X - 2, this.Coordinate.Y + 1));
                if (this.Coordinate.X + 2 <= 8 && this.Coordinate.X + 2 >= 1)
                    result.Add(new CoordinatPoint(this.Coordinate.X + 2, this.Coordinate.Y + 1));
            }
            if (this.Coordinate.Y - 1 <= 8 && this.Coordinate.Y - 1 >= 1)
            {
                if (this.Coordinate.X - 2 >= 1 && this.Coordinate.X - 2 <= 8)
                    result.Add(new CoordinatPoint(this.Coordinate.X - 2, this.Coordinate.Y - 1));
                if (this.Coordinate.X + 2 <= 8 && this.Coordinate.X + 2 >= 1)
                    result.Add(new CoordinatPoint(this.Coordinate.X + 2, this.Coordinate.Y - 1));
            }
            foreach (var item in model)
            {
                if (result.Contains(item.Coordinate))
                {
                    if (item.Color == this.Color)
                    {
                        if (result.IndexOf(this.Coordinate) < result.IndexOf(item.Coordinate))
                            result = result.Where(c => result.IndexOf(c) < result.IndexOf(item.Coordinate)).ToList();
                        else
                            result = result.Where(c => result.IndexOf(c) > result.IndexOf(item.Coordinate)).ToList();
                    }
                    else
                    {
                        if (result.IndexOf(this.Coordinate) < result.IndexOf(item.Coordinate))
                            result = result.Where(c => result.IndexOf(c) <= result.IndexOf(item.Coordinate)).ToList();
                        else
                            result = result.Where(c => result.IndexOf(c) >= result.IndexOf(item.Coordinate)).ToList();
                    }
                }
            }
            return result;
        }
        public List<CoordinatPoint> Crosswise()
        {
            var arrayHor = this.Horizontal();
            var arrayVert = this.Vertical();
            arrayHor.AddRange(arrayVert);
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
        public bool IsProtected()
        {
            var model =othereFigures.Where(c => c != this && c.Color == this.Color).ToList();
            foreach (var item in model)
            {
                IAvailableMoves tempfigur = (IAvailableMoves)item;
                if (tempfigur.AvailableMoves().Contains(this.Coordinate))
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsProtected(CoordinatPoint CoordinatPoint)
        {
            var model =othereFigures.Where(c => c != this && c.Color == this.Color).ToList();
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
                    if (CoordinatPoint.Modul(item, king.Coordinate) >= 2d)
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
        public int MinCount(CoordinatPoint CoordinatPoint)
        {
            var knightMoves = KnightMove.Crosswise(this.Coordinate);
            var endMoves = KnightMove.Crosswise(CoordinatPoint);
            if (knightMoves.Contains((CoordinatPoint)))
            {
                return count;
            }
            else
            {
                count++;
                if (KnightMove.Equals(this.Coordinate, CoordinatPoint))
                {
                    return count;
                }
                else
                {
                    count++;
                    if (KnightMove.Equals(knightMoves, CoordinatPoint))
                    {
                        return count;
                    }
                    else
                    {
                        count++;
                        if (KnightMove.Equals(knightMoves, endMoves))
                        {
                            return count;
                        }
                        else
                        {
                            count++;
                            if (KnightMove.EqualsEnd(knightMoves, endMoves))
                            {
                                return count;
                            }
                            else
                            {
                                return 6;
                            }
                        }
                    }
                }
            }
        }
        public bool IsUnderAttack(CoordinatPoint CoordinatPoint, CoordinatPoint CoordinatPoint1)
        {
            if (CoordinatPoint.Modul(CoordinatPoint1, CoordinatPoint) < 2d)
            {
                return true;
            }
            return false;
        }
    }
}