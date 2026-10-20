namespace Pr6_Roguelike
{
    public class Vvg : Goblin
    {
        public override bool IsBoss => true;
        protected override double CritChance => BaseCritChance + 0.1;

        public Vvg() : base("ВВГ", 2.0, 1.5, 1.2)
        {
        }
    }

    public class Kovalsky : Skeleton
    {
        public override bool IsBoss => true;

        public Kovalsky() : base("Ковальский", 2.5, 1.3, 1.4)
        {
        }
    }

    public class ArchmageCpp : Mage
    {
        public override bool IsBoss => true;
        protected override double FreezeChance => BaseFreezeChance + 0.1;

        public ArchmageCpp() : base("Архимаг C++", 1.8, 1.6, 1.1)
        {
        }
    }

    /// <summary>
    /// Босс расы Скелет: игнорирует защиту и при этом может заморозить, как маг.
    /// </summary>
    public class PestovCMinusMinus : Skeleton
    {
        public override bool IsBoss => true;
        protected override double FreezeChance => Mage.BaseFreezeChance + 0.15;

        public PestovCMinusMinus() : base("Пестов С--", 1.3, 1.8, 0.6)
        {
        }
    }
}
