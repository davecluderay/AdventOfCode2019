using System.Collections.Generic;

namespace Aoc2019_Day16
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> RepeatSequence<T>(this IEnumerable<T> sequence, int times)
        {
            while (times-- > 0)
                foreach (var item in sequence)
                    yield return item;
        }
    }
}