namespace Aoc2019_Day02
{
    internal class Solution
    {
        public string Title => "Day 2: 1202 Program Alarm";

        public object PartOne()
        {
            var computer = new IntCodeComputer();

            computer.LoadProgram();
            
            computer.EnterInputs(12, 2);

            computer.RunProgram();
            
            return computer.GetResult();
        }

        public object PartTwo()
        {
            var computer = new IntCodeComputer();
            
            for (var noun = 0; noun < 100; noun++)
            for (var verb = 0; verb < 100; verb++)
            {
                computer.LoadProgram();

                computer.EnterInputs(noun, verb);
                
                computer.RunProgram();

                if (computer.GetResult() == 19690720)
                    return noun * 100 + verb;
            }

            return null;
        }
    }
}