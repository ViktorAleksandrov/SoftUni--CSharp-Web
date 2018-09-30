using System;
using System.Linq;
using System.Threading;

namespace P1.EvenNumbersThread
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] stringNumbers = Console.ReadLine().Split();

            int start = int.Parse(stringNumbers[0]);
            int end = int.Parse(stringNumbers[1]);

            var evens = new Thread(() => PrintEvenNumbers(start, end));

            evens.Start();
            evens.Join();

            Console.WriteLine("Thread finished work");
        }

        private static void PrintEvenNumbers(int start, int end)
        {
            var evenNumbers = Enumerable.Range(start, end)
                .Where(n => n % 2 == 0);

            Console.WriteLine(string.Join(Environment.NewLine, evenNumbers));
        }
    }
}
