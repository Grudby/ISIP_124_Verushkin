using System;
using System.Collections.Generic;
using System.Globalization;
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
            ReadExpenses(count);
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

        // Считываем траты в формате (Название услуги или товара; Количество денег)
        static void ReadExpenses(int count)
        {
            Console.WriteLine("Введите траты в формате: Название услуги или товара; Количество денег");
            Console.WriteLine("Пример: Влажные салфетки \"Лента\"; 235");

            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    Console.Write($"Трата {i + 1}: ");
                    string line = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine("Строка не может быть пустой. Повторите ввод.");
                        continue;
                    }

                    // Убираем скобки, если пользователь их ввёл
                    line = line.Trim().TrimStart('(').TrimEnd(')');

                    int sepIndex = line.LastIndexOf(';');
                    if (sepIndex == -1)
                    {
                        Console.WriteLine("Неверный формат. Используйте разделитель ';' между названием и суммой.");
                        continue;
                    }

                    string name = line.Substring(0, sepIndex).Trim();
                    string amountStr = line.Substring(sepIndex + 1).Trim();

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Название не может быть пустым.");
                        continue;
                    }

                    if (!double.TryParse(amountStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double amount)
                        && !double.TryParse(amountStr, NumberStyles.Any, CultureInfo.CurrentCulture, out amount))
                    {
                        Console.WriteLine("Сумма указана неверно. Введите число.");
                        continue;
                    }

                    if (amount < 0)
                    {
                        Console.WriteLine("Сумма не может быть отрицательной.");
                        continue;
                    }

                    expenses.Add(new Expense(name, amount));
                    break;
                }
            }

            Console.WriteLine("Все траты успешно внесены.");
        }
    }
}
