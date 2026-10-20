using System;

namespace Pr6_Roguelike
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

        /// <summary>
        /// Запрашивает выбор, пока пользователь не введёт один из допустимых вариантов.
        /// </summary>
        public static string ReadChoice(string prompt, params string[] options)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = ReadLine().Trim();
                if (Array.IndexOf(options, value) >= 0)
                {
                    return value;
                }
                Console.WriteLine($"Введите один из вариантов: {string.Join(", ", options)}.");
            }
        }
    }
}
