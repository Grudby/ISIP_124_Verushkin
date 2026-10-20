using System;
using System.Collections.Generic;
using System.Text;
using Marketplace.Data;
using Marketplace.Models;
using Marketplace.Services;
using Microsoft.EntityFrameworkCore;

namespace Marketplace
{
    internal class Program
    {
        private static User? currentUser;

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("Практическая работа 7: маркетплейс GMWOG||GG.MOW||WONGG");
            Console.WriteLine("Верушкин Василий Андреевич, ИСиП-124");

            try
            {
                DatabaseInitializer.Initialize();
                RunMenu();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Ошибка базы данных: {ex.InnerException?.Message ?? ex.Message}");
            }

            Console.WriteLine("Работа завершена.");
        }

        private static void RunMenu()
        {
            while (true)
            {
                Console.WriteLine();
                bool keepRunning = currentUser == null ? GuestMenu() : UserMenu();
                if (!keepRunning)
                {
                    return;
                }
            }
        }

        private static bool GuestMenu()
        {
            Console.WriteLine("===== Главное меню (гость) =====");
            Console.WriteLine("1 - Каталог товаров");
            Console.WriteLine("2 - Регистрация");
            Console.WriteLine("3 - Вход");
            Console.WriteLine("0 - Выход");
            Console.Write("Выберите пункт: ");
            string choice = InputHelper.ReadLine().Trim();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ShowCatalog();
                    break;
                case "2":
                    Register();
                    break;
                case "3":
                    Login();
                    break;
                case "0":
                    return false;
                default:
                    Console.WriteLine("Нет такого пункта меню.");
                    break;
            }
            return true;
        }

        private static bool UserMenu()
        {
            Console.WriteLine($"===== Главное меню ({currentUser!.Login}) =====");
            Console.WriteLine("1 - Каталог товаров");
            Console.WriteLine("2 - Добавить товар в корзину");
            Console.WriteLine("3 - Корзина");
            Console.WriteLine("4 - Убрать товар из корзины");
            Console.WriteLine("5 - Купить один товар из корзины");
            Console.WriteLine("6 - Оформить всю корзину");
            Console.WriteLine("9 - Выйти из аккаунта");
            Console.WriteLine("0 - Выход");
            Console.Write("Выберите пункт: ");
            string choice = InputHelper.ReadLine().Trim();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ShowCatalog();
                    break;
                case "2":
                    AddToCart();
                    break;
                case "3":
                    ShowCart();
                    break;
                case "4":
                    RemoveFromCart();
                    break;
                case "5":
                    BuyOneItem();
                    break;
                case "6":
                    BuyWholeCart();
                    break;
                case "9":
                    Console.WriteLine($"Вы вышли из аккаунта {currentUser.Login}.");
                    currentUser = null;
                    break;
                case "0":
                    return false;
                default:
                    Console.WriteLine("Нет такого пункта меню.");
                    break;
            }
            return true;
        }

        private static void ShowCatalog()
        {
            List<Product> products = CatalogService.GetProducts();
            Console.WriteLine("=== Каталог товаров ===");
            if (products.Count == 0)
            {
                Console.WriteLine("Каталог пуст.");
                return;
            }

            string category = string.Empty;
            foreach (Product product in products)
            {
                if (product.Category!.Name != category)
                {
                    category = product.Category.Name;
                    Console.WriteLine($"[{category}]");
                }
                string stock = product.StockQuantity > 0 ? $"в наличии: {product.StockQuantity} шт." : "нет в наличии";
                Console.WriteLine($"  {product.Id}. {product.Name} - {product.Price:0.00} руб. ({stock})");
                Console.WriteLine($"     {product.Description}");
            }
        }

        private static void AddToCart()
        {
            ShowCatalog();
            Console.WriteLine();
            int productId = InputHelper.ReadInt("Номер товара: ", 1, int.MaxValue);
            Product? product = CatalogService.FindProduct(productId);
            if (product == null)
            {
                Console.WriteLine($"Товар с номером {productId} не найден.");
                return;
            }
            if (product.StockQuantity == 0)
            {
                Console.WriteLine("Этого товара нет в наличии.");
                return;
            }

            int quantity = InputHelper.ReadInt($"Количество (1-{product.StockQuantity}): ", 1, product.StockQuantity);
            string? error = CartService.AddProduct(currentUser!.Id, productId, quantity);
            Console.WriteLine(error ?? $"В корзину добавлено: {product.Name} x{quantity}");
        }

        private static List<CartItem> ShowCart()
        {
            List<CartItem> items = CartService.GetItems(currentUser!.Id);
            Console.WriteLine("=== Корзина ===");
            if (items.Count == 0)
            {
                Console.WriteLine("Корзина пуста.");
                return items;
            }

            for (int i = 0; i < items.Count; i++)
            {
                CartItem item = items[i];
                Console.WriteLine($"{i + 1}. {item.Product!.Name} - {item.Product.Price:0.00} руб. x {item.Quantity} = " +
                                  $"{item.Product.Price * item.Quantity:0.00} руб.");
            }
            Console.WriteLine($"Итого: {CartService.GetTotal(items):0.00} руб.");
            return items;
        }

        private static CartItem? SelectCartItem(string prompt)
        {
            List<CartItem> items = ShowCart();
            if (items.Count == 0)
            {
                return null;
            }

            int number = InputHelper.ReadInt($"{prompt} (1-{items.Count}): ", 1, items.Count);
            return items[number - 1];
        }

        private static void RemoveFromCart()
        {
            CartItem? item = SelectCartItem("Номер позиции для удаления");
            if (item == null)
            {
                return;
            }

            bool removed = CartService.RemoveItem(currentUser!.Id, item.Id);
            Console.WriteLine(removed ? $"{item.Product!.Name} убран из корзины." : "Позиция не найдена.");
        }

        private static void BuyOneItem()
        {
            CartItem? item = SelectCartItem("Номер позиции для покупки");
            if (item != null)
            {
                Checkout(new List<int> { item.Id });
            }
        }

        private static void BuyWholeCart()
        {
            List<CartItem> items = ShowCart();
            if (items.Count == 0)
            {
                return;
            }

            List<int> ids = new List<int>();
            foreach (CartItem item in items)
            {
                ids.Add(item.Id);
            }
            Checkout(ids);
        }

        private static void Checkout(List<int> cartItemIds)
        {
            List<PickupPoint> points = CatalogService.GetPickupPoints();
            if (points.Count == 0)
            {
                Console.WriteLine("Нет доступных пунктов выдачи.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Пункты выдачи заказов:");
            for (int i = 0; i < points.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {points[i].Address} ({points[i].WorkingHours})");
            }
            int number = InputHelper.ReadInt($"Выберите ПВЗ (1-{points.Count}): ", 1, points.Count);
            PickupPoint point = points[number - 1];

            if (!InputHelper.Confirm($"Оформить заказ с получением по адресу {point.Address}?"))
            {
                Console.WriteLine("Оформление отменено.");
                return;
            }

            Order? order = OrderService.Checkout(currentUser!.Id, cartItemIds, point.Id, out string error);
            if (order == null)
            {
                Console.WriteLine($"Заказ не оформлен: {error}");
                return;
            }

            Console.WriteLine($"Заказ №{order.Id} оформлен на сумму {OrderService.GetTotal(order):0.00} руб.");
            Console.WriteLine($"Забрать: {point.Address}");
        }

        private static void Register()
        {
            Console.WriteLine("=== Регистрация ===");
            string login;
            while (true)
            {
                login = InputHelper.ReadString("Ник: ");
                string? error = AuthService.ValidateLogin(login);
                if (error == null && AuthService.IsLoginTaken(login))
                {
                    error = "Такой ник уже занят. Если это вы - воспользуйтесь входом.";
                }
                if (error == null)
                {
                    break;
                }
                Console.WriteLine(error);
            }

            string password;
            while (true)
            {
                password = InputHelper.ReadPassword("Пароль: ");
                string? error = AuthService.ValidatePassword(password);
                if (error != null)
                {
                    Console.WriteLine(error);
                    continue;
                }

                string confirmation = InputHelper.ReadPassword("Повторите пароль: ");
                if (confirmation == password)
                {
                    break;
                }
                Console.WriteLine("Пароли не совпадают, попробуйте ещё раз.");
            }

            currentUser = AuthService.Register(login, password);
            Console.WriteLine($"Аккаунт {currentUser.Login} создан, вы вошли в систему.");
        }

        private static void Login()
        {
            Console.WriteLine("=== Вход ===");
            string login = InputHelper.ReadString("Ник: ");
            string password = InputHelper.ReadPassword("Пароль: ");

            User? user = AuthService.Login(login, password);
            if (user == null)
            {
                Console.WriteLine("Неверный ник или пароль.");
                return;
            }

            currentUser = user;
            Console.WriteLine($"Добро пожаловать, {user.Login}!");
        }
    }
}
