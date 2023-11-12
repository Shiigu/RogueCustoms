using RogueCustomsGameEngine.Game.DungeonStructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueCustomsGameEngine.Utils.Helpers
{
    public static class EnumerableHelpers
    {
        public static List<List<T>> GetListsWithLeastElements<T>(this List<List<T>> lists)
        {
            var listCounts = lists.ConvertAll(l => l.Count);
            var minCount = listCounts.Min();
            return lists.Where(l => l.Count == minCount).ToList();
        }

        public static bool ContainsAll<T>(this IEnumerable<T> possibleContainer, IEnumerable<T> values)
        {
            foreach(var value in values)
            {
                if (!possibleContainer.Contains(value))
                    return false;
            }
            return true;
        }

        public static T? TakeRandomElement<T>(this IEnumerable<T> elementList, RngHandler rng)
        {
            return elementList.ToList()[rng.NextInclusive(elementList.Count() - 1)];
        }

        public static T? GetWithProbability<T>(this IEnumerable<T> elementList, Func<T, int> probability, RngHandler rng, int odds = 100)
        {
            int totalProbability = 0, currentProbability = 0;
            foreach (var element in elementList)
            {
                totalProbability += probability(element);
            }

            int random = rng.NextInclusive(1, odds);

            foreach (var element in elementList)
            {
                var maxProbability = currentProbability + probability(element);

                if (random.Between(currentProbability + 1, maxProbability))
                    return element;

                currentProbability = maxProbability;
            }

            return default;
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                yield return item;

                if (predicate(item))
                {
                    yield break;
                }
            }
        }

        public static List<T> Shuffle<T>(this List<T> list, Random rng)
        {
            var shuffledList = new List<T>(list);

            int n = shuffledList.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;
            }

            return shuffledList;
        }
    }
}
