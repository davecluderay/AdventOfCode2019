using System;

namespace Aoc2019_Day15.Computer
{
    internal class IntCodeExecutionContext
    {
        private Func<long> _readInput = DefaultReadInput;
        private Action<long> _writeOutput = DefaultWriteOutput;
        private readonly IntCodeMemory _memory = new IntCodeMemory();

        public long InstructionPointer { get; set; }
        public long RelativeBase { get; set; }
        public bool IsHalted => ReadMemory(InstructionPointer) % 100 == Halt.Opcode;

        public long ReadMemory(long address)
        {
            return _memory.GetAt(address);
        }

        public long[] ReadMemory(long address, int count)
        {
            var array = new long[count];
            for (var index = 0; index < count; index++)
                array[index] = _memory.GetAt(address + index);
            return array;
        }

        public void WriteMemory(long address, params long[] data)
        {
            for (var index = 0; index < data.Length; index++)
                _memory.SetAt(address + index, data[index]);
        }

        public long ReadInput()
        {
            return _readInput();
        }

        public void WriteOutput(long value)
        {
            _writeOutput(value);
        }

        public long ReadParameterAddress(IntCodeInstructionParameter parameter)
        {
            if (parameter.IsImmediateMode) throw new NotSupportedException("Address parameters do not support immediate mode.");
            return parameter.Value + (parameter.IsRelativeMode ? RelativeBase : 0L);
        }

        public long ReadParameterValue(IntCodeInstructionParameter parameter)
        {
            if (parameter.IsImmediateMode) return parameter.Value;
            return ReadMemory(parameter.Value + (parameter.IsRelativeMode ? RelativeBase : 0L));
        }

        public void SetInputReader(Func<long> function)
            => _readInput = function;

        public void SetOutputWriter(Action<long> action)
            => _writeOutput = action;

        public static long DefaultReadInput()
            => Convert.ToInt64(Console.ReadLine());

        public static void DefaultWriteOutput(long value)
            => Console.WriteLine(value);

        public IntCodeDebugSnapshot TakeSnapshot()
        {
            return new IntCodeDebugSnapshot(InstructionPointer,
                                            _memory.Copy(),
                                            RelativeBase);
        }

        public void ApplySnapshot(IntCodeDebugSnapshot snapshot)
        {
            InstructionPointer = snapshot.InstructionPointer;
            RelativeBase = snapshot.RelativeBase;
            _memory.Replace(snapshot.Memory);
        }
    }
}
