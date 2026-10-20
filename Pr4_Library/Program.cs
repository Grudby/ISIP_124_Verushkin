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
                Console.WriteLine("4 - Найти книги по названию");
                Console.WriteLine("5 - Найти книги по автору");
                Console.WriteLine("6 - Найти книги по жанру");
                Console.WriteLine("7 - Отсортировать по названию");
                Console.WriteLine("8 - Отсортировать по году издания");
                Console.WriteLine("9 - Самая дорогая и самая дешёвая книга");
                Console.WriteLine("10 - Количество книг по авторам");
                Console.WriteLine("11 - Вставить блок книг");
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
                    case "4":
                        string title = InputHelper.ReadString("Введите название или его часть: ");
                        PrintBooks(Library.FindByTitle(title), $"Поиск по названию \"{title}\"");
                        break;
                    case "5":
                        string author = InputHelper.ReadString("Введите автора или часть имени: ");
                        PrintBooks(Library.FindByAuthor(author), $"Поиск по автору \"{author}\"");
                        break;
                    case "6":
                        Genre genre = InputHelper.ReadGenre();
                        PrintBooks(Library.FindByGenre(genre), $"Книги жанра \"{GenreNames.GetName(genre)}\"");
                        break;
                    case "7":
                        PrintBooks(Library.SortByTitle(), "Книги по названию (А-Я)");
                        break;
                    case "8":
                        PrintBooks(Library.SortByYear(), "Книги по году издания");
                        break;
                    case "9":
                        ShowPriceExtremes();
                        break;
                    case "10":
                        ShowAuthors();
                        break;
                    case "11":
                        ImportBooks();
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

        private static void ShowPriceExtremes()
        {
            Book? mostExpensive = Library.GetMostExpensive();
            Book? cheapest = Library.GetCheapest();
            if (mostExpensive == null || cheapest == null)
            {
                Console.WriteLine("Библиотека пуста.");
                return;
            }

            Console.WriteLine("Самая дорогая книга:");
            Console.WriteLine(mostExpensive);
            Console.WriteLine("Самая дешёвая книга:");
            Console.WriteLine(cheapest);
        }

        private static void ShowAuthors()
        {
            var authors = Library.GroupByAuthor();
            if (authors.Count == 0)
            {
                Console.WriteLine("Библиотека пуста.");
                return;
            }

            Console.WriteLine("=== Книги по авторам ===");
            foreach (var item in authors)
            {
                Console.WriteLine($"{item.Author}: {item.Count} шт.");
            }
        }

        private static void ImportBooks()
        {
            Console.WriteLine("=== Пакетный импорт ===");
            Console.WriteLine("Вставьте строки в формате: Название;Автор;Жанр;Год;Цена");
            Console.WriteLine("Жанр можно указать номером или названием. Для завершения оставьте пустую строку:");

            int added = 0;
            int lineNumber = 0;
            while (true)
            {
                string line = InputHelper.ReadLine().Trim();
                if (line.Length == 0)
                {
                    break;
                }
                lineNumber++;

                if (TryParseBookLine(line, out string error))
                {
                    added++;
                }
                else
                {
                    Console.WriteLine($"Строка {lineNumber} пропущена: {error}");
                }
            }

            Console.WriteLine($"Добавлено книг: {added} из {lineNumber}.");
        }

        private static bool TryParseBookLine(string line, out string error)
        {
            string[] parts = line.Split(';');
            if (parts.Length != 5)
            {
                error = "нужно 5 значений через ';'.";
                return false;
            }

            string title = parts[0].Trim();
            string author = parts[1].Trim();
            if (title.Length == 0 || author.Length == 0)
            {
                error = "название и автор не могут быть пустыми.";
                return false;
            }

            if (!GenreNames.TryParse(parts[2], out Genre genre))
            {
                error = $"неизвестный жанр \"{parts[2].Trim()}\".";
                return false;
            }

            if (!int.TryParse(parts[3].Trim(), out int year) || year < MinYear || year > DateTime.Now.Year)
            {
                error = $"год должен быть от {MinYear} до {DateTime.Now.Year}.";
                return false;
            }

            if (!InputHelper.TryParsePrice(parts[4], out decimal price))
            {
                error = "цена должна быть положительным числом.";
                return false;
            }

            Book book = Library.Add(title, author, genre, year, price);
            Console.WriteLine($"Добавлена: {book}");
            error = string.Empty;
            return true;
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
