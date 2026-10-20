using System.Collections.Generic;
using System.Linq;

namespace Pr4_Library
{
    /// <summary>
    /// Хранилище книг. Идентификатор новой книги назначается автоматически.
    /// </summary>
    public class Library
    {
        private readonly List<Book> books = new List<Book>();
        private int nextId = 1;

        public int Count => books.Count;

        public IReadOnlyList<Book> GetAll()
        {
            return books.OrderBy(book => book.Id).ToList();
        }

        public Book Add(string title, string author, Genre genre, int year, decimal price)
        {
            Book book = new Book(nextId, title, author, genre, year, price);
            nextId++;
            books.Add(book);
            return book;
        }

        public void SeedTestData()
        {
            Add("Преступление и наказание", "Фёдор Достоевский", Genre.Novel, 1866, 450m);
            Add("Идиот", "Фёдор Достоевский", Genre.Novel, 1869, 520m);
            Add("Убийство в Восточном экспрессе", "Агата Кристи", Genre.Detective, 1934, 390m);
            Add("Пикник на обочине", "Аркадий и Борис Стругацкие", Genre.Fantasy, 1972, 310m);
            Add("Краткая история времени", "Стивен Хокинг", Genre.Science, 1988, 640m);
        }
    }
}
