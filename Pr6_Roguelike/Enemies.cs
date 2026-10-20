using System;

namespace Pr6_Roguelike
{
    public class Goblin : Enemy
    {
        public const double BaseCritChance = 0.2;
        private const int BaseHp = 30;
        private const int BaseAttack = 9;
        private const int BaseDefense = 2;

        public override string Race => "Гоблин";
        protected override double CritChance => BaseCritChance;

        public Goblin() : base("Гоблин", BaseHp, BaseAttack, BaseDefense)
        {
        }

        protected Goblin(string name, double hpMultiplier, double attackMultiplier, double defenseMultiplier)
            : base(name, Scale(BaseHp, hpMultiplier), Scale(BaseAttack, attackMultiplier), Scale(BaseDefense, defenseMultiplier))
        {
        }

        protected static int Scale(int value, double multiplier)
        {
            return (int)Math.Round(value * multiplier);
        }
    }

    public class Skeleton : Enemy
    {
        private const int BaseHp = 35;
        private const int BaseAttack = 7;
        private const int BaseDefense = 3;

        public override string Race => "Скелет";
        protected override bool IgnoresDefense => true;

        public Skeleton() : base("Скелет", BaseHp, BaseAttack, BaseDefense)
        {
        }

        protected Skeleton(string name, double hpMultiplier, double attackMultiplier, double defenseMultiplier)
            : base(name, (int)Math.Round(BaseHp * hpMultiplier), (int)Math.Round(BaseAttack * attackMultiplier),
                   (int)Math.Round(BaseDefense * defenseMultiplier))
        {
        }
    }

    public class Mage : Enemy
    {
        public const double BaseFreezeChance = 0.2;
        private const int BaseHp = 25;
        private const int BaseAttack = 11;
        private const int BaseDefense = 1;

        public override string Race => "Маг";
        protected override double FreezeChance => BaseFreezeChance;

        public Mage() : base("Маг", BaseHp, BaseAttack, BaseDefense)
        {
        }

        protected Mage(string name, double hpMultiplier, double attackMultiplier, double defenseMultiplier)
            : base(name, (int)Math.Round(BaseHp * hpMultiplier), (int)Math.Round(BaseAttack * attackMultiplier),
                   (int)Math.Round(BaseDefense * defenseMultiplier))
        {
        }
    }
}
