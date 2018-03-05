/* Лабораторна робота №4 – Потоки
1.	Створити масив заповнений 100000 випадкових елементів, цілі числа
2.	Підрахувати суму елементів масиву (3б)
3.	Підрахувати суму елементів масиву в 4 потоки, порівняти швидкість (3б)
4.	Знайти максимальний елемент масиву в 4 потоки (4б)
5.	Створити UI додаток, який буде підраховувати кількість пробілів в текстовому файлі, виконання обрахунків в фоновому потоці, результат виводиться на форму. Прогрес виконання в ProgressBar (4б)
6.	Застосувати ReaderWriterLockSlim, створити клас, в якому досутп до полів на запис обмежений блокуванням (5б)
*/

using System;
using System.Diagnostics;
using System.Threading;

namespace Threads
{
    public class Programm
    {
        public static void Main()
        {
            const int SIZE = 100000;

            uint[] array = new uint[SIZE];
            Random rnd = new Random();

            uint sum = 0;
            for (uint i = 0; i < SIZE; i++)
            {
                array[i] = Convert.ToUInt32(rnd.Next());
                sum += array[i];
            }
            Console.WriteLine("Sum: {0}", sum);

            // Підрахувати суму елементів масиву в 4 потоки, порівняти швидкість
            uint sumPara = 0;
            System.Threading.Tasks.Parallel.For(0, SIZE, i =>
            {
                sumPara += array[i];
            });
            Console.WriteLine("Parallel Summ: {0}", sumPara);

            // Знайти максимальний елемент масиву в 4 потоки

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
