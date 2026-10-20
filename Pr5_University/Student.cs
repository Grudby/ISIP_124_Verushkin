using System.Collections.Generic;
using System.Linq;

namespace Pr5_University
{
    public class Student : Person
    {
        private readonly List<Course> courses = new List<Course>();
        private string group;

        public string Group
        {
            get => group;
            set => group = RequireText(value, "Группа");
        }

        public IReadOnlyList<Course> Courses => courses;

        public override string Role => "Студент";

        protected override int MinAge => 14;

        public Student(int id, string fullName, int age, string email, string phone, string group)
            : base(id, fullName, age, email, phone)
        {
            this.group = string.Empty;
            Group = group;
        }

        internal void AddCourse(Course course)
        {
            if (!courses.Contains(course))
            {
                courses.Add(course);
            }
        }

        public override string GetInfo()
        {
            string courseList = courses.Count == 0
                ? "нет"
                : string.Join(", ", courses.Select(course => course.Title));
            return base.GetInfo() + $" | Группа: {Group} | Курсы: {courseList}";
        }
    }
}
