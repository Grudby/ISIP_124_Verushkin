using System;
using System.Collections.Generic;

namespace Pr6_Roguelike
{
    /// <summary>
    /// Общие характеристики и порядок атаки любого врага. Особенности типов задаются переопределением.
    /// </summary>
    public abstract class Enemy
    {
        public const double DodgeChance = 0.4;

        public string Name { get; }
        public int MaxHp { get; }
        public int Hp { get; private set; }
        public int Attack { get; }
        public int Defense { get; }
        public bool IsAlive => Hp > 0;
        public virtual bool IsBoss => false;

        public abstract string Race { get; }

        protected virtual double CritChance => 0;
        protected virtual double FreezeChance => 0;
        protected virtual bool IgnoresDefense => false;

        protected Enemy(string name, int hp, int attack, int defense)
        {
            Name = name;
            MaxHp = hp;
            Hp = hp;
            Attack = attack;
            Defense = defense;
        }

        public int TakeHit(int attack)
        {
            int damage = Math.Max(1, attack - Defense);
            Hp = Math.Max(0, Hp - damage);
            return damage;
        }

        public List<string> AttackPlayer(Player player)
        {
            List<string> log = new List<string>();

            if (player.IsDefending && Rng.Chance(DodgeChance))
            {
                log.Add($"{Name} атакует, но вы уклоняетесь!");
                return log;
            }

            double damage = Attack;
            if (CritChance > 0 && Rng.Chance(CritChance))
            {
                damage *= 2;
                log.Add("Критический удар!");
            }

            if (IgnoresDefense)
            {
                log.Add("Удар проходит сквозь броню.");
            }
            else if (player.IsDefending)
            {
                double blockPercent = Rng.Range(0.7, 1.0);
                double blocked = player.Defense * blockPercent;
                damage -= blocked;
                log.Add($"Блок поглощает {blocked:0.#} урона ({blockPercent:P0} от защиты).");
            }
            else
            {
                damage -= player.Defense / 2.0;
            }

            int finalDamage = Math.Max(0, (int)Math.Round(damage));
            player.TakeDamage(finalDamage);
            log.Add($"{Name} наносит вам {finalDamage} урона.");

            if (FreezeChance > 0 && player.IsAlive && Rng.Chance(FreezeChance))
            {
                player.IsFrozen = true;
                log.Add("Вас заморозили! Следующий ход вы пропускаете.");
            }

            return log;
        }

        public override string ToString()
        {
            string title = IsBoss ? $"БОСС {Name} (раса {Race})" : Name;
            return $"{title} | Здоровье: {Hp}/{MaxHp} | Атака: {Attack} | Защита: {Defense}";
        }
    }
}
