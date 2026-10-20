using System;
using System.IO;
using System.Text.Json;

namespace Pr6_Roguelike
{
    public class SaveData
    {
        public int Turn { get; set; }
        public int Kills { get; set; }
        public int BossKills { get; set; }
        public int ChestsOpened { get; set; }
        public int MaxHp { get; set; }
        public int Hp { get; set; }
        public string WeaponName { get; set; } = string.Empty;
        public Rarity WeaponRarity { get; set; }
        public int WeaponAttack { get; set; }
        public string ArmorName { get; set; } = string.Empty;
        public Rarity ArmorRarity { get; set; }
        public int ArmorDefense { get; set; }
    }

    /// <summary>
    /// Сохранение и загрузка прогресса в JSON-файл рядом с программой.
    /// </summary>
    public static class SaveManager
    {
        public const string FileName = "save.json";

        public static bool HasSave => File.Exists(FileName);

        public static void Save(Game game)
        {
            Player player = game.Player;
            SaveData data = new SaveData
            {
                Turn = game.Turn,
                Kills = game.Kills,
                BossKills = game.BossKills,
                ChestsOpened = game.ChestsOpened,
                MaxHp = player.MaxHp,
                Hp = player.Hp,
                WeaponName = player.Weapon.Name,
                WeaponRarity = player.Weapon.Rarity,
                WeaponAttack = player.Weapon.Attack,
                ArmorName = player.Armor.Name,
                ArmorRarity = player.Armor.Rarity,
                ArmorDefense = player.Armor.Defense
            };

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }

        public static Game? Load(out string error)
        {
            error = string.Empty;
            if (!HasSave)
            {
                error = "Сохранение не найдено.";
                return null;
            }

            try
            {
                SaveData? data = JsonSerializer.Deserialize<SaveData>(File.ReadAllText(FileName));
                if (data == null || !IsValid(data))
                {
                    error = "Файл сохранения повреждён.";
                    return null;
                }

                Weapon weapon = new Weapon(data.WeaponName, data.WeaponRarity, data.WeaponAttack);
                Armor armor = new Armor(data.ArmorName, data.ArmorRarity, data.ArmorDefense);
                Player player = new Player(data.MaxHp, data.Hp, weapon, armor);
                return new Game(player, data.Turn, data.Kills, data.BossKills, data.ChestsOpened);
            }
            catch (Exception ex) when (ex is JsonException || ex is IOException || ex is UnauthorizedAccessException)
            {
                error = $"Не удалось прочитать сохранение: {ex.Message}";
                return null;
            }
        }

        private static bool IsValid(SaveData data)
        {
            return data.Turn >= 0 && data.Kills >= 0 && data.BossKills >= 0 && data.ChestsOpened >= 0
                   && data.MaxHp > 0 && data.Hp > 0 && data.Hp <= data.MaxHp
                   && data.WeaponName.Length > 0 && data.WeaponAttack > 0
                   && data.ArmorName.Length > 0 && data.ArmorDefense >= 0
                   && Enum.IsDefined(typeof(Rarity), data.WeaponRarity)
                   && Enum.IsDefined(typeof(Rarity), data.ArmorRarity);
        }
    }
}
