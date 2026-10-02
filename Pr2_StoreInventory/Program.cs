using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pr2_StoreInventory
{
    class Program
    {
        static List<Product> products = new List<Product>();
        static int nextCode = 1;

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            SeedTestData();

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                if (choice == null)
                    break;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddProduct();
                            break;
                        case "2":
                            DeleteProduct();
                            break;
                        case "6":
                            ShowAllProducts();
                            break;
                        case "0":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}. Программа продолжает работу.");
                }

                if (running)
                {
                    Console.WriteLine();
                    Console.WriteLine("Нажмите Enter для продолжения...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Работа программы завершена.");
        }

        static void ShowMenu()
        {
            if (!Console.IsOutputRedirected)
                Console.Clear();
            Console.WriteLine("===== УЧЁТ ТОВАРОВ В МАГАЗИНЕ =====");
            Console.WriteLine("Верушкин Василий Андреевич, ИСиП-124");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("6. Показать все товары");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт меню: ");
        }

        static void SeedTestData()
        {
            products.Add(new Product { Code = nextCode++, Name = "Хлеб белый", Price = 45.50m, Quantity = 30, Category = Category.Продукты });
            products.Add(new Product { Code = nextCode++, Name = "Наушники беспроводные", Price = 1999.00m, Quantity = 12, Category = Category.Электроника });
            products.Add(new Product { Code = nextCode++, Name = "Футболка мужская", Price = 899.00m, Quantity = 20, Category = Category.Одежда });
            products.Add(new Product { Code = nextCode++, Name = "Стиральный порошок", Price = 350.00m, Quantity = 0, Category = Category.БытоваяХимия });
            products.Add(new Product { Code = nextCode++, Name = "Молоко 1л", Price = 89.90m, Quantity = 15, Category = Category.Продукты });
        }

        static string ReadNonEmptyString(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("Значение не может быть пустым. Повторите ввод.");
            } while (string.IsNullOrWhiteSpace(input));
            return input.Trim();
        }

        static decimal ReadNonNegativeDecimal(string prompt)
        {
            decimal value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (decimal.TryParse(input, out value) && value >= 0)
                    return value;
                Console.WriteLine("Некорректное значение. Введите число, большее или равное нулю.");
            }
        }

        static int ReadNonNegativeInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out value) && value >= 0)
                    return value;
                Console.WriteLine("Некорректное значение. Введите целое число, большее или равное нулю.");
            }
        }

        static Category ReadCategory()
        {
            List<Category> categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
            while (true)
            {
                Console.WriteLine("Выберите категорию:");
                for (int i = 0; i < categories.Count; i++)
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                Console.Write("Номер категории: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int index) && index >= 1 && index <= categories.Count)
                    return categories[index - 1];
                Console.WriteLine("Некорректный номер категории. Повторите ввод.");
            }
        }

        static void ShowAllProducts()
        {
            Console.WriteLine("--- Список всех товаров ---");
            if (products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
                return;
            }
            foreach (Product p in products.OrderBy(p => p.Code))
                Console.WriteLine(p);
        }

        static void AddProduct()
        {
            Console.WriteLine("--- Добавление товара ---");
            string name = ReadNonEmptyString("Название товара: ");
            decimal price = ReadNonNegativeDecimal("Цена товара: ");
            int quantity = ReadNonNegativeInt("Количество товара: ");
            Category category = ReadCategory();

            Product product = new Product
            {
                Code = nextCode++,
                Name = name,
                Price = price,
                Quantity = quantity,
                Category = category
            };
            products.Add(product);
            Console.WriteLine($"Товар добавлен с кодом {product.Code}.");
        }

        static void DeleteProduct()
        {
            Console.WriteLine("--- Удаление товара ---");
            int code = ReadNonNegativeInt("Введите код товара для удаления: ");
            Product product = products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }
            products.Remove(product);
            Console.WriteLine($"Товар \"{product.Name}\" удалён.");
        }
    }
}
