using System;
using System.Collections.Generic;
using System.Text;

namespace Chess.Figure
{
    class Old
    {
        //    public static void ChessLogic()
        //    {
        //        View.ShowBoard(king.FCoord, king.SCoord);

        //        var tuple = GetCoordinats();
        //        bool isAction = (Math.Abs(king.FCoord - tuple.Item1) <= 1 & Math.Abs(king.SCoord - tuple.Item2) <= 1);

        //        if (isAction)
        //        {
        //            View.ShowBoard(tuple.Item1, tuple.Item2);

        //            while (count <= 4)
        //            {
        //                Thread.Sleep(1500);
        //                View.ShowBoard(tuple.Item1, tuple.Item2, count);
        //                if (count != 4)
        //                {
        //                    tuple = GetCoordinats();
        //                    if ((Math.Abs(king.FCoord - tuple.Item1) <= 1 & Math.Abs(king.SCoord - tuple.Item2) <= 1))
        //                    {
        //                        View.ShowBoard(tuple.Item1, tuple.Item2, count);
        //                    }
        //                    else
        //                    {
        //                        Console.SetCursorPosition(40, 6);
        //                        Console.ForegroundColor = ConsoleColor.Red;
        //                        Console.WriteLine("Non correct action");
        //                        Console.ResetColor();
        //                        Console.SetCursorPosition(0, 18);
        //                        break;
        //                    }
        //                }
        //                count++;
        //            }
        //            Console.WriteLine("\n\nGame over");
        //        }
        //        else
        //        {
        //            Console.SetCursorPosition(40, 6);
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("Non correct action");
        //            Console.ResetColor();
        //            Console.SetCursorPosition(0, 18);
        //        }
        //    }

        //private static (int, int) GetCoordinats()
        //{
        //    Console.SetCursorPosition(40, 0);
        //    Console.WriteLine("Please enter a letter coordinate");
        //    Console.SetCursorPosition(40, 1);
        //    char a = char.Parse(Console.ReadLine());
        //    int aFirst = GetLetters(a);
        //    Console.SetCursorPosition(40, 2);
        //    Console.WriteLine("Please enter a number coordinate");
        //    Console.SetCursorPosition(40, 3);
        //    int b = int.Parse(Console.ReadLine());

        //    if (IsKingActionForFirstGame(aFirst, b))
        //        return (aFirst, b);

        //    while (!IsKingActionForFirstGame(aFirst, b))
        //    {
        //        Console.SetCursorPosition(40, 4);
        //        Console.WriteLine("Please enter a correct coordinats");
        //        (aFirst, b) = GetCoordinats();
        //        break;
        //    }

        //    return (aFirst, b);
        //}
        //public static bool IsKingActionForFirstGame(int a, int b)
        //{
        //    var arrayRight = GetRightIndex(queen.FCoord, queen.SCoord);
        //    var arrayLeft = GetLeftIndex(queen.FCoord, queen.SCoord);

        //    if (a > queen.FCoord && b < rookL.SCoord && b < queen.SCoord && !arrayLeft.Contains((a, b)) && !arrayRight.Contains((a, b)))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        /// <summary>
        /// Ցուցադրում է խաղադաշտի սկզբնական տեսքը
        /// </summary>
        /// <param name="a">Խաղը սկսելուց սև արքաի առաջին կոորդինատ</param>
        /// <param name="b">Խաղը սկսելուց սև արքաի երկրորդ կոորդինատ</param>
        //public static void ShowBoard(int a, int b)
        //{
        //    Board();
        //    Manager.king.SetPosition(a, b);
        //    Manager.rookL.SetPosition(1, 8);
        //    Manager.rookR.SetPosition(8, 8);
        //    Manager.queen.SetPosition(4, 8);
        //    Manager.kingW.SetPosition(5, 8);
        //}

        ///// <summary>
        ///// Ցուցադրում է խաղադաշտի տեսքը խաղի ժամանակ, ըստ կատարված քայլերի
        ///// </summary>
        ///// <param name="a">Սև արքայի ստացվող առաջին կոորդինատ</param>
        ///// <param name="b">Սև արքայի ստացվող երկրորդ կոորդինատ</param>
        ///// <param name="versia">Ըստ արքայի կատարած խաղի որոշվող քայլերի հաջորդականություն</param>
        //public static void ShowBoard(int a, int b, int versia)
        //{
        //    Board();
        //    switch (versia)
        //    {
        //        case 1:
        //            Manager.king.SetPosition(a, b);
        //            Manager.rookL.SetPosition(1, 3);
        //            Manager.rookR.SetPosition(8, 8);
        //            Manager.queen.SetPosition(4, 8);
        //            Manager.kingW.SetPosition(5, 8);
        //            break;
        //        case 2:
        //            Manager.king.SetPosition(a, b);
        //            Manager.rookL.SetPosition(1, 3);
        //            Manager.rookR.SetPosition(8, 8);
        //            Manager.queen.SetPosition(4, 3);
        //            Manager.kingW.SetPosition(5, 8);
        //            break;
        //        case 3:
        //            Manager.king.SetPosition(a, b);
        //            Manager.rookL.SetPosition(1, 3);
        //            Manager.rookR.SetPosition(8, 8);
        //            Manager.queen.SetPosition(3, 2);
        //            Manager.kingW.SetPosition(5, 8);
        //            break;
        //        case 4:
        //            Manager.king.SetPosition(a, b);
        //            Manager.rookL.SetPosition(1, 1);
        //            Manager.rookR.SetPosition(8, 8);
        //            Manager.queen.SetPosition(3, 2);
        //            Manager.kingW.SetPosition(5, 8);
        //            break;
        //    }
        //}
    }
}
