namespace Pr4_Library
{
    /// <summary>
    /// Книга в библиотеке.
    /// </summary>
    public class Book
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public Genre Genre { get; }
        public int Year { get; }
        public decimal Price { get; }

        public Book(int id, string title, string author, Genre genre, int year, decimal price)
        {
            Id = id;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }

        public override string ToString()
        {
            return $"ID: {Id} | \"{Title}\" | Автор: {Author} | Жанр: {GenreNames.GetName(Genre)} | " +
                   $"Год: {Year} | Цена: {Price:0.00} руб.";
        }
    }
}
