using System;


namespace HomeWork1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[10];
            Console.WriteLine("Please enter a number\n");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"Number{i+1} - ");
                string number = Console.ReadLine();
                if (int.TryParse(number, out int result))
                {
                    arr[i] = result;
                }
            }
            Console.WriteLine();

            ArrayAction.MaxDigitArray(arr);

            Console.WriteLine("UnSorted array is\n");
            ArrayAction.PrintArray(arr);


            int[] newArray =  ArrayAction.SortedArray(arr);
            Console.WriteLine("Sorted array is\n");
            ArrayAction.PrintArray(newArray);

            Console.ReadLine();
        }
    }
}
