using System;
using System.Collections.Generic;
using System.Text;

namespace Pr3_TextAnalyzer
{
    internal class Program
    {
        // Статистика по всем проанализированным текстам
        private static readonly List<TextStatistics> History = new List<TextStatistics>();

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("Практическая работа: анализ текста");
            Console.WriteLine("Верушкин Василий Андреевич, ИСиП-124");

            AnalyzeNewText();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Меню:");
                Console.WriteLine("1 - Ввести новый текст");
                Console.WriteLine("2 - Показать статистику по прошлым текстам");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите пункт: ");

                string? choice = Console.ReadLine();
                if (choice == null)
                {
                    return;
                }

                switch (choice.Trim())
                {
                    case "1":
                        AnalyzeNewText();
                        break;
                    case "2":
                        ShowHistory();
                        break;
                    case "0":
                        Console.WriteLine("Работа завершена.");
                        return;
                    default:
                        Console.WriteLine("Нет такого пункта меню.");
                        break;
                }
            }
        }

        private static void AnalyzeNewText()
        {
            string text;
            try
            {
                text = TextInput.ReadText();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
                return;
            }

            TextStatistics statistics = TextStatistics.Analyze(text);
            History.Add(statistics);

            Console.WriteLine();
            Console.WriteLine($"=== Статистика текста №{History.Count} ===");
            statistics.Print();
        }

        private static void ShowHistory()
        {
            if (History.Count == 0)
            {
                Console.WriteLine("Список статистики пуст.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Сохранённые тексты:");
            for (int i = 0; i < History.Count; i++)
            {
                Console.WriteLine($"{i + 1}. [{History[i].AnalyzedAt:HH:mm:ss}] {History[i].GetPreview()}");
            }

            Console.Write("Введите номер текста для подробной статистики или 'все' для вывода всех: ");
            string? input = Console.ReadLine();
            if (input == null)
            {
                return;
            }
            input = input.Trim().ToLower();

            if (input == "все" || input == "all")
            {
                for (int i = 0; i < History.Count; i++)
                {
                    Console.WriteLine();
                    Console.WriteLine($"=== Статистика текста №{i + 1} ===");
                    History[i].Print();
                }
                return;
            }

            if (int.TryParse(input, out int number) && number >= 1 && number <= History.Count)
            {
                Console.WriteLine();
                Console.WriteLine($"=== Статистика текста №{number} ===");
                History[number - 1].Print();
            }
            else
            {
                Console.WriteLine("Неверный номер.");
            }
        }
    }
}
