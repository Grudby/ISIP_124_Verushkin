using System;

namespace Pr5_University
{
    /// <summary>
    /// Ввод значений с консоли с проверкой. Повторяет запрос, пока значение не станет корректным.
    /// </summary>
    internal static class InputHelper
    {
        public static string ReadLine()
        {
            string? line = Console.ReadLine();
            if (line == null)
            {
                throw new InvalidOperationException("Ввод был прерван.");
            }
            return line;
        }

        public static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = ReadLine().Trim();
                if (value.Length > 0)
                {
                    return value;
                }
                Console.WriteLine("Значение не может быть пустым.");
            }
        }

        public static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(ReadLine().Trim(), out int value) && value >= min && value <= max)
                {
                    return value;
                }
                Console.WriteLine($"Введите целое число от {min} до {max}.");
            }
        }

        public static string ReadEmail(string prompt)
        {
            while (true)
            {
                string value = ReadString(prompt);
                if (Person.IsValidEmail(value))
                {
                    return value;
                }
                Console.WriteLine("Email должен быть в формате name@example.ru.");
            }
        }

        public static string ReadPhone(string prompt)
        {
            while (true)
            {
                string value = ReadString(prompt);
                int digits = 0;
                bool allowed = true;
                foreach (char c in value)
                {
                    if (char.IsDigit(c))
                    {
                        digits++;
                    }
                    else if (c != '+' && c != '-' && c != ' ' && c != '(' && c != ')')
                    {
                        allowed = false;
                    }
                }

                if (allowed && digits >= 10 && digits <= 12)
                {
                    return value;
                }
                Console.WriteLine("Телефон должен содержать от 10 до 12 цифр, например +7 900 123-45-67.");
            }
        }
    }
}
