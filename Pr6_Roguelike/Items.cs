namespace Pr6_Roguelike
{
    public abstract class Item
    {
        public string Name { get; }

        protected Item(string name)
        {
            Name = name;
        }

        public abstract string Describe();

        public override string ToString()
        {
            return Describe();
        }
    }

    public abstract class Equipment : Item
    {
        public Rarity Rarity { get; }

        protected Equipment(string name, Rarity rarity) : base(name)
        {
            Rarity = rarity;
        }
    }

    public class Weapon : Equipment
    {
        public int Attack { get; }

        public Weapon(string name, Rarity rarity, int attack) : base(name, rarity)
        {
            Attack = attack;
        }

        public override string Describe()
        {
            return $"Оружие \"{Name}\" [{RarityInfo.GetName(Rarity)}] - атака {Attack}";
        }
    }

    public class Armor : Equipment
    {
        public int Defense { get; }

        public Armor(string name, Rarity rarity, int defense) : base(name, rarity)
        {
            Defense = defense;
        }

        public override string Describe()
        {
            return $"Доспех \"{Name}\" [{RarityInfo.GetName(Rarity)}] - защита {Defense}";
        }
    }

    public class HealingPotion : Item
    {
        public HealingPotion() : base("Лечебное зелье")
        {
        }

        public override string Describe()
        {
            return "Лечебное зелье - полностью восстанавливает здоровье";
        }
    }
}
