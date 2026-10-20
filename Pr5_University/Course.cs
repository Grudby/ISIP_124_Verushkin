using System;
using System.Collections.Generic;

namespace Pr5_University
{
    public class Course
    {
        private readonly List<Student> students = new List<Student>();
        private string title;
        private int hours;

        public int Id { get; }

        public string Title
        {
            get => title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Название курса не может быть пустым.");
                }
                title = value.Trim();
            }
        }

        public int Hours
        {
            get => hours;
            set
            {
                if (value < 1 || value > 500)
                {
                    throw new ArgumentException("Количество часов должно быть от 1 до 500.");
                }
                hours = value;
            }
        }

        public Teacher? Teacher { get; private set; }

        public IReadOnlyList<Student> Students => students;

        public Course(int id, string title, int hours)
        {
            Id = id;
            this.title = string.Empty;
            Title = title;
            Hours = hours;
        }

        public bool Enroll(Student student)
        {
            if (students.Contains(student))
            {
                return false;
            }
            students.Add(student);
            student.AddCourse(this);
            return true;
        }

        public void AssignTeacher(Teacher teacher)
        {
            Teacher?.RemoveCourse(this);
            Teacher = teacher;
            teacher.AddCourse(this);
        }

        public override string ToString()
        {
            string teacherName = Teacher == null ? "не назначен" : Teacher.FullName;
            return $"Курс | ID: {Id} | {Title} | {Hours} ч. | Преподаватель: {teacherName} | Студентов: {students.Count}";
        }
    }
}
