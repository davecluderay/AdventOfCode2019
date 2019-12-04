using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day04
{
    internal class Solution
    {
        private const int MinValue = 387638;
        private const int MaxValue = 919123;
        
        public string Title => "Day 4: Secure Container";

        public object PartOne()
        {
            var candidates = Enumerable.Range(MinValue, MaxValue - MinValue + 1).Select(x => x.ToString());
            var matches    = FindValidPasswords(candidates, requireExactDouble: false);
            return matches.Count();
        }
        
        public object PartTwo()
        {
            var candidates = Enumerable.Range(MinValue, MaxValue - MinValue + 1).Select(x => x.ToString());
            var matches = FindValidPasswords(candidates, requireExactDouble: true);
            return matches.Count();
        }

        private static IEnumerable<string> FindValidPasswords(IEnumerable<string> candidates, bool requireExactDouble)
        {
            foreach (var text in candidates)
            {
                var hasDouble = false;
                var hasDecrease = false;

                var index = 0;
                while (index < text.Length)
                {
                    var first = text[index];
                    
                    var repeatCount = 0;
                    while (++index < text.Length && text[index] == first)
                    {
                        repeatCount++;
                    }

                    if (repeatCount > 0 && (!requireExactDouble || repeatCount == 1))
                    {
                        hasDouble = true;
                    }

                    if (index < text.Length && text[index] < first)
                    {
                        hasDecrease = true;
                    }
                }

                if (hasDouble && !hasDecrease)
                {
                    yield return text;
                }
            }
        }
    }
}