using System;
using System.Collections.Generic;
using System.Text;

namespace Pr5_University
{
    internal class Program
    {
        private static readonly University University = new University();

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("Практическая работа 5: система управления университетом");
            Console.WriteLine("Верушкин Василий Андреевич, ИСиП-124");

            University.SeedTestData();

            try
            {
                RunMenu();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Работа завершена.");
        }

        private static void RunMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== Студенты =====");
                Console.WriteLine("1 - Добавить студента");
                Console.WriteLine("2 - Все студенты");
                Console.WriteLine("3 - Информация о студенте");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите пункт: ");

                string choice = InputHelper.ReadLine().Trim();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddStudent();
                            break;
                        case "2":
                            PrintList(University.Students, "Все студенты");
                            break;
                        case "3":
                            ShowStudent();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Нет такого пункта меню.");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        private static void AddStudent()
        {
            Console.WriteLine("=== Новый студент ===");
            string fullName = InputHelper.ReadString("ФИО: ");
            int age = InputHelper.ReadInt("Возраст (14-100): ", 14, 100);
            string email = InputHelper.ReadEmail("Email: ");
            string phone = InputHelper.ReadPhone("Телефон: ");
            string group = InputHelper.ReadString("Группа: ");

            Student student = University.AddStudent(fullName, age, email, phone, group);
            Console.WriteLine("Студент добавлен:");
            Console.WriteLine(student.GetInfo());
        }

        private static void ShowStudent()
        {
            Student? student = SelectStudent();
            if (student != null)
            {
                Console.WriteLine(student.GetInfo());
            }
        }

        private static Student? SelectStudent()
        {
            if (University.Students.Count == 0)
            {
                Console.WriteLine("Студентов нет.");
                return null;
            }

            foreach (Student item in University.Students)
            {
                Console.WriteLine($"  {item.Id} - {item.FullName} ({item.Group})");
            }
            int id = InputHelper.ReadInt("Введите ID студента: ", 1, int.MaxValue);
            Student? student = University.FindStudent(id);
            if (student == null)
            {
                Console.WriteLine($"Студент с ID {id} не найден.");
            }
            return student;
        }

        private static void PrintList<T>(IReadOnlyCollection<T> items, string title)
        {
            Console.WriteLine($"=== {title} ===");
            if (items.Count == 0)
            {
                Console.WriteLine("Список пуст.");
                return;
            }

            foreach (T item in items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"Всего: {items.Count}");
        }
    }
}
