using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace LeftOut.Extensions
{
    public static class ListExtensions
    {
        public static T SelectRandom<T>(this List<T> self) =>
            self.Count > 0
            ? self[Random.Range(0, self.Count)]
            : throw new ArgumentOutOfRangeException(
                $"Can't {nameof(SelectRandom)} from an empty list.");

        public static void Shuffle<T>(this List<T> self)
        {
            var n = self.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (self[k], self[n]) = (self[n], self[k]);
            }
        }

        public static List<T> GetShuffledCopy<T>(this List<T> self)
        {
            var copy = new List<T>(self);
            copy.Shuffle();
            return copy;
        }

        public static List<T> PruneNull<T>(this List<T> self) where T : class
        {
            for (var i = self.Count; i >= 0; --i)
            {
                if (self[i] == null)
                {
                    self.RemoveAt(i);
                }
            }

            return self;
        }
    }
}
