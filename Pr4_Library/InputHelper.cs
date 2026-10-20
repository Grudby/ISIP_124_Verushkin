using System;
using System.Globalization;

namespace Pr4_Library
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

        public static decimal ReadPrice(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (TryParsePrice(ReadLine(), out decimal value))
                {
                    return value;
                }
                Console.WriteLine("Цена должна быть положительным числом.");
            }
        }

        public static Genre ReadGenre()
        {
            while (true)
            {
                Console.WriteLine("Жанры:");
                foreach (Genre genre in Enum.GetValues(typeof(Genre)))
                {
                    Console.WriteLine($"  {(int)genre} - {GenreNames.GetName(genre)}");
                }
                Console.Write("Выберите жанр (номер или название): ");
                if (GenreNames.TryParse(ReadLine(), out Genre result))
                {
                    return result;
                }
                Console.WriteLine("Нет такого жанра.");
            }
        }

        /// <summary>
        /// Разбирает цену, принимая и точку, и запятую как разделитель дробной части.
        /// </summary>
        public static bool TryParsePrice(string? text, out decimal price)
        {
            price = 0;
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            string normalized = text.Trim().Replace(',', '.');
            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out price)
                   && price > 0;
        }
    }
}
