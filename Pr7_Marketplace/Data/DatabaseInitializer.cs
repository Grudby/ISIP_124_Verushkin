using System.Linq;
using Marketplace.Models;

namespace Marketplace.Data
{
    /// <summary>
    /// Создаёт базу данных при первом запуске и заполняет каталог, категории и пункты выдачи.
    /// </summary>
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using MarketplaceContext context = new MarketplaceContext();
            context.Database.EnsureCreated();

            if (context.Categories.Any())
            {
                return;
            }

            Category electronics = new Category { Name = "Электроника" };
            Category games = new Category { Name = "Игры" };
            Category clothes = new Category { Name = "Одежда" };
            Category books = new Category { Name = "Книги" };
            context.Categories.AddRange(electronics, games, clothes, books);

            context.Products.AddRange(
                new Product { Name = "Беспроводные наушники", Description = "Bluetooth 5.3, шумоподавление", Price = 4990m, StockQuantity = 15, Category = electronics },
                new Product { Name = "Механическая клавиатура", Description = "Переключатели Red, подсветка", Price = 6490m, StockQuantity = 8, Category = electronics },
                new Product { Name = "Игровая мышь", Description = "16000 DPI, 6 кнопок", Price = 2590m, StockQuantity = 20, Category = electronics },
                new Product { Name = "Ключ Steam на игру", Description = "Цифровой ключ активации", Price = 1299m, StockQuantity = 50, Category = games },
                new Product { Name = "Настольная игра «Каркассон»", Description = "Для 2-5 игроков", Price = 2190m, StockQuantity = 5, Category = games },
                new Product { Name = "Худи оверсайз", Description = "Хлопок, размер L", Price = 3290m, StockQuantity = 12, Category = clothes },
                new Product { Name = "Кепка", Description = "Регулируемый размер", Price = 990m, StockQuantity = 25, Category = clothes },
                new Product { Name = "C# для профессионалов", Description = "Учебник по .NET 8", Price = 2890m, StockQuantity = 7, Category = books });

            context.PickupPoints.AddRange(
                new PickupPoint { Address = "ул. Ленина, 10", WorkingHours = "09:00-21:00" },
                new PickupPoint { Address = "пр. Мира, 45, ТЦ «Галерея»", WorkingHours = "10:00-22:00" },
                new PickupPoint { Address = "ул. Садовая, 3", WorkingHours = "08:00-20:00" });

            context.SaveChanges();
        }
    }
}
