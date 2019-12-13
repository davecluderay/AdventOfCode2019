using System;
using System.Linq;

namespace Aoc2019_Day12
{
    internal static class Calculate
    {
        public static long LowestCommonMultiple(long[] numbers)
        {
            return numbers.Aggregate(LowestCommonMultiple);
        }
        
        public static long LowestCommonMultiple(long first, long second)
        {
            return Math.Abs(first * second) / GreatestCommonDivisor(first, second);
        }
        
        public static long GreatestCommonDivisor(long first, long second)
        {
            return second == 0 ? first : GreatestCommonDivisor(second, first % second);
        }
        
    }
}