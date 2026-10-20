using System.Collections.Generic;
using System.Linq;

namespace Pr4_Library
{
    public class CartItem
    {
        public Book Book { get; }
        public int Quantity { get; set; }
        public decimal Sum => Book.Price * Quantity;

        public CartItem(Book book, int quantity)
        {
            Book = book;
            Quantity = quantity;
        }
    }

    /// <summary>
    /// Корзина для покупки книг.
    /// </summary>
    public class Cart
    {
        private readonly List<CartItem> items = new List<CartItem>();

        public IReadOnlyList<CartItem> Items => items;
        public bool IsEmpty => items.Count == 0;
        public decimal Total => items.Sum(item => item.Sum);

        public void Add(Book book, int quantity)
        {
            CartItem? existing = items.FirstOrDefault(item => item.Book.Id == book.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                items.Add(new CartItem(book, quantity));
            }
        }

        public bool Remove(int bookId)
        {
            return items.RemoveAll(item => item.Book.Id == bookId) > 0;
        }

        public void Clear()
        {
            items.Clear();
        }
    }
}
