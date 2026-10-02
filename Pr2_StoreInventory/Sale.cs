using System;

namespace Pr2_StoreInventory
{
    /// <summary>
    /// Продажа товара. Хранится в истории для отмены и отчёта.
    /// </summary>
    public class Sale
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"{Date:dd.MM.yyyy HH:mm} | {Product.Name} | Кол-во: {Quantity} | Сумма: {TotalPrice:0.00}";
        }
    }
}
