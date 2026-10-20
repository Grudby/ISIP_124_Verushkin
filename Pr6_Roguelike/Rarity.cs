namespace Pr6_Roguelike
{
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    public static class RarityInfo
    {
        public static string GetName(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.Rare:
                    return "Редкий";
                case Rarity.Epic:
                    return "Эпический";
                case Rarity.Legendary:
                    return "Легендарный";
                default:
                    return "Обычный";
            }
        }

        /// <summary>
        /// Шанс выпадения: обычный 60%, редкий 25%, эпический 12%, легендарный 3%.
        /// </summary>
        public static Rarity Roll()
        {
            int roll = Rng.Range(1, 100);
            if (roll <= 60)
            {
                return Rarity.Common;
            }
            if (roll <= 85)
            {
                return Rarity.Rare;
            }
            if (roll <= 97)
            {
                return Rarity.Epic;
            }
            return Rarity.Legendary;
        }
    }
}
