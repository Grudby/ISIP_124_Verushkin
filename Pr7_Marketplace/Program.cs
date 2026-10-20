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
