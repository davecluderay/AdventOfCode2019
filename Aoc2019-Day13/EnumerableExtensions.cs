using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day13
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T[]> InBatchesOf<T>(this IEnumerable<T> sequence, int size)
        {
            var batch = new List<T>(size);
            foreach (var item in sequence)
            {
                if (batch.Count == size)
                {
                    yield return batch.ToArray();
                    batch.Clear();
                }
                
                batch.Add(item);
            }

            if (batch.Any())
            {
                yield return batch.ToArray();
            }
        }
    }
}