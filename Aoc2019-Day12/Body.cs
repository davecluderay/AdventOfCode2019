using System;
using System.Text.RegularExpressions;

namespace Aoc2019_Day12
{
    internal class Body
    {
        public (int x, int y, int z) Position;
        public (int x, int y, int z) Velocity;

        public override string ToString()
        {
            return $"pos=<{Position}>, vel=<{Velocity}>";
        }

        public static Body Parse(string text)
        {
            var matches = ComponentPattern.Matches(text);
            if (matches.Count != 3) throw new ArgumentException($"Couldn't read position from input: {text}", nameof(text));

            var position = (x: 0, y: 0, z: 0);
            for (var index = 0; index < matches.Count; index++)
            {
                var axis  = matches[index].Groups["Axis"].Value;
                var value = Convert.ToInt32(matches[index].Groups["Value"].Value);
                    
                switch (axis)
                {
                    case "x":
                        position.x = value;
                        break;
                    case "y":
                        position.y = value;
                        break;
                    case "z":
                        position.z = value;
                        break;
                }
            }
                
            return new Body { Position = position };
        }

        private static readonly Regex ComponentPattern = new Regex(@"\b(?<Axis>[xyz])=(?<Value>-?\d+)\b", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
    }
}