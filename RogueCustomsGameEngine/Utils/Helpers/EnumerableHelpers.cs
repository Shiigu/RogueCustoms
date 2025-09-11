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
            if (!elementList.Any()) return default;
            return elementList.ToList()[rng.NextInclusive(elementList.Count() - 1)];
        }

        public static List<T> TakeNDifferentRandomElements<T>(this IEnumerable<T> elementList, int amount, RngHandler rng)
        {
            if (amount <= 0) return new();

            var list = elementList.ToList();
            if (amount >= list.Count) return list;

            var selectedElements = new HashSet<int>();
            var result = new List<T>();

            while (selectedElements.Count < amount)
            {
                var randomIndex = rng.NextInclusive(0, list.Count - 1);
                if (selectedElements.Add(randomIndex))
                {
                    result.Add(list[randomIndex]);
                }
            }

            return result;
        }

        public static T? TakeRandomElement<T>(this T[][] jaggedArray, RngHandler rng)
        {
            if (jaggedArray == null || jaggedArray.Length == 0)
                return default;

            var randomInnerArray = jaggedArray[rng.NextInclusive(jaggedArray.Length - 1)];

            if (randomInnerArray.Length == 0)
                return default;

            return randomInnerArray[rng.NextInclusive(randomInnerArray.Length - 1)];
        }
        public static List<T> TakeNDifferentRandomElements<T>(this T[][] jaggedArray, int amount, RngHandler rng)
        {
            var arrayAsList = jaggedArray.SelectMany(innerArray => innerArray).ToList();
            if (amount >= arrayAsList.Count) return arrayAsList;

            var shuffledList = arrayAsList.OrderBy(x => rng.NextInclusive(int.MaxValue)).ToList();

            return shuffledList.Take(amount).ToList();
        }

        public static T? TakeRandomElement<T>(this T[,] multiArray, RngHandler rng)
        {
            var rows = multiArray.GetLength(0);
            var cols = multiArray.GetLength(1);

            if (rows == 0 || cols == 0)
                return default;

            var randomRow = rng.NextInclusive(rows - 1);
            var randomCol = rng.NextInclusive(cols - 1);

            return multiArray[randomRow, randomCol];
        }
        public static List<T> TakeNDifferentRandomElements<T>(this T[,] multiArray, int amount, RngHandler rng)
        {
            var arrayAsList = new List<T>();
            for (int i = 0; i < multiArray.GetLength(0); i++)
            {
                for (int j = 0; j < multiArray.GetLength(1); j++)
                {
                    arrayAsList.Add(multiArray[i, j]);
                }
            }

            if (amount >= arrayAsList.Count) return arrayAsList; 

            var shuffledList = arrayAsList.OrderBy(x => rng.NextInclusive(int.MaxValue)).ToList();

            return shuffledList.Take(amount).ToList();
        }

        public static T? TakeRandomElementWithWeights<T>(this IEnumerable<T> elementList, Func<T, int> weight, RngHandler rng)
        {
            var elements = elementList.ToArray();
            int totalWeight = elements.Sum(weight);

            if (totalWeight <= 0)
                return default;

            int randomValue = rng.NextInclusive(1, totalWeight);

            int cumulativeWeight = 0;

            foreach (var element in elements)
            {
                cumulativeWeight += weight(element);

                if (randomValue <= cumulativeWeight)
                {
                    return element;
                }
            }

            // Return null as a fallback (though this should not happen if weights are valid)
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

        public static List<T> Shuffle<T>(this List<T> list, RngHandler rng)
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

        public static void AddButKeepingCapacity<T>(this List<T> list, T newItem, int maxSize)
        {
            if (list.Count >= maxSize)
            {
                list.RemoveAt(0); // Remove the oldest (first) element
            }

            list.Add(newItem); // Add the new element
        }
    }
}
