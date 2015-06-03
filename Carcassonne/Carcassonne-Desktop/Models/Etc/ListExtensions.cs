using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassonne_Desktop.Models.Etc
{
    public static class ListExtensions
    {
        private static Random random = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetRandomElement<T>(this IList<T> list)
        {
            return list.ElementAt(random.Next(list.Count));
        }
    }
}
