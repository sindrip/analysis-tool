using System;
using System.Collections.Generic;
using System.Linq;

namespace Analysis
{
    public static class IEnumerableExt
    {
        public static IEnumerable<T> Singleton<T>(this T element)
        {
            return Enumerable.Repeat<T>(element, 1);
        }
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list, int? seed = 0)
        {
            var r = new Random(seed ?? 0);
            var shuffledList =
                list.
                    Select(x => new { Number = r.Next(), Item = x }).
                    OrderBy(x => x.Number).
                    Select(x => x.Item).
                    Take(list.Count()); // Assume first @size items is fine

            return shuffledList.ToList();
        }
    }
}