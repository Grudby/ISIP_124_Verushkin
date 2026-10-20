using System;
using System.Linq;
using System.Security.Cryptography;
using Marketplace.Data;
using Marketplace.Models;

namespace Marketplace.Services
{
    /// <summary>
    /// Регистрация и вход. Пароль хранится только в виде хеша PBKDF2 с солью.
    /// </summary>
    public static class AuthService
    {
        public const int MinLoginLength = 3;
        public const int MaxLoginLength = 32;
        public const int MinPasswordLength = 6;

        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public static string? ValidateLogin(string login)
        {
            if (login.Length < MinLoginLength || login.Length > MaxLoginLength)
            {
                return $"Ник должен быть от {MinLoginLength} до {MaxLoginLength} символов.";
            }
            if (!login.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '.'))
            {
                return "Ник может содержать только буквы, цифры, '_' и '.'.";
            }
            return null;
        }

        public static string? ValidatePassword(string password)
        {
            if (password.Length < MinPasswordLength)
            {
                return $"Пароль должен быть не короче {MinPasswordLength} символов.";
            }
            if (password.Any(char.IsWhiteSpace))
            {
                return "Пароль не должен содержать пробелов.";
            }
            return null;
        }

        public static bool IsLoginTaken(string login)
        {
            using MarketplaceContext context = new MarketplaceContext();
            string normalized = login.ToLower();
            return context.Users.Any(user => user.Login.ToLower() == normalized);
        }

        public static User Register(string login, string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            User user = new User
            {
                Login = login,
                PasswordSalt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(HashPassword(password, salt)),
                CreatedAt = DateTime.Now
            };

            using MarketplaceContext context = new MarketplaceContext();
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        public static User? Login(string login, string password)
        {
            using MarketplaceContext context = new MarketplaceContext();
            string normalized = login.ToLower();
            User? user = context.Users.FirstOrDefault(item => item.Login.ToLower() == normalized);
            if (user == null)
            {
                return null;
            }

            byte[] salt = Convert.FromBase64String(user.PasswordSalt);
            byte[] expected = Convert.FromBase64String(user.PasswordHash);
            byte[] actual = HashPassword(password, salt);
            return CryptographicOperations.FixedTimeEquals(expected, actual) ? user : null;
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            return Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        }
    }
}
