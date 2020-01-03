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

            return Calculate.Modulo(combinedShuffle.a * 2019 + combinedShuffle.b,
                                    deck.Length);
        }
        
        public object PartTwo()
        {
            var deckSize = (BigInteger) 119315717514047;
            var numberOfShuffles = (BigInteger) 101741582076661;
            
            var shuffles = DealStrategyFactory.Create(InputFile.ReadAllLines());
            var combinedShuffle = DealStrategyFactory.Combine(shuffles, deckSize);
            var megaShuffle = DealStrategyFactory.ApplyMultipleTimes(combinedShuffle, numberOfShuffles, deckSize);
            var sourcePositionFinder = DealStrategyFactory.GetSourcePositionFinder(megaShuffle, deckSize);

            return sourcePositionFinder(2020);
        }
    }
}