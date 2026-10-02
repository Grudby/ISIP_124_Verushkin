using System;
using System.Collections.Generic;
using System.Text;

namespace Pr1_Expenses
{
    class Program
    {
        // Список всех введённых трат
        static List<Expense> expenses = new List<Expense>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("Учёт потраченных за день средств");
            Console.WriteLine("Верушкин Василий Андреевич, ИСиП-124");
            Console.WriteLine("=================================");

            int count = ReadOperationsCount();
            Console.WriteLine($"Будет введено операций: {count}");
        }

        // Считываем количество операций от 2 до 40
        static int ReadOperationsCount()
        {
            int count;
            while (true)
            {
                Console.Write("Введите количество операций (от 2 до 40): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out count) && count >= 2 && count <= 40)
                {
                    return count;
                }
                Console.WriteLine("Некорректное значение. Введите целое число от 2 до 40.");
            }
        }
    }
}
