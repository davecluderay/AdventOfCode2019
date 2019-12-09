using System;
using System.Linq;
using Aoc2019_Day09.Computer;

namespace Aoc2019_Day09
{
    internal class Solution
    {
        public string Title => "Day 9: Sensor Boost";

        public object PartOne()
        {
            var computer = new IntCodeComputer(new DebugOutput { WriteToConsole = false });
            computer.LoadProgram();
            return computer.RunProgram(1)
                           .Last();
        }
        
        public object PartTwo()
        {
            var computer = new IntCodeComputer(new DebugOutput { WriteToConsole = false });
            computer.LoadProgram();
            return computer.RunProgram(2)
                           .Last();
        }
    }
}