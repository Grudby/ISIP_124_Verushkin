using System;

namespace Pr6_Roguelike
{
    public class Player
    {
        public const int StartMaxHp = 100;

        public int MaxHp { get; }
        public int Hp { get; private set; }
        public Weapon Weapon { get; private set; }
        public Armor Armor { get; private set; }
        public bool IsDefending { get; set; }
        public bool IsFrozen { get; set; }
        public bool IsAlive => Hp > 0;

        public int Attack => Weapon.Attack;
        public int Defense => Armor.Defense;

        public Player()
            : this(StartMaxHp, StartMaxHp,
                   new Weapon("Ржавый меч", Rarity.Common, 12),
                   new Armor("Кожаная куртка", Rarity.Common, 5))
        {
        }

        public Player(int maxHp, int hp, Weapon weapon, Armor armor)
        {
            MaxHp = maxHp;
            Hp = Math.Clamp(hp, 1, maxHp);
            Weapon = weapon;
            Armor = armor;
        }

        public void TakeDamage(int damage)
        {
            Hp = Math.Max(0, Hp - Math.Max(0, damage));
        }

        public void HealFull()
        {
            Hp = MaxHp;
        }

        public void Equip(Weapon weapon)
        {
            Weapon = weapon;
        }

        public void Equip(Armor armor)
        {
            Armor = armor;
        }

        public override string ToString()
        {
            return $"Здоровье: {Hp}/{MaxHp} | Атака: {Attack} | Защита: {Defense}\n" +
                   $"  {Weapon.Describe()}\n  {Armor.Describe()}";
        }
    }
}
