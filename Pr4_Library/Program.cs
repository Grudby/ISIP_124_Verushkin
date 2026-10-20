using System;
using System.Collections.Generic;
using System.Text;

namespace Pr4_Library
{
    internal class Program
    {
        private static readonly Library Library = new Library();

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("Практическая работа 4: учёт книг в библиотеке");
            Console.WriteLine("Верушкин Василий Андреевич, ИСиП-124");

            Library.SeedTestData();

            try
            {
                RunMenu();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Работа завершена.");
        }

        private static void RunMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Меню:");
                Console.WriteLine("1 - Показать все книги");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите пункт: ");

                string choice = InputHelper.ReadLine().Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        PrintBooks(Library.GetAll(), "Все книги");
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Нет такого пункта меню.");
                        break;
                }
            }
        }

        private static void PrintBooks(IReadOnlyCollection<Book> books, string title)
        {
            Console.WriteLine($"=== {title} ===");
            if (books.Count == 0)
            {
                Console.WriteLine("Книги не найдены.");
                return;
            }

            foreach (Book book in books)
            {
                Console.WriteLine(book);
            }
            Console.WriteLine($"Всего: {books.Count}");
        }
    }
}
