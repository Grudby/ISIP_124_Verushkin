using System;

namespace Pr1_Expenses
{
    /// <summary>
    /// Одна трата: название товара или услуги и сумма в рублях.
    /// </summary>
    class Expense
    {
        public string Name { get; set; }
        public double Amount { get; set; }

        public Expense(string name, double amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
