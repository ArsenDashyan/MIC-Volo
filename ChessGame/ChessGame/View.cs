using System;


namespace ChessGame
{
    class View
    {
        /// <summary>
        /// Xaxi skzbnakan dast
        /// </summary>
        /// <param name="a">sev arqai i koordinatn e </param>
        /// <param name="b">sev arqai j koordinatn e </param>
        public static void ShowBoard(int a, int b)
        {
            Console.Clear();
            var arr = GetArray();
            arr[a - 1, b - 1] = "(K)";
            arr[Manager.rookL.FCoord, Manager.rookL.SCoord] = "(R)";
            arr[Manager.rookR.FCoord, Manager.rookR.SCoord] = "(R)";
            arr[Manager.queen.FCoord, Manager.queen.SCoord] = "(Q)";
            arr[7, 4] = "(K)";
            Print(arr);
        }

        /// <summary>
        /// Sev arqai katarac xaxin hamapatasxan trvox dasht
        /// </summary>
        /// <param name="a"> sev arqai i koordinat</param>
        /// <param name="b">sev arqai j koordinat</param>
        /// <param name="x">tarberak te vor qayli dimac inch petq e ereva </param>
        public static void ShowBoard(int a, int b, int x)
        {
            Console.Clear();
            var arr = GetArray();
            switch (x)
            {
                case 1:
                    Manager.rookR.FCoord = 2;
                    Manager.rookR.SCoord = 7;
                    arr[a - 1, b - 1] = "(K)";
                    arr[Manager.rookL.FCoord, Manager.rookL.SCoord] = "(R)";
                    arr[Manager.rookR.FCoord, Manager.rookR.SCoord] = "(R)";
                    arr[Manager.queen.FCoord, Manager.queen.SCoord] = "(Q)";
                    arr[7, 4] = "(K)";
                    break;
                case 2:
                    Manager.rookL.FCoord = 1;
                    Manager.rookL.SCoord = 0;
                    arr[a - 1, b - 1] = "(K)";
                    arr[Manager.rookL.FCoord, Manager.rookL.SCoord] = "(R)";
                    arr[Manager.rookR.FCoord, Manager.rookR.SCoord] = "(R)";
                    arr[Manager.queen.FCoord, Manager.queen.SCoord] = "(Q)";
                    arr[7, 4] = "(K)";
                    break;
                case 3:
                    Manager.queen.FCoord = 2;
                    Manager.queen.SCoord = 3;
                    arr[a - 1, b - 1] = "(K)";
                    arr[Manager.rookL.FCoord, Manager.rookL.SCoord] = "(R)";
                    arr[Manager.rookR.FCoord, Manager.rookR.SCoord] = "(R)";
                    arr[Manager.queen.FCoord, Manager.queen.SCoord] = "(Q)";
                    arr[7, 4] = "(K)";
                    break;
                case 4:
                    Manager.queen.FCoord = 0;
                    Manager.queen.SCoord = 1;
                    arr[a - 1, b - 1] = "(K)";
                    arr[Manager.rookL.FCoord, Manager.rookL.SCoord] = "(R)";
                    arr[Manager.rookR.FCoord, Manager.rookR.SCoord] = "(R)";
                    arr[Manager.queen.FCoord, Manager.queen.SCoord] = "(Q)";
                    arr[7, 4] = "(K)";
                    break;
            }

            Print(arr);
        }

        /// <summary>
        /// Ogtagorcvum e vorpessi tpenq mer erkchap zangvacy
        /// </summary>
        /// <param name="arr"> stacvox erkchap zangvaci anuny</param>
        private static void Print(string[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write(arr[i, j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Stexcum e erkchap zangvac, 8*8 chapi
        /// </summary>
        /// <returns>veradardznum e erkchap zangvac 8*8 chapi</returns>
        private static string[,] GetArray()
        {
            string[,] arr = new string[8, 8];

            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        arr[i, j] = "(*)";
                    }
                    else
                    {
                        arr[i, j] = "(%)";
                    }
                }
            }
            return arr;
        }
    }
}
