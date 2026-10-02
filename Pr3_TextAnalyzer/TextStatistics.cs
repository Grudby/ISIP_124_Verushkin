using System;
using System.Collections.Generic;

namespace Pr3_TextAnalyzer
{
    /// <summary>
    /// Полная статистика по одному тексту.
    /// </summary>
    internal class TextStatistics
    {
        public string Text { get; }
        public DateTime AnalyzedAt { get; }
        public int CharacterCount { get; }
        public int WordCount { get; }
        public string ShortestWord { get; }
        public string LongestWord { get; }
        public int SentenceCount { get; }
        public int VowelCount { get; }
        public int ConsonantCount { get; }
        public List<KeyValuePair<char, int>> LetterFrequency { get; }

        private TextStatistics(string text, int wordCount, string shortestWord, string longestWord,
            int sentenceCount, int vowelCount, int consonantCount, List<KeyValuePair<char, int>> letterFrequency)
        {
            Text = text;
            AnalyzedAt = DateTime.Now;
            CharacterCount = text.Length;
            WordCount = wordCount;
            ShortestWord = shortestWord;
            LongestWord = longestWord;
            SentenceCount = sentenceCount;
            VowelCount = vowelCount;
            ConsonantCount = consonantCount;
            LetterFrequency = letterFrequency;
        }

        /// <summary>
        /// Выполняет полный анализ текста.
        /// </summary>
        public static TextStatistics Analyze(string text)
        {
            List<string> words = TextProcessor.SplitIntoWords(text);
            TextProcessor.CountVowelsAndConsonants(text, out int vowels, out int consonants);

            return new TextStatistics(
                text,
                words.Count,
                TextProcessor.FindShortestWord(words),
                TextProcessor.FindLongestWord(words),
                TextProcessor.CountSentences(text),
                vowels,
                consonants,
                TextProcessor.GetLetterFrequency(text));
        }

        /// <summary>
        /// Начало текста для краткого отображения в списке.
        /// </summary>
        public string GetPreview(int maxLength = 50)
        {
            string flat = Text.Replace('\n', ' ');
            if (flat.Length <= maxLength)
            {
                return flat;
            }
            return flat.Substring(0, maxLength) + "...";
        }

        public void Print()
        {
            Console.WriteLine($"Время анализа:          {AnalyzedAt:dd.MM.yyyy HH:mm:ss}");
            Console.WriteLine($"Текст:                  {GetPreview()}");
            Console.WriteLine($"Количество символов:    {CharacterCount}");
            Console.WriteLine($"Количество слов:        {WordCount}");
            Console.WriteLine($"Самое короткое слово:   {ShortestWord} ({ShortestWord.Length} симв.)");
            Console.WriteLine($"Самое длинное слово:    {LongestWord} ({LongestWord.Length} симв.)");
            Console.WriteLine($"Количество предложений: {SentenceCount}");
            Console.WriteLine($"Гласных букв:           {VowelCount}");
            Console.WriteLine($"Согласных букв:         {ConsonantCount}");
            Console.WriteLine("Частота букв:");

            int column = 0;
            foreach (KeyValuePair<char, int> pair in LetterFrequency)
            {
                Console.Write($"  {pair.Key}: {pair.Value,-4}");
                column++;
                if (column == 6)
                {
                    Console.WriteLine();
                    column = 0;
                }
            }
            if (column != 0)
            {
                Console.WriteLine();
            }
        }
    }
}
