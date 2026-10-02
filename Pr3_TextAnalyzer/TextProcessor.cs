using System;
using System.Collections.Generic;
using System.Text;

namespace Pr3_TextAnalyzer
{
    /// <summary>
    /// Методы анализа текста. LINQ не используется - только циклы и коллекции.
    /// </summary>
    internal static class TextProcessor
    {
        /// <summary>
        /// Разбивает текст на слова. Слово - последовательность букв и цифр;
        /// дефис и апостроф внутри слова допускаются ("кто-то", "don't").
        /// </summary>
        public static List<string> SplitIntoWords(string text)
        {
            List<string> words = new List<string>();
            StringBuilder current = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (char.IsLetterOrDigit(c))
                {
                    current.Append(c);
                }
                else if ((c == '-' || c == '\'') && current.Length > 0
                         && i + 1 < text.Length && char.IsLetterOrDigit(text[i + 1]))
                {
                    current.Append(c);
                }
                else if (current.Length > 0)
                {
                    words.Add(current.ToString());
                    current.Clear();
                }
            }

            if (current.Length > 0)
            {
                words.Add(current.ToString());
            }

            return words;
        }

        /// <summary>
        /// Количество слов в тексте.
        /// </summary>
        public static int CountWords(string text)
        {
            return SplitIntoWords(text).Count;
        }
    }
}
