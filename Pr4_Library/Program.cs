using System;
using System.Collections.Generic;
using System.Text;

namespace Pr4_Library
{
    internal class Program
    {
        public const int MinYear = 1450;

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
                Console.WriteLine("2 - Добавить книгу");
                Console.WriteLine("3 - Удалить книгу по ID");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите пункт: ");

                string choice = InputHelper.ReadLine().Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        PrintBooks(Library.GetAll(), "Все книги");
                        break;
                    case "2":
                        AddBook();
                        break;
                    case "3":
                        RemoveBook();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Нет такого пункта меню.");
                        break;
                }
            }
        }

        private static void AddBook()
        {
            Console.WriteLine("=== Добавление книги ===");
            string title = InputHelper.ReadString("Название: ");
            string author = InputHelper.ReadString("Автор: ");
            Genre genre = InputHelper.ReadGenre();
            int year = InputHelper.ReadInt($"Год издания ({MinYear}-{DateTime.Now.Year}): ", MinYear, DateTime.Now.Year);
            decimal price = InputHelper.ReadPrice("Цена: ");

            Book book = Library.Add(title, author, genre, year, price);
            Console.WriteLine("Книга добавлена:");
            Console.WriteLine(book);
        }

        private static void RemoveBook()
        {
            if (Library.Count == 0)
            {
                Console.WriteLine("Библиотека пуста.");
                return;
            }

            int id = InputHelper.ReadInt("Введите ID книги для удаления: ", 1, int.MaxValue);
            Book? book = Library.FindById(id);
            if (book == null)
            {
                Console.WriteLine($"Книга с ID {id} не найдена.");
                return;
            }

            Library.Remove(id);
            Console.WriteLine($"Удалена книга: {book}");
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
