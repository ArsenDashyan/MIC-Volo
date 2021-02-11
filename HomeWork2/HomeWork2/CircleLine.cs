using System;
using System.Collections.Generic;
using System.Text;

namespace HomeWork2
{
    class CircleLine
    {
        /// <summary>
        /// Ditarkel em shrjanagci ev uxxi hatman 4 depq
        /// </summary>
        private enum version { arajin, erkrord, errord, chorord };
        public static void IsInside(double x, double y)
        {
            Console.OutputEncoding = Encoding.UTF8;
            double xLine;
            double yLine;
            double rCircle;

            Console.WriteLine("Խնդրում եմ մուտքագրեք ուղղի և աբսցիսների առանցքի հատման x կետը");
            xLine = int.Parse(Console.ReadLine());

            Console.WriteLine("Խնդրում եմ մուտքագրեք ուղղի և օրդինատների առանցքի հատման y կետը");
            yLine = double.Parse(Console.ReadLine());

            Console.WriteLine("Խնդրում եմ մուտքագրեք շրջանի շառավղի r երկարությունը");
            rCircle = double.Parse(Console.ReadLine());

            var vers = ReturnVersion(rCircle, xLine, yLine);

            //switch (vers)
            //{
            //    case version.arajin:
            //        Inside(x,y,rCircle);
            //        break;
            //    case version.erkrord:
            //        Inside(x, y, xLine, yLine);
            //        break;
            //    case version.errord:
            //        InsideErr(x,y,rCircle,yLine);
            //        break;
            //    case version.chorord:
            //        InsideCho(x, y, xLine, rCircle);
            //        break;
            //}

            string str = vers switch
            {
                version.arajin => Inside(x, y, rCircle),
                version.erkrord => Inside(x, y, xLine, yLine),
                version.errord => InsideErr(x, y, rCircle, yLine),
                version.chorord => InsideCho(x, y, xLine, rCircle),
                _ => "Mutqagrvac tvyalneri mej sxal ka"
            };
            Console.WriteLine(str);
        }

        private static Enum ReturnVersion(double r, double x, double y)
        {
            int result = 0;
            if (r < Math.Abs(x) && r < Math.Abs(y))
            {
                result = (int)version.arajin;
            }
            else if (r >= Math.Abs(x) && r >= Math.Abs(y))
            {
                result = (int)version.erkrord;
            }
            else if (r >= Math.Abs(y) && r < Math.Abs(x))
            {
                result = (int)version.errord;
            }
            else if (r < Math.Abs(y) && r > Math.Abs(x))
            {
                result = (int)version.chorord;
            }
            return (version)result;
        }

        private static string Inside(double i, double j, double xI, double yI)
        {
            double yRes = 0.0;
            if (xI <= i && i <= 0 && 0 <= j && j <= yI)
            {
                yRes = yI - yI / xI * i;
                if (yRes >= j)
                {
                    return "Թիվը պատկանում է տիրույթին";
                }
                else
                {
                    return "Թիվը չի պատկանում է տիրույթին";
                }
            }
            else
            {
                return "Թիվը չի պատկանում է տիրույթին";
            }
        }
        private static string Inside(double i, double j, double r)
        {
            double yRes = 0.0;
            if (r <= i && i <= 0 && 0 <= j && j <= r)
            {
                yRes = Math.Sqrt(r * r - i * i);
                if (yRes >= j)
                {
                    return "Թիվը պատկանում է տիրույթին";
                }
                else
                {
                    return "Թիվը չի պատկանում է տիրույթին";
                }
            }
            else
            {
                return "Թիվը չի պատկանում է տիրույթին";
            }
        }

        private static string InsideErr(double i, double j, double r, double yI)
        {
            double yResC = 0.0;
            double yResL = 0.0;
            if (r <= i && i <= 0 && 0 <= j && j <= yI)
            {
                yResC = Math.Sqrt(r * r - i * i);
                yResL = yI - yI / r * i;
                if (yResL < yResC)
                {
                    if (yResL >= j)
                    {
                        return "Թիվը պատկանում է տիրույթին";
                    }
                    else
                    {
                        return "Թիվը չի պատկանում է տիրույթին";
                    }
                }
                else
                {
                    if (yResC >= j)
                    {
                        return "Թիվը պատկանում է տիրույթին";
                    }
                    else
                    {
                        return "Թիվը չի պատկանում է տիրույթին";
                    }
                }

            }
            else
            {
                return "Թիվը չի պատկանում է տիրույթին";
            }
        }
        private static string InsideCho(double i, double j, double xI, double r)
        {
            double yResC = 0.0;
            double yResL = 0.0;
            if (xI <= i && i <= 0 && 0 <= j && j <= r)
            {
                yResC = Math.Sqrt(r * r - i * i);
                yResL = r - r / xI * i;
                if (yResL < yResC)
                {
                    if (yResL >= j)
                    {
                        return "Թիվը պատկանում է տիրույթին";
                    }
                    else
                    {
                        return "Թիվը չի պատկանում է տիրույթին";
                    }
                }
                else
                {
                    if (yResC >= j)
                    {
                        return "Թիվը պատկանում է տիրույթին";
                    }
                    else
                    {
                        return "Թիվը չի պատկանում է տիրույթին";
                    }
                }

            }
            else
            {
                return "Թիվը չի պատկանում է տիրույթին";
            }
        }
    }
}
