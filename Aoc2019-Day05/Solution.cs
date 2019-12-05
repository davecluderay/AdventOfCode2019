using System;
using System.Linq;

namespace Aoc2019_Day05
{
    internal class Solution
    {
        public string Title => "Day 5: Sunny with a Chance of Asteroids";

        public object PartOne()
        {
            var computer = new IntCodeComputer();
            
            computer.LoadProgram();
            
            computer.RunProgram(1);
            
            if (computer.GetIntermediateOutput().Any(x => x != 0)) throw new Exception("Some failures occurred.");
            
            return computer.GetLastOutput();
        }
        
        public object PartTwo()
        {
            var computer = new IntCodeComputer();

            computer.LoadProgram();
            computer.RunProgram(5);
            return computer.GetLastOutput();
        }
    }
}