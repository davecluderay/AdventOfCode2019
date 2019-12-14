using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2019_Day14
{
    internal class ChemicalReaction
    {
        public Chemical              Product  { get; }
        public IEnumerable<Chemical> Reagents { get; }

        public ChemicalReaction(Chemical product, IEnumerable<Chemical> reagents = null)
        {
            Product = product;
            Reagents = reagents ?? Enumerable.Empty<Chemical>();
        }
        
        public static ChemicalReaction Parse(string input)
        {
            var match = Pattern.Match(input);
            if (!match.Success) throw new ArgumentException($"Not recognised: {input}", nameof(input));
            
            var reagents = match.Groups["Reagent"].Captures.Select(c => Chemical.Parse(c.Value));
            var product  = Chemical.Parse(match.Groups["Product"].Value);

            return new ChemicalReaction(product, reagents);
        }
        
        private static readonly Regex Pattern = new Regex(@"^((?<Reagent>[^,=]+),?\s*)+=>\s*(?<Product>.*)$",
                                                          RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Singleline);
    }
}