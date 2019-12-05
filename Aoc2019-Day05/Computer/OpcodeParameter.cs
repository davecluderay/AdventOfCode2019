using System;

namespace Aoc2019_Day05
{
    internal class OpcodeParameter
    {
        public OpcodeParameterMode Mode { get; }
        public int RawValue { get; }

        private OpcodeParameter(OpcodeParameterMode mode, int rawValue)
        {
            Mode = mode;
            RawValue = rawValue;
        }

        public int DereferencedValue(int[] memory)
        {
            return Mode == OpcodeParameterMode.Position
                ? memory[RawValue]
                : RawValue;
        }

        public static OpcodeParameter[] ReadParameters(int[] memory, int instructionPointer, int count)
        {
            var result = new OpcodeParameter[count];
            for (var index = 0; index < count; index++)
            {
                var value = memory[instructionPointer + index + 1];
                var mode = (OpcodeParameterMode)(memory[instructionPointer] / (int)Math.Pow(10, 2 + index) % 10);
                result[index] = new OpcodeParameter(mode, value);
            }

            return result;
        }
    }
}