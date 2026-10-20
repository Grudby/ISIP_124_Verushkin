using System.Collections.Generic;
using System.Linq;

namespace Pr5_University
{
    /// <summary>
    /// Хранит студентов, преподавателей и курсы. Идентификаторы назначаются автоматически.
    /// </summary>
    public class University
    {
        private readonly List<Student> students = new List<Student>();
        private readonly List<Teacher> teachers = new List<Teacher>();
        private readonly List<Course> courses = new List<Course>();
        private int nextPersonId = 1;
        private int nextCourseId = 1;

        public IReadOnlyList<Student> Students => students;
        public IReadOnlyList<Teacher> Teachers => teachers;
        public IReadOnlyList<Course> Courses => courses;

        public Student AddStudent(string fullName, int age, string email, string phone, string group)
        {
            Student student = new Student(nextPersonId, fullName, age, email, phone, group);
            nextPersonId++;
            students.Add(student);
            return student;
        }

        public Teacher AddTeacher(string fullName, int age, string email, string phone, string department, int experience)
        {
            Teacher teacher = new Teacher(nextPersonId, fullName, age, email, phone, department, experience);
            nextPersonId++;
            teachers.Add(teacher);
            return teacher;
        }

        public Course AddCourse(string title, int hours)
        {
            Course course = new Course(nextCourseId, title, hours);
            nextCourseId++;
            courses.Add(course);
            return course;
        }

        public Student? FindStudent(int id)
        {
            return students.FirstOrDefault(student => student.Id == id);
        }

        public Teacher? FindTeacher(int id)
        {
            return teachers.FirstOrDefault(teacher => teacher.Id == id);
        }

        public Course? FindCourse(int id)
        {
            return courses.FirstOrDefault(course => course.Id == id);
        }

        /// <summary>
        /// Все участники учебного процесса через базовый класс Person.
        /// </summary>
        public IReadOnlyList<Person> GetAllPeople()
        {
            return students.Cast<Person>()
                .Concat(teachers)
                .OrderBy(person => person.Id)
                .ToList();
        }

        public void SeedTestData()
        {
            Teacher ivanova = AddTeacher("Иванова Мария Петровна", 45, "ivanova@college.ru", "+7 900 111-22-33", "Информатика", 20);
            Teacher petrov = AddTeacher("Петров Алексей Игоревич", 38, "petrov@college.ru", "+7 900 222-33-44", "Математика", 12);

            Student smirnov = AddStudent("Смирнов Артём Олегович", 18, "smirnov@mail.ru", "+7 911 123-45-67", "ИСиП-124");
            Student kuznetsova = AddStudent("Кузнецова Анна Сергеевна", 19, "kuznetsova@mail.ru", "+7 911 234-56-78", "ИСиП-124");
            Student volkov = AddStudent("Волков Дмитрий Андреевич", 17, "volkov@mail.ru", "+7 911 345-67-89", "ИСиП-125");

            Course programming = AddCourse("Основы программирования на C#", 72);
            Course databases = AddCourse("Базы данных", 54);
            Course math = AddCourse("Дискретная математика", 36);

            programming.AssignTeacher(ivanova);
            databases.AssignTeacher(ivanova);
            math.AssignTeacher(petrov);

            programming.Enroll(smirnov);
            programming.Enroll(kuznetsova);
            databases.Enroll(smirnov);
            math.Enroll(volkov);
        }
    }
}
