using System;
using System.Text;

namespace Marketplace
{
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

        public static bool Confirm(string prompt)
        {
            Console.Write($"{prompt} (да/нет): ");
            string answer = ReadLine().Trim().ToLower();
            return answer == "да" || answer == "д" || answer == "yes" || answer == "y";
        }

        /// <summary>
        /// Читает пароль, показывая вместо символов звёздочки.
        /// Если ввод перенаправлен из файла, читает обычную строку.
        /// </summary>
        public static string ReadPassword(string prompt)
        {
            Console.Write(prompt);
            if (Console.IsInputRedirected)
            {
                return ReadLine();
            }

            StringBuilder builder = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return builder.ToString();
                }
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (builder.Length > 0)
                    {
                        builder.Length--;
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    builder.Append(key.KeyChar);
                    Console.Write('*');
                }
            }
        }
    }
}
