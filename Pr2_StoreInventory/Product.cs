namespace Pr2_StoreInventory
{
    /// <summary>
    /// Товар на складе магазина.
    /// </summary>
    public class Product
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public Category Category { get; set; }

        public override string ToString()
        {
            return $"Код: {Code} | Название: {Name} | Цена: {Price:0.00} | Количество: {Quantity} | " +
                   $"В наличии: {(InStock ? "Да" : "Нет")} | Категория: {Category}";
        }
    }
}
