using System;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Aoc2019_Day22
{
    internal static class DealStrategyFactory
    {
        private static readonly Regex DealIntoNewStackPattern = new Regex("^deal into new stack$",
                                                                          RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex CutPattern = new Regex(@"^cut (?<N>-?\d+)$",
                                                             RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        private static readonly Regex DealWithIncrementPattern = new Regex(@"^deal with increment (?<N>\d+)$",
                                                                           RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static (BigInteger a, BigInteger b) Combine((BigInteger a, BigInteger b)[] coefficients, BigInteger modulus)
        {
            var combined = (a: (BigInteger) 1, b: (BigInteger) 0); 
            foreach (var c in coefficients)
            {
                // f(x) => px + q            (mod m)
                // g(x) => rx + s            (mod m)
                // g(f(x)) => r(px + q) + s  (mod m)
                //            rpx + rq + s   (mod m)
                combined = (Calculate.Modulo(c.a * combined.a, modulus), 
                            Calculate.Modulo(c.a * combined.b + c.b, modulus));
            }

            return combined;
        }
        
        public static (BigInteger a, BigInteger b)[] Create(params string[] lines)
        {
            return lines.Select(Create)
                        .ToArray();
        }

        private static (BigInteger a, BigInteger b) Create(string line)
        {
            var match = DealIntoNewStackPattern.Match(line);
            if (match.Success)
            {
                // f(x) = -x - 1 (mod m)
                return (-1, -1);
            }

            match = CutPattern.Match(line);
            if (match.Success)
            {
                // f(x) = x - n (mod m)
                var n = Convert.ToInt32(match.Groups["N"].Value);
                return (1, -n);
            }

            match = DealWithIncrementPattern.Match(line);
            if (match.Success)
            {
                // f(x) = nx (mod m)
                var increment = Convert.ToInt32(match.Groups["N"].Value);
                return (increment, 0);
            }

            throw new InvalidOperationException($"Unrecognised line: {line}");
        }
    }
}