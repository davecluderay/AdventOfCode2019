using System;

namespace Aoc2019_Day13.Computer
{
    internal class OpcodeParameter
    {
        public OpcodeParameterMode Mode { get; }
        public long RawValue { get; }

        private OpcodeParameter(OpcodeParameterMode mode, long rawValue)
        {
            Mode = mode;
            RawValue = rawValue;
        }

        public long DereferencedValue(IntCodeMemory memory, long relativeBase)
        {
            switch (Mode)
            {
                case OpcodeParameterMode.Position:
                    return memory.GetAt(RawValue);
                case OpcodeParameterMode.Immediate:
                    return RawValue;
                case OpcodeParameterMode.Relative:
                    return memory.GetAt(RawValue + relativeBase);
                default:
                    throw new Exception($"Unrecognised parameter mode: {Mode}");
            }
        }

        public long Value(long relativeBase)
        {
            switch (Mode)
            {
                case OpcodeParameterMode.Position:
                    return RawValue;
                case OpcodeParameterMode.Immediate:
                    return RawValue;
                case OpcodeParameterMode.Relative:
                    return RawValue + relativeBase;
                default:
                    throw new Exception($"Unrecognised parameter mode: {Mode}");
            }
        }

        public static OpcodeParameter[] ReadParameters(IntCodeMemory memory, long instructionPointer, int count)
        {
            var result = new OpcodeParameter[count];
            for (var index = 0; index < count; index++)
            {
                var value = memory.GetAt(instructionPointer + index + 1);
                var mode = (OpcodeParameterMode)(memory.GetAt(instructionPointer) / (int)Math.Pow(10, 2 + index) % 10);
                result[index] = new OpcodeParameter(mode, value);
            }

            return result;
        }
    }
}