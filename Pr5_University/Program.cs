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
                Console.WriteLine("===== Преподаватели =====");
                Console.WriteLine("4 - Добавить преподавателя");
                Console.WriteLine("5 - Все преподаватели");
                Console.WriteLine("6 - Информация о преподавателе");
                Console.WriteLine("===== Курсы =====");
                Console.WriteLine("7 - Создать курс");
                Console.WriteLine("8 - Все курсы");
                Console.WriteLine("9 - Информация о курсе и его студенты");
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
                        case "4":
                            AddTeacher();
                            break;
                        case "5":
                            PrintList(University.Teachers, "Все преподаватели");
                            break;
                        case "6":
                            ShowTeacher();
                            break;
                        case "7":
                            AddCourse();
                            break;
                        case "8":
                            PrintList(University.Courses, "Все курсы");
                            break;
                        case "9":
                            ShowCourse();
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

        private static void AddTeacher()
        {
            Console.WriteLine("=== Новый преподаватель ===");
            string fullName = InputHelper.ReadString("ФИО: ");
            int age = InputHelper.ReadInt("Возраст (20-100): ", 20, 100);
            string email = InputHelper.ReadEmail("Email: ");
            string phone = InputHelper.ReadPhone("Телефон: ");
            string department = InputHelper.ReadString("Кафедра: ");
            int experience = InputHelper.ReadInt($"Стаж в годах (0-{age - 18}): ", 0, age - 18);

            Teacher teacher = University.AddTeacher(fullName, age, email, phone, department, experience);
            Console.WriteLine("Преподаватель добавлен:");
            Console.WriteLine(teacher.GetInfo());
        }

        private static void ShowTeacher()
        {
            Teacher? teacher = SelectTeacher();
            if (teacher != null)
            {
                Console.WriteLine(teacher.GetInfo());
            }
        }

        private static Teacher? SelectTeacher()
        {
            if (University.Teachers.Count == 0)
            {
                Console.WriteLine("Преподавателей нет.");
                return null;
            }

            foreach (Teacher item in University.Teachers)
            {
                Console.WriteLine($"  {item.Id} - {item.FullName} ({item.Department})");
            }
            int id = InputHelper.ReadInt("Введите ID преподавателя: ", 1, int.MaxValue);
            Teacher? teacher = University.FindTeacher(id);
            if (teacher == null)
            {
                Console.WriteLine($"Преподаватель с ID {id} не найден.");
            }
            return teacher;
        }

        private static void AddCourse()
        {
            Console.WriteLine("=== Новый курс ===");
            string title = InputHelper.ReadString("Название курса: ");
            int hours = InputHelper.ReadInt("Количество часов (1-500): ", 1, 500);

            Course course = University.AddCourse(title, hours);
            Console.WriteLine("Курс создан:");
            Console.WriteLine(course);
        }

        private static void ShowCourse()
        {
            Course? course = SelectCourse();
            if (course == null)
            {
                return;
            }

            Console.WriteLine(course);
            if (course.Teacher != null)
            {
                Console.WriteLine($"Преподаватель: {course.Teacher.GetInfo()}");
            }
            PrintList(course.Students, $"Студенты курса \"{course.Title}\"");
        }

        private static Course? SelectCourse()
        {
            if (University.Courses.Count == 0)
            {
                Console.WriteLine("Курсов нет.");
                return null;
            }

            foreach (Course item in University.Courses)
            {
                Console.WriteLine($"  {item.Id} - {item.Title}");
            }
            int id = InputHelper.ReadInt("Введите ID курса: ", 1, int.MaxValue);
            Course? course = University.FindCourse(id);
            if (course == null)
            {
                Console.WriteLine($"Курс с ID {id} не найден.");
            }
            return course;
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
