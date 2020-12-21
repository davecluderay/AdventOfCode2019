using Aoc2019_Day05.Computer;

namespace Aoc2019_Day05
{
    internal class Solution
    {
        public string Title => "Day 5: Sunny with a Chance of Asteroids";

        public object? PartOne()
            => RunWithInput(1L);

        public object? PartTwo()
            => RunWithInput(5L);

        private static long? RunWithInput(long input, string? fileName = null)
        {
            long? lastOutput = null;

            var computer = new IntCodeComputer();
            computer.InputFrom(input);
            computer.OutputTo(output => lastOutput = output);

            computer.LoadProgram(fileName);
            computer.Run();

            return lastOutput;
        }
    }
}
