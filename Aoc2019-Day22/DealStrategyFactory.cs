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

        public static (BigInteger a, BigInteger b) ApplyMultipleTimes((BigInteger a, BigInteger b) coefficients, BigInteger numberOfIterations, BigInteger deckSize)
        {
            // When applying the same function k times, the result is itself a linear function:
            //   f(x    = ax         + b
            //   ff(x)  = aax        + ab + b
            //   fff(x) = aaax       + aab + ab + b
            // For k iterations:
            //   f(^k)(x)  = a(^k)x  + a(^k-1)b + a(^k-2)b .. + aab + ab + b
            //
            // The increment part is a^k.
            // The offset part is a geometric series (see https://en.wikipedia.org/wiki/Geometric_progression#Geometric_series), so uses formula:
            //   b(1 - a(^k)) / (1 - a)
            //
            // This is modular arithmetic, so the final shuffle function is:
            //
            // f(x) = a(^k)x + b(1 - a(^k)) / (1 - a)  (mod m)
            //
            // (where k = the number of shuffles and m = the deck size)
            var increment = BigInteger.ModPow(coefficients.a, numberOfIterations, deckSize);
            var offset = Calculate.Modulo(coefficients.b * (1 - BigInteger.ModPow(coefficients.a, numberOfIterations, deckSize)),
                                          deckSize) *
                         Calculate.Modulo(BigInteger.ModPow(1 - coefficients.a,
                                                   deckSize - 2,
                                                   deckSize),
                                          deckSize);
            var result = (a: increment,
                          b: Calculate.Modulo(offset, deckSize));
            return result;
        }

        public static Func<BigInteger, BigInteger> GetSourcePositionFinder((BigInteger a, BigInteger b) coefficients, BigInteger deckSize)
        {
            // Forwards:
            //   f(x) = ax + b (mod m)
            // Rearranging:
            //   x = (x' - b) / a (mod m)
            // And it is modular arithmetic (with relative primes), so:
            //  x = (x' - b) mod m * a(^m-2) mod m
            return finalPosition => Calculate.Modulo(Calculate.Modulo(finalPosition - coefficients.b, deckSize) *
                                                     BigInteger.ModPow(coefficients.a, deckSize - 2, deckSize),
                                                     deckSize);
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