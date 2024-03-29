﻿using System;
using System.Collections.Generic;
using System.Linq;
using Figure;

namespace Helper
{
    public static class Helper
    {
        /// <summary>
        /// Check the game is baby or no
        /// </summary>
        /// <param name="list">Current figur last fore moves</param>
        /// <returns>Return true if figure move is baby</returns>
        public static bool BabyGame(this List<CoordinatePoint> list)
        {
            var tempOne = new List<CoordinatePoint>();
            var tempTwo = new List<CoordinatePoint>();
            if (list.Count >= 4)
            {
                for (int i = list.Count - 1; i >= list.Count - 3; i -= 2)
                {
                    tempOne.Add(list[i]);
                }
                for (int i = list.Count - 2; i >= list.Count - 3; i -= 2)
                {
                    tempTwo.Add(list[i]);
                }
            }
            var tempEndOne = tempOne.Distinct().ToList();
            var tempEndTwo = tempTwo.Distinct().ToList();
            if (tempEndOne.Count == 1 && tempEndTwo.Count == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check current king position half
        /// </summary>
        /// <param name="coordinat">Current king coordinate</param>
        /// <returns>Return the half with inside current king</returns>
        public static int GetCurrentKingHalf(this CoordinatePoint coordinat)
        {
            if (coordinat.X >= 4 && coordinat.Y <= 3)
            {
                return 1;
            }
            else if (coordinat.X >= 4 && coordinat.Y >= 4)
            {
                return 4;
            }
            else if (coordinat.X <= 3 && coordinat.Y <= 3)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        /// <summary>
        /// Universal filtr method for IEnumerable collections
        /// </summary>
        /// <typeparam name="T">Type for collectios</typeparam>
        /// <param name="list">Collections name</param>
        /// <param name="func">Funcon for filtr</param>
        /// <returns></returns>
        public static bool Filtr<T>(this IEnumerable<T> list, Func<T, bool> func)
        {
            foreach (var item in list)
            {
                if (func(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Universal filtr method for IEnumerable collections
        /// </summary>
        /// <typeparam name="T">Type for collectios</typeparam>
        /// <param name="list">Collections name</param>
        /// <param name="func">Funcon for filtr</param>
        /// <returns></returns>
        public static IEnumerable<T> FiltrFor<T>(this IEnumerable<T> list, Func<T, bool> func)
        {
            foreach (var item in list)
            {
                if (func(item))
                    yield return item;
            }
        }
    }
}
