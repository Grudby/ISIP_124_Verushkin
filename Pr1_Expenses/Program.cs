using System;
using System.Collections.Generic;
using System.Text;

namespace Pr1_Expenses
{
    class Program
    {
        // Список всех введённых трат
        static List<Expense> expenses = new List<Expense>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("Учёт потраченных за день средств");
            Console.WriteLine("Верушкин Василий Андреевич, ИСиП-124");
            Console.WriteLine("=================================");
        }
    }
}
