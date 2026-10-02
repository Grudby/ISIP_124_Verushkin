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

        /// <summary>
        /// Самое короткое слово. При равной длине берётся первое встретившееся.
        /// </summary>
        public static string FindShortestWord(List<string> words)
        {
            if (words.Count == 0)
            {
                return string.Empty;
            }

            string shortest = words[0];
            for (int i = 1; i < words.Count; i++)
            {
                if (words[i].Length < shortest.Length)
                {
                    shortest = words[i];
                }
            }
            return shortest;
        }

        /// <summary>
        /// Самое длинное слово. При равной длине берётся первое встретившееся.
        /// </summary>
        public static string FindLongestWord(List<string> words)
        {
            if (words.Count == 0)
            {
                return string.Empty;
            }

            string longest = words[0];
            for (int i = 1; i < words.Count; i++)
            {
                if (words[i].Length > longest.Length)
                {
                    longest = words[i];
                }
            }
            return longest;
        }

        /// <summary>
        /// Количество предложений. Предложение заканчивается на '.', '!', '?' или '…';
        /// группа знаков подряд ("?!", "...") считается одним концом предложения.
        /// Хвост текста без знака в конце тоже считается предложением, если в нём есть буквы или цифры.
        /// </summary>
        public static int CountSentences(string text)
        {
            int count = 0;
            bool hasContent = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (IsSentenceEnd(c))
                {
                    if (hasContent)
                    {
                        count++;
                        hasContent = false;
                    }
                }
                else if (char.IsLetterOrDigit(c))
                {
                    hasContent = true;
                }
            }

            if (hasContent)
            {
                count++;
            }

            return count;
        }

        private static bool IsSentenceEnd(char c)
        {
            return c == '.' || c == '!' || c == '?' || c == '…';
        }
    }
}
