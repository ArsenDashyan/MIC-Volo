using System;
using System.Collections.Generic;
using System.Text;

namespace HomeWork2
{
    class Calculator
    {
        public static void StartCalc()
        {
            int num1 = 0;
            int num2 = 0;
            char op;

            Console.WriteLine("Please enter a first number");
            bool isParse = int.TryParse(Console.ReadLine(), out int result);
            if (isParse)
            {
                num1 = result;
            }

            Console.WriteLine("Please select a operation\n" +
                "For sum press +\n" +
                "For subtraction press -\n" +
                "For division press /\n" +
                "For multiplication press *\n" +
                "For Sqrt press s\n");
            op = Convert.ToChar(Console.ReadLine());

            if (op != 's')
            {
                Console.WriteLine("Please enter a second number");
                bool isParse2 = int.TryParse(Console.ReadLine(), out int result2);
                if (isParse2)
                {
                    num2 = result2;
                }
            }

            switch (op)
            {
                case '+':
                    Console.WriteLine(num1 + num2);
                    break;
                case '-':
                    Console.WriteLine(num1 - num2);
                    break;
                case '*':
                    Console.WriteLine(num1 * num2);
                    break;
                case '/':
                    if (num2 != 0)
                    {
                        Console.WriteLine(num1 + num2);
                    }
                    else
                    {
                        Console.WriteLine("Second number is zero");
                    }
                    break;
                case 's':
                    Sqrtnumber(num1);
                    break;
                default:
                    Console.WriteLine("Sorry you write a non correct operation");
                    break;
            }

        }
        private static void Sqrtnumber(double num)
        {
            int result = 0;
            for (double i = 0; i < num; i += 0.1)
            {
                if ((int)(i * i) == (int)num)
                {
                    result = (int)i;
                }
            }
            Console.WriteLine(result);
        }

    }
}
