using System;

namespace Pr4_Library
{
    /// <summary>
    /// Жанры книг, доступные в библиотеке.
    /// </summary>
    public enum Genre
    {
        Novel = 1,
        Detective,
        Fantasy,
        Poetry,
        Science
    }

    public static class GenreNames
    {
        public static string GetName(Genre genre)
        {
            switch (genre)
            {
                case Genre.Novel:
                    return "Роман";
                case Genre.Detective:
                    return "Детектив";
                case Genre.Fantasy:
                    return "Фантастика";
                case Genre.Poetry:
                    return "Поэзия";
                case Genre.Science:
                    return "Научная литература";
                default:
                    return genre.ToString();
            }
        }

        /// <summary>
        /// Определяет жанр по номеру или по названию (без учёта регистра).
        /// </summary>
        public static bool TryParse(string? text, out Genre genre)
        {
            genre = Genre.Novel;
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            text = text.Trim();

            if (int.TryParse(text, out int number))
            {
                if (Enum.IsDefined(typeof(Genre), number))
                {
                    genre = (Genre)number;
                    return true;
                }
                return false;
            }

            foreach (Genre value in Enum.GetValues(typeof(Genre)))
            {
                if (string.Equals(GetName(value), text, StringComparison.OrdinalIgnoreCase))
                {
                    genre = value;
                    return true;
                }
            }

            return false;
        }
    }
}
