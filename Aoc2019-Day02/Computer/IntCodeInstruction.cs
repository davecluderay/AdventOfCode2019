using System;

namespace Aoc2019_Day02.Computer
{
    internal abstract class IntCodeInstruction
    {
        public abstract int Length { get; }
        public abstract void Execute(long[] memory);
    }

    internal sealed class Add : IntCodeInstruction
    {
        public const long Opcode = 1;

        private readonly long[] _parameters;
        private long Operand1 => _parameters[0];
        private long Operand2 => _parameters[1];
        private long Operand3 => _parameters[2];

        private Add(long[] parameters) => _parameters = parameters;

        public override int Length => 4;

        public override void Execute(long[] memory)
            => memory[Operand3] = memory[Operand1] + memory[Operand2];

        public static Add Read(long[] memory, int at)
            => new Add(memory.AsSpan(at + 1, 3).ToArray());
    }

    internal sealed class Multiply : IntCodeInstruction
    {
        public const long Opcode = 2;

        private readonly long[] _parameters;
        private long Operand1 => _parameters[0];
        private long Operand2 => _parameters[1];
        private long Operand3 => _parameters[2];

        private Multiply(long[] parameters) => _parameters = parameters;

        public override int Length => 4;

        public override void Execute(long[] memory)
            => memory[Operand3] = memory[Operand1] * memory[Operand2];

        public static Multiply Read(long[] memory, int at)
            => new Multiply(memory.AsSpan(at + 1, 3).ToArray());
    }

    internal sealed class Halt : IntCodeInstruction
    {
        public const long Opcode = 99;
        public override int Length => 1;

        public override void Execute(long[] memory) { }

        public static Halt Read(long[] memory, int at) => new Halt();
    }
}
