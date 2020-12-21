using System;

namespace Aoc2019_Day07.Computer
{
    internal class IntCodeExecutionContext
    {
        private Func<long> _readInput = DefaultReadInput;
        private Action<long> _writeOutput = DefaultWriteOutput;
        private long[] _memory = Array.Empty<long>();

        public long InstructionPointer { get; set; }
        public bool IsHalted => ReadMemory(InstructionPointer) % 100 == Halt.Opcode;

        public long ReadMemory(long address)
        {
            return _memory[address];
        }

        public long[] ReadMemory(long address, int count)
        {
            var array = new long[count];
            for (var index = 0; index < count; index++)
                array[index] = _memory[address + index];
            return array;
        }

        public void WriteMemory(long address, params long[] data)
        {
            if (_memory.Length < address + data.Length)
            {
                var newMemory = new long[address + data.Length];
                Array.Copy(_memory, newMemory, _memory.Length);
                _memory = newMemory;
            }

            for (var index = 0; index < data.Length; index++)
                _memory[address + index] = data[index];
        }

        public long ReadInput()
        {
            return _readInput();
        }

        public void WriteOutput(long value)
        {
            _writeOutput(value);
        }

        public long ReadParameterValue(IntCodeInstructionParameter parameter)
        {
            return parameter.IsImmediateMode
                       ? parameter.Value
                       : ReadMemory(parameter.Value);
        }

        public void SetInputReader(Func<long> function)
            => _readInput = function;

        public void SetOutputWriter(Action<long> action)
            => _writeOutput = action;

        public static long DefaultReadInput()
            => Convert.ToInt64(Console.ReadLine());

        public static void DefaultWriteOutput(long value)
            => Console.WriteLine(value);
    }
}
