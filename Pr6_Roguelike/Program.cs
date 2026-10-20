using System;
using System.IO;
using System.Text;

namespace Pr6_Roguelike
{
    internal class Program
    {
        private static Game game = new Game();

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("Практическая работа 6: текстовый рогалик");
            Console.WriteLine("Верушкин Василий Андреевич, ИСиП-124");

            try
            {
                RunMenu();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Игра завершена.");
        }

        private static void RunMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 - Следующий ход");
                Console.WriteLine("2 - Состояние героя");
                Console.WriteLine("3 - Сохранить прогресс");
                Console.WriteLine("4 - Загрузить прогресс");
                Console.WriteLine("0 - Выход");
                string choice = InputHelper.ReadChoice("Выберите пункт: ", "1", "2", "3", "4", "0");

                switch (choice)
                {
                    case "1":
                        game.PlayTurn();
                        if (game.IsOver)
                        {
                            ShowGameOver();
                            return;
                        }
                        break;
                    case "2":
                        game.PrintStatus();
                        break;
                    case "3":
                        SaveGame();
                        break;
                    case "4":
                        LoadGame();
                        break;
                    case "0":
                        return;
                }
            }
        }

        private static void SaveGame()
        {
            try
            {
                SaveManager.Save(game);
                Console.WriteLine($"Прогресс сохранён в файл {SaveManager.FileName} (ход {game.Turn}).");
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                Console.WriteLine($"Не удалось сохранить: {ex.Message}");
            }
        }

        private static void LoadGame()
        {
            Game? loaded = SaveManager.Load(out string error);
            if (loaded == null)
            {
                Console.WriteLine(error);
                return;
            }

            game = loaded;
            Console.WriteLine("Прогресс загружен.");
            game.PrintStatus();
        }

        private static void ShowGameOver()
        {
            Console.WriteLine();
            Console.WriteLine("======== ВЫ ПОГИБЛИ ========");
            Console.WriteLine($"Пройдено ходов: {game.Turn}");
            Console.WriteLine($"Побеждено врагов: {game.Kills}, из них боссов: {game.BossKills}");
            Console.WriteLine($"Открыто сундуков: {game.ChestsOpened}");
        }
    }
}
