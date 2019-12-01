using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day01
{
    public class Solution
    {
        public string Title => "Day 1: The Tyranny of the Rocket Equation";
        public object PartOne()
        {
            return ReadModuleMasses().Select(CalculateFuelRequired)
                                     .Sum();
        }

        public object PartTwo()
        {
            var totalFuel = 0;
            foreach (var moduleMass in ReadModuleMasses())
            {
                var fuelAmount = CalculateFuelRequired(moduleMass);
                while (fuelAmount > 0)
                {
                    totalFuel += fuelAmount;
                    fuelAmount = CalculateFuelRequired(fuelAmount);
                }
            }
            return totalFuel;
        }

        private static IEnumerable<int> ReadModuleMasses()
        {
            return InputFile.ReadAllLines()
                            .Select(l => Convert.ToInt32(l))
                            .ToList();
        }

        private int CalculateFuelRequired(int mass) => Math.Max(0, mass / 3 - 2);
    }
}