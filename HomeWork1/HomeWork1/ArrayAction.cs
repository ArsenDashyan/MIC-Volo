using System;


namespace HomeWork1
{
    static class ArrayAction
    {
        public static void MaxDigitArray(int[] arr)
        {
            int max = arr[0];

            for (int i = 1; i < arr.Length; i++)
            {
                if (max<arr[i])
                {
                    max = arr[i];
                }
            }
            Console.WriteLine($"Array Max element is {max}");
        }

        public static int[] SortedArray(int[] arr)
        {
            int temp;

            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
            return arr;
        }

        public static void PrintArray(int[] arr)
        {
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }
        }
    }
}
