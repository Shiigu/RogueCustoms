using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class ListHelpers
    {
        public static void AddRange<T>(this ConcurrentBag<T> bag, IEnumerable<T> toAdd)
        {
            foreach (var element in toAdd)
            {
                bag.Add(element);
            }
        }
        public static bool HasMinimumMatches<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, int minimum)
        {
            if (minimum < 2)
                throw new ArgumentException("Minimum must be 2 or greater.", nameof(minimum));

            HashSet<TKey> seenKeys = new HashSet<TKey>();
            int matchCount = 0;

            foreach (var item in source)
            {
                var key = keySelector(item);
                if (seenKeys.Contains(key))
                {
                    matchCount++;
                    if (matchCount + 1 >= minimum) // +1 to count initial item
                        return true;
                }
                else
                {
                    seenKeys.Add(key);
                }
            }

            return false;
        }
    }
}
