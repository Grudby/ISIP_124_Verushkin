using System;
using System.Text;

namespace Pr3_TextAnalyzer
{
    /// <summary>
    /// Ввод текста от пользователя с проверкой минимальной длины.
    /// </summary>
    internal static class TextInput
    {
        public const int MinLength = 100;

        /// <summary>
        /// Считывает многострочный текст. Ввод завершается пустой строкой.
        /// Повторяет запрос, пока текст короче MinLength символов.
        /// </summary>
        public static string ReadText()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"Введите текст (минимум {MinLength} символов).");
                Console.WriteLine("Для завершения ввода оставьте пустую строку:");

                StringBuilder builder = new StringBuilder();
                string? line = Console.ReadLine();
                while (line != null && line.Length > 0)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append('\n');
                    }
                    builder.Append(line);
                    line = Console.ReadLine();
                }

                string text = builder.ToString().Trim();

                if (text.Length >= MinLength)
                {
                    return text;
                }

                if (line == null && text.Length == 0)
                {
                    // Поток ввода закрыт, текста нет - дальше ждать бессмысленно
                    throw new InvalidOperationException("Ввод был прерван.");
                }

                Console.WriteLine($"Текст слишком короткий: {text.Length} симв. Нужно не меньше {MinLength}.");
            }
        }
    }
}
