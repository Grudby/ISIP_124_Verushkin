using System;

namespace Pr6_Roguelike
{
    /// <summary>
    /// Общий генератор случайных чисел для всех шансов и случайных величин игры.
    /// </summary>
    public static class Rng
    {
        private static readonly Random Random = new Random();

        public static bool Chance(double probability)
        {
            return Random.NextDouble() < probability;
        }

        public static int Range(int min, int max)
        {
            return Random.Next(min, max + 1);
        }

        public static double Range(double min, double max)
        {
            return min + Random.NextDouble() * (max - min);
        }

        public static T Pick<T>(T[] items)
        {
            return items[Random.Next(items.Length)];
        }
    }
}
