using System;

namespace Pr6_Roguelike
{
    /// <summary>
    /// Пошаговый ход игры: на каждом ходу сундук или враг, каждый 10-й ход - босс.
    /// </summary>
    public class Game
    {
        public const int BossEvery = 10;

        public Player Player { get; private set; }
        public int Turn { get; private set; }
        public int Kills { get; private set; }
        public int BossKills { get; private set; }
        public int ChestsOpened { get; private set; }
        public bool IsOver => !Player.IsAlive;

        public Game()
        {
            Player = new Player();
        }

        public Game(Player player, int turn, int kills, int bossKills, int chestsOpened)
        {
            Player = player;
            Turn = turn;
            Kills = kills;
            BossKills = bossKills;
            ChestsOpened = chestsOpened;
        }

        public void PlayTurn()
        {
            Turn++;
            Console.WriteLine();
            Console.WriteLine($"========== Ход {Turn} ==========");

            if (Turn % BossEvery == 0)
            {
                Enemy boss = EnemyFactory.CreateRandomBoss();
                Console.WriteLine($"Земля дрожит... Перед вами босс {boss.Name}!");
                Fight(boss);
            }
            else if (Rng.Chance(0.5))
            {
                OpenChest();
            }
            else
            {
                Enemy enemy = EnemyFactory.CreateRandom();
                Console.WriteLine($"На вас напал {enemy.Name}!");
                Fight(enemy);
            }
        }

        private void Fight(Enemy enemy)
        {
            Player.IsFrozen = false;
            int round = 0;

            while (Player.IsAlive && enemy.IsAlive)
            {
                round++;
                Console.WriteLine();
                Console.WriteLine($"--- Раунд {round} ---");
                Console.WriteLine($"Вы: {Player.Hp}/{Player.MaxHp} HP | Атака {Player.Attack} | Защита {Player.Defense}");
                Console.WriteLine($"Враг: {enemy}");

                Player.IsDefending = false;
                if (Player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    Player.IsFrozen = false;
                }
                else
                {
                    string action = InputHelper.ReadChoice("1 - Атака, 2 - Защита: ", "1", "2");
                    if (action == "1")
                    {
                        int damage = enemy.TakeHit(Player.Attack);
                        Console.WriteLine($"Вы наносите {enemy.Name} {damage} урона.");
                        if (!enemy.IsAlive)
                        {
                            break;
                        }
                    }
                    else
                    {
                        Player.IsDefending = true;
                        Console.WriteLine("Вы встаёте в защитную стойку.");
                    }
                }

                foreach (string line in enemy.AttackPlayer(Player))
                {
                    Console.WriteLine(line);
                }
            }

            Player.IsDefending = false;
            Player.IsFrozen = false;

            if (Player.IsAlive)
            {
                Kills++;
                if (enemy.IsBoss)
                {
                    BossKills++;
                }
                Console.WriteLine($"Победа! {enemy.Name} повержен. Осталось здоровья: {Player.Hp}/{Player.MaxHp}");
            }
            else
            {
                Console.WriteLine($"{enemy.Name} одержал победу...");
            }
        }

        private void OpenChest()
        {
            ChestsOpened++;
            Item item = LootFactory.CreateRandom();
            Console.WriteLine("Вы нашли сундук!");
            Console.WriteLine($"Внутри: {item.Describe()}");

            switch (item)
            {
                case HealingPotion:
                    Player.HealFull();
                    Console.WriteLine($"Здоровье полностью восстановлено: {Player.Hp}/{Player.MaxHp}");
                    break;
                case Weapon weapon:
                    Console.WriteLine($"Текущее: {Player.Weapon.Describe()}");
                    if (AskTake())
                    {
                        Player.Equip(weapon);
                        Console.WriteLine("Вы взяли новое оружие.");
                    }
                    else
                    {
                        Console.WriteLine("Вы выбросили находку.");
                    }
                    break;
                case Armor armor:
                    Console.WriteLine($"Текущее: {Player.Armor.Describe()}");
                    if (AskTake())
                    {
                        Player.Equip(armor);
                        Console.WriteLine("Вы надели новый доспех.");
                    }
                    else
                    {
                        Console.WriteLine("Вы выбросили находку.");
                    }
                    break;
            }
        }

        private static bool AskTake()
        {
            return InputHelper.ReadChoice("1 - Взять (заменить текущий), 2 - Выбросить: ", "1", "2") == "1";
        }

        public void PrintStatus()
        {
            Console.WriteLine($"Ход: {Turn} | Побеждено врагов: {Kills} (боссов: {BossKills}) | Открыто сундуков: {ChestsOpened}");
            Console.WriteLine(Player);
            Console.WriteLine($"До босса ходов: {BossEvery - Turn % BossEvery}");
        }
    }
}
