using System;
using System.Text;
using Aoc2019_Day25.Computer;

namespace Aoc2019_Day25
{
    internal class Solution
    {
        public string Title => "Day 25: Cryostasis";

        public object PartOne()
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();

            var commands = new[]
                           {
                               "east",
                               "north",
                               "east",
                               "north",
                               "north",
                               "west",
                               "take asterisk",
                               "east",
                               "south",
                               "east",
                               "take sand",
                               "south",
                               "west",
                               "take prime number",
                               "east",
                               "north",
                               "east",
                               "south",
                               "take tambourine",
                               "west",
                               "north",
                               "west"
                           };
            foreach (var output in computer.RunProgram(new AutoplayAdapter(commands).GetNextInput))
            {
                Console.Write(Encoding.ASCII.GetString(new[] { (byte) output }));
            }

            return "2228740";
        }
        
        public object PartTwo()
        {
            return "That's it!";
        }
    }
}