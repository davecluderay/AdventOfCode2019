using System.Linq;

namespace Aoc2019_Day14
{
    internal class Solution
    {
        public string Title => "Day 14: Space Stoichiometry";

        public object PartOne()
        {
            var reactions = LoadChemicalReactions();
            var reactors = reactions.Select(reaction => new ChemicalReactor(reaction))
                                    .ToArray();

            reactors.Single(r => r.ProductName == "FUEL")
                    .Produce(reactors, 1);
            return reactors.Single(r => r.ProductName == "ORE")
                           .TotalQuantityProduced;
        }

        public object PartTwo()
        {
            const long availableOreQuantity = 1000000000000L;
            var        maxSuccessQuantity   = 0L;
            var        minFailureQuantity   = availableOreQuantity + 1;

            var reactions = LoadChemicalReactions();
            while (minFailureQuantity - maxSuccessQuantity != 1)
            {
                var nextTest = (maxSuccessQuantity - minFailureQuantity) / 2 + minFailureQuantity;

                var reactors = reactions.Select(reaction => new ChemicalReactor(reaction))
                                        .ToArray();

                var fuel = reactors.Single(r => r.ProductName == "FUEL");
                fuel.Produce(reactors, nextTest);

                var ore = reactors.Single(r => r.ProductName == "ORE");
                if (ore.TotalQuantityProduced <= availableOreQuantity)
                    maxSuccessQuantity = nextTest;
                else
                    minFailureQuantity = nextTest;
            }

            return maxSuccessQuantity;
        }

        private ChemicalReaction[] LoadChemicalReactions(string? fileName = null)
        {
            return InputFile.ReadAllLines(fileName)
                                     .Select(ChemicalReaction.Parse)
                                     .Concat(new[] { new ChemicalReaction(new Chemical("ORE", 1)) })
                                     .ToArray();
        }
    }
}
