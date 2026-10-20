using System;
using System.Collections.Generic;
using System.Linq;

namespace Pr5_University
{
    public class Teacher : Person
    {
        private readonly List<Course> courses = new List<Course>();
        private string department;
        private int experience;

        public string Department
        {
            get => department;
            set => department = RequireText(value, "Кафедра");
        }

        public int Experience
        {
            get => experience;
            set
            {
                if (value < 0 || value > Age - 18)
                {
                    throw new ArgumentException($"Стаж должен быть от 0 до {Age - 18} лет.");
                }
                experience = value;
            }
        }

        public IReadOnlyList<Course> Courses => courses;

        public override string Role => "Преподаватель";

        protected override int MinAge => 20;

        public Teacher(int id, string fullName, int age, string email, string phone, string department, int experience)
            : base(id, fullName, age, email, phone)
        {
            this.department = string.Empty;
            Department = department;
            Experience = experience;
        }

        internal void AddCourse(Course course)
        {
            if (!courses.Contains(course))
            {
                courses.Add(course);
            }
        }

        internal void RemoveCourse(Course course)
        {
            courses.Remove(course);
        }

        public override string GetInfo()
        {
            string courseList = courses.Count == 0
                ? "нет"
                : string.Join(", ", courses.Select(course => course.Title));
            return base.GetInfo() + $" | Кафедра: {Department} | Стаж: {Experience} лет | Ведёт: {courseList}";
        }
    }
}
