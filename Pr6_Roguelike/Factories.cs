namespace Pr6_Roguelike
{
    public static class EnemyFactory
    {
        public static Enemy CreateRandom()
        {
            switch (Rng.Range(1, 3))
            {
                case 1:
                    return new Goblin();
                case 2:
                    return new Skeleton();
                default:
                    return new Mage();
            }
        }

        public static Enemy CreateRandomBoss()
        {
            switch (Rng.Range(1, 4))
            {
                case 1:
                    return new Vvg();
                case 2:
                    return new Kovalsky();
                case 3:
                    return new ArchmageCpp();
                default:
                    return new PestovCMinusMinus();
            }
        }
    }

    /// <summary>
    /// Содержимое сундука: зелье, оружие или доспех. Сила снаряжения зависит от редкости.
    /// </summary>
    public static class LootFactory
    {
        private static readonly string[] WeaponNames = { "Меч", "Топор", "Булава", "Копьё", "Кинжал", "Молот" };
        private static readonly string[] ArmorNames = { "Кольчуга", "Латы", "Кираса", "Плащ", "Нагрудник" };

        public static Item CreateRandom()
        {
            switch (Rng.Range(1, 3))
            {
                case 1:
                    return new HealingPotion();
                case 2:
                    return CreateWeapon();
                default:
                    return CreateArmor();
            }
        }

        public static Weapon CreateWeapon()
        {
            Rarity rarity = RarityInfo.Roll();
            int attack;
            switch (rarity)
            {
                case Rarity.Rare:
                    attack = Rng.Range(12, 16);
                    break;
                case Rarity.Epic:
                    attack = Rng.Range(16, 22);
                    break;
                case Rarity.Legendary:
                    attack = Rng.Range(22, 30);
                    break;
                default:
                    attack = Rng.Range(7, 12);
                    break;
            }
            return new Weapon(Rng.Pick(WeaponNames), rarity, attack);
        }

        public static Armor CreateArmor()
        {
            Rarity rarity = RarityInfo.Roll();
            int defense;
            switch (rarity)
            {
                case Rarity.Rare:
                    defense = Rng.Range(5, 8);
                    break;
                case Rarity.Epic:
                    defense = Rng.Range(8, 11);
                    break;
                case Rarity.Legendary:
                    defense = Rng.Range(11, 15);
                    break;
                default:
                    defense = Rng.Range(3, 5);
                    break;
            }
            return new Armor(Rng.Pick(ArmorNames), rarity, defense);
        }
    }
}
