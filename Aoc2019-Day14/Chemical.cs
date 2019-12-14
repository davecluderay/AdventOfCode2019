using System;
using System.Text.RegularExpressions;

namespace Aoc2019_Day14
{
    internal class Chemical
    {
        public string Name     { get; }
        public long   Quantity { get; }

        public Chemical(string name, in long quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public static Chemical Parse(string input)
        {
            var match = Pattern.Match(input);
            if (!match.Success) throw new ArgumentException($"Not recognised: {input}", nameof(input));
            
            var name     = match.Groups["Name"].Value;
            var quantity = Convert.ToInt32(match.Groups["Quantity"].Value);
            
            return new Chemical(name, quantity);
        }
        
        private static readonly Regex Pattern = new Regex(@"^\s*(?<Quantity>\d+)\s*(?<Name>\S+)\s*$",
                                                          RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    }
}