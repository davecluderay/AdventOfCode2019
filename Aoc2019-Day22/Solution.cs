using System.Linq;
using System.Numerics;

namespace Aoc2019_Day22
{
    internal class Solution
    {
        public string Title => "Day 22: Slam Shuffle";

        public object PartOne()
        {
            var deck = Enumerable.Range(0, 10007)
                                 .Select(x => (BigInteger)x)
                                 .ToArray();
            
            var shuffles = DealStrategyFactory.Create(InputFile.ReadAllLines());
            var combinedShuffle = DealStrategyFactory.Combine(shuffles, deck.Length);

            var result = new BigInteger[deck.Length];
            for (var sourcePosition = 0; sourcePosition < deck.Length; sourcePosition++)
            {
                var targetPosition = (int) Calculate.Modulo(combinedShuffle.a * sourcePosition + combinedShuffle.b,
                                               deck.Length);
                result[targetPosition] = deck[sourcePosition];
            }

            return result.Select((x, i) => (value: x, position: i))
                         .Single(x => x.value == 2019)
                         .position;
        }
        
        public object PartTwo()
        {
            return null;
        }
    }
}