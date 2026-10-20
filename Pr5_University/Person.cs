using System;

namespace Pr5_University
{
    /// <summary>
    /// Общие данные любого участника учебного процесса.
    /// </summary>
    public abstract class Person
    {
        private string fullName;
        private int age;
        private string email;
        private string phone;

        public int Id { get; }

        public string FullName
        {
            get => fullName;
            set => fullName = RequireText(value, "ФИО");
        }

        public int Age
        {
            get => age;
            set
            {
                if (value < MinAge || value > 100)
                {
                    throw new ArgumentException($"Возраст должен быть от {MinAge} до 100.");
                }
                age = value;
            }
        }

        public string Email
        {
            get => email;
            set
            {
                string text = RequireText(value, "Email");
                if (!IsValidEmail(text))
                {
                    throw new ArgumentException("Некорректный email.");
                }
                email = text;
            }
        }

        public string Phone
        {
            get => phone;
            set => phone = RequireText(value, "Телефон");
        }

        public abstract string Role { get; }

        protected abstract int MinAge { get; }

        protected Person(int id, string fullName, int age, string email, string phone)
        {
            Id = id;
            this.fullName = string.Empty;
            this.email = string.Empty;
            this.phone = string.Empty;
            FullName = fullName;
            Age = age;
            Email = email;
            Phone = phone;
        }

        public virtual string GetInfo()
        {
            return $"{Role} | ID: {Id} | {FullName}, {Age} лет | Email: {Email} | Тел.: {Phone}";
        }

        public override string ToString()
        {
            return GetInfo();
        }

        public static bool IsValidEmail(string text)
        {
            int at = text.IndexOf('@');
            return at > 0 && at == text.LastIndexOf('@') && text.IndexOf('.', at) > at + 1
                   && !text.EndsWith(".") && !text.Contains(" ");
        }

        protected static string RequireText(string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"Поле \"{fieldName}\" не может быть пустым.");
            }
            return value.Trim();
        }
    }
}
