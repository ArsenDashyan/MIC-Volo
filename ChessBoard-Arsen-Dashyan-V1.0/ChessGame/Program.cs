using System;
using ChessGame;

Manager.Play();

Console.ReadLine();

#region Draft

/// <summary>
/// Play the Game
/// </summary>
//public static void Play()
//{
//    View.Board();
//    PlacementManager();
//    do
//    {
//        if (kingBlack.AvailableMoves().Count != 0)
//        {
//            KingPosition();
//            if (kingBlack.point.X == 1 || kingBlack.point.X == 8)
//            {
//                break;
//            }
//        }
//        else
//        {
//            Console.SetCursorPosition(40, 8);
//            Console.WriteLine("Game over");
//            break;
//        }

//    } while (kingBlack.point.X != 1 || kingBlack.point.X != 8);

//    if (kingBlack.AvailableMoves().Count != 0)
//    {
//        var point = InputCoordinats("Black", "King");
//        if (kingBlack.IsMove(point))
//            EndGame(models, point);
//    }
//    else
//    {
//        Console.SetCursorPosition(40, 8);
//        Console.WriteLine("Game over");
//        return;
//    }
//}

/// <summary>
/// Chack the Black king position in bord
/// </summary>
//public static void KingPosition()
//{
//    View.ClearText();
//    var tupl = InputCoordinats("Black", "King");
//    if (kingBlack.IsMove(tupl))
//    {
//        if (tupl.X <= 4)
//            Half(tupl, 1);
//        else
//            Half(tupl, 2);
//    }
//}

/// <summary>
/// Chack the versia for king positions and random moving white figure
/// </summary>
/// <param name="a">Black king first coordinat</param>
/// <param name="b">Black king second coordinat</param>
/// <param name="vers">King position in bord half</param>
//private static void Half(Point point, int vers)
//{
//    kingBlack.SetPosition(point);
//    var modelNew = models.Where(c => !(c is King)).ToList();
//    if (!MoveNextTo(modelNew, point, vers))
//    {
//        Point point1 = queen.AvailableMoves().RandomMove();
//        queen.SetPosition(point1);
//    }
//}

/// <summary>
/// For end game chack white figure coordinat avialable move 
/// </summary>
/// <param name="a">black king first coordinat</param>
/// <param name="b">black king second coordinat</param>
/// <param name="qu">Rook instanse</param>
/// <param name="ro">Rook instanse</param>
//private static void EndGame(List<Model> models, Point point)
//{
//    kingBlack.SetPosition(point);
//    var modelNew = models.Where(c => !(c is King)).ToList();
//    ForEndMove(modelNew, point);
//    Console.SetCursorPosition(40, 8);
//    Console.WriteLine("Game over");
//}
//public static void ForEndMove(List<Model> modelNew, Point point)
//{
//    foreach (var item in modelNew)
//    {
//        if (item is Queen queen)
//        {
//            if (queen.point.X == 2 || queen.point.X == 7)
//            {
//                modelNew.Remove(queen);
//                ForEndMovePosition(modelNew, point);
//                break;
//            }
//        }
//        if (ReferenceEquals(item, rookL))
//        {
//            if (rookL.point.X == 2 || rookL.point.X == 7)
//            {
//                modelNew.Remove(rookL);
//                ForEndMovePosition(modelNew, point);
//                break;
//            }
//        }
//        if (ReferenceEquals(item, rookR))
//        {
//            if (rookR.point.X == 2 || rookR.point.X == 7)
//            {
//                modelNew.Remove(rookR);
//                ForEndMovePosition(modelNew, point);
//                break;
//            }
//        }
//    }
//}
//public static void ForEndMovePosition(List<Model> modelNew, Point point)
//{
//    foreach (var item in modelNew)
//    {
//        if (item is Queen queen)
//        {
//            Point tuplQ = queen.AvailableMoves().EndPosition(point);
//            if (tuplQ != null)
//            {
//                queen.SetPosition(tuplQ);
//                break;
//            }
//        }
//        if (ReferenceEquals(item, rookL))
//        {
//            Point tuplL = rookL.AvailableMoves().EndPosition(point);
//            if (tuplL != null)
//            {
//                rookL.SetPosition(tuplL);
//                break;
//            }
//        }
//        if (ReferenceEquals(item, rookR))
//        {
//            Point tuplR = rookR.AvailableMoves().EndPosition(point);
//            if (tuplR != null)
//            {
//                rookL.SetPosition(tuplR);
//                break;
//            }
//        }
//    }
//}
#endregion

