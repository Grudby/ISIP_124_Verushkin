using System;
using System.Collections.Generic;
using System.Text;

namespace Pr3_TextAnalyzer
{
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("Практическая работа: анализ текста");
            Console.WriteLine("Верушкин Василий Андреевич, ИСиП-124");

            string text = TextInput.ReadText();
            Console.WriteLine($"Принят текст длиной {text.Length} символов.");
            List<string> words = TextProcessor.SplitIntoWords(text);
            Console.WriteLine($"Количество слов: {words.Count}");
            Console.WriteLine($"Самое короткое слово: {TextProcessor.FindShortestWord(words)}");
            Console.WriteLine($"Самое длинное слово: {TextProcessor.FindLongestWord(words)}");
        }
    }
}
