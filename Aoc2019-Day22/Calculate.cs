using System.Numerics;

namespace Aoc2019_Day22
{
    internal static class Calculate
    {
        public static BigInteger Modulo(BigInteger a, BigInteger m)
        {
            var result = a % m;
            return result < 0 ? result + m : result;
        }
    }
}