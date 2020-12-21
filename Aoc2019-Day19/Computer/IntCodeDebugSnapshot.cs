using System.Collections.Generic;

namespace Aoc2019_Day19.Computer
{
    internal class IntCodeDebugSnapshot
    {
        public long InstructionPointer { get; }
        public IDictionary<long, long> Memory { get; }
        public long RelativeBase { get; }

        public IntCodeDebugSnapshot(long instructionPointer, IDictionary<long, long> memory, long relativeBase)
        {
            InstructionPointer = instructionPointer;
            Memory = memory;
            RelativeBase = relativeBase;
        }
    }
}
