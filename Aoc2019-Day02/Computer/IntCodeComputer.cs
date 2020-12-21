using System;
using System.Linq;

namespace Aoc2019_Day02.Computer
{
    internal class IntCodeComputer
    {
        private int _instructionPointer = 0;
        private long[] _memory = Array.Empty<long>();

        public void Run()
        {
            while (true)
            {
                if (!Step()) break;
            }
        }

        public bool Step()
        {
            var instruction = ReadInstruction(_memory, _instructionPointer);
            instruction.Execute(_memory);
            _instructionPointer = _instructionPointer + instruction.Length;
            return !(instruction is Halt);
        }

        public void PokeMemory(long position, long data)
        {
            _memory[position] = data;
        }

        public long[] ReadMemory() => _memory;

        public long[] ReadMemory(int position, int count)
        {
            if (count == 0) return Array.Empty<long>();
            return _memory.AsSpan(position, count)
                          .ToArray();
        }

        public void LoadProgram(string? fileName = null)
        {
            _memory = InputFile.ReadAllText(fileName)
                               .Split(',')
                               .Select(long.Parse)
                               .ToArray();
            _instructionPointer = 0;
        }

        private static IntCodeInstruction ReadInstruction(long[] memory, int at)
            => memory[at] switch
               {
                   Add.Opcode      => Add.Read(memory, at),
                   Multiply.Opcode => Multiply.Read(memory, at),
                   Halt.Opcode     => Halt.Read(memory, at),
                   _               => throw new NotSupportedException()
               };
    }
}
