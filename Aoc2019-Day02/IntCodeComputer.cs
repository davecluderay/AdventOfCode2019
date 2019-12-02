using System;
using System.Linq;

namespace Aoc2019_Day02
{
    internal class IntCodeComputer
    {
        private int[] _memory = Array.Empty<int>();

        public void LoadProgram(string fileName = null)
        {
            _memory = InputFile.ReadAllLines()
                .Single()
                .Split(',')
                .Select(int.Parse)
                .ToArray();
        }

        public void EnterInputs(int noun, int verb)
        {
            _memory[1] = noun;
            _memory[2] = verb;
        }

        public int GetResult() => _memory[0];

        public void RunProgram()
        {
            var instructionPointer = 0;
            var running = true;
            while (running)
            {
                var opcode = _memory[instructionPointer];
                switch (opcode)
                {
                    case Opcodes.Add:
                    {
                        var (address1, address2, address3) = (_memory[instructionPointer + 1], _memory[instructionPointer + 2], _memory[instructionPointer + 3]);
                        _memory[address3] = _memory[address1] + _memory[address2];
                        instructionPointer += 4;
                    }
                        break;
                    case Opcodes.Multiply:
                    {
                        var (address1, address2, address3) = (_memory[instructionPointer + 1], _memory[instructionPointer + 2], _memory[instructionPointer + 3]);
                        _memory[address3] = _memory[address1] * _memory[address2];
                        instructionPointer += 4;
                        break;
                    }
                    case Opcodes.Halt:
                    {
                        running = false;
                        instructionPointer += 1;
                        break;
                    }
                    default:
                        throw new Exception($"Unrecognised opcode: {opcode}");
                }
            }
        }

        private static class Opcodes
        {
            public const int Add = 1;
            public const int Multiply = 2;
            public const int Halt = 99;
        }
    }
}