using System.Collections.Generic;

namespace Aoc2019_Day18
{
    internal static class Combine
    {
        public static IEnumerable<(T, T)> IntoAllPossiblePairs<T>(IReadOnlyList<T> items)
        {
            for (var index0 = 0; index0 < items.Count - 1; index0++)
            for (var index1 = index0 + 1; index1 < items.Count; index1++)
                yield return (items[index0], items[index1]);
        }
    }
}