/* Лабораторна робота №4 – Потоки
1.  Створити масив заповнений 100000 випадкових елементів, цілі числа
2.  Підрахувати суму елементів масиву (3б)
3.  Підрахувати суму елементів масиву в 4 потоки, порівняти швидкість (3б)
4.  Знайти максимальний елемент масиву в 4 потоки (4б)
*/

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Threads
{
    public class Programm
    {
        public static void Main()
        {
            const int SIZE = 100000;
            Stopwatch stopwatch = new Stopwatch();
            
            // Створити масив заповнений 100000 випадкових елементів, цілі числа
            uint[] array = new uint[SIZE];
            Random rnd = new Random();

            for (uint i = 0; i < SIZE; i++)
            {
                array[i] = Convert.ToUInt32(rnd.Next());
            }

            // Підрахувати суму елементів масиву (3б)
            uint sum = 0;
            stopwatch.Start();
            for (uint i = 0; i < SIZE; i++)
            {
                sum += array[i];
            }
            stopwatch.Stop();
            Console.WriteLine("Total sum of elements in array: {0}. Time elapsed {1}", sum, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();

            // Підрахувати суму елементів масиву в 4 потоки, порівняти швидкість
            uint totalSum = 0;
            stopwatch.Start();
            Parallel.For(0, 4, (counter) =>
            {
                uint x = 0;
                for (int i = counter * SIZE/4; i < (counter + 1) * SIZE/4; i++)
                    x += array[i];
                totalSum += x;
            });
            stopwatch.Stop();
            Console.WriteLine("Total sum of elements in array: {0}. Time elapsed {1}", totalSum, stopwatch.ElapsedMilliseconds);

            stopwatch.Reset();

            // Знайти максимальний елемент масиву в 4 потоки
            uint maxPara = array[0];
            stopwatch.Start();
            Parallel.For(0, SIZE, (counter) =>
            {
                uint x = 0;
                for (int i = counter * SIZE / 4; i < (counter) * SIZE / 4; i++)
                {
                    if (array[i] > x)
                        x = array[i];
                }
                if (x > maxPara)
                    maxPara = x;
            });
            stopwatch.Stop();
            Console.WriteLine("Max value element in array: {0}. Time elapsed {1}", maxPara, stopwatch.ElapsedMilliseconds);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
