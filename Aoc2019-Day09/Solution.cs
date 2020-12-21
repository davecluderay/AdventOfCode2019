using Aoc2019_Day09.Computer;

namespace Aoc2019_Day09
{
    internal class Solution
    {
        public string Title => "Day 9: Sensor Boost";

        public object? PartOne()
        {
            long? lastOutput = null;

            var computer = new IntCodeComputer();

            computer.InputFrom(1L);
            computer.OutputTo(output => lastOutput = output);

            computer.LoadProgram();
            computer.Run();

            return lastOutput;
        }

        public object? PartTwo()
        {
            long? lastOutput = null;

            var computer = new IntCodeComputer();

            computer.InputFrom(2L);
            computer.OutputTo(output => lastOutput = output);

            computer.LoadProgram();
            computer.Run();

            return lastOutput;
        }
    }
}
