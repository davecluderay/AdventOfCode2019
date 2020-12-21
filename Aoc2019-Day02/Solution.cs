using System.Linq;
using Aoc2019_Day02.Computer;

namespace Aoc2019_Day02
{
    internal class Solution
    {
        public string Title => "Day 2: 1202 Program Alarm";

        public object? PartOne()
        {
            return Execute(12, 2);
        }

        public object? PartTwo()
        {
            foreach (var noun in Enumerable.Range(0, 100))
            foreach (var verb in Enumerable.Range(0, 100))
                if (Execute(noun, verb) == 19690720)
                    return noun * 100 + verb;
            return null;
        }

        private long Execute(long noun, long verb)
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();

            computer.PokeMemory(1, noun);
            computer.PokeMemory(2, verb);

            computer.Run();

            return computer.ReadMemory(0, 1).Single();
        }
    }
}
