using System;
using System.Linq;
using System.Text;

namespace Aoc2019_Day07.Computer
{
    internal class IntCodeInstructionParameter
    {
        public long Value { get; }
        public bool IsImmediateMode { get; }

        public IntCodeInstructionParameter(long value, bool isImmediateMode)
            => (Value, IsImmediateMode) = (value, isImmediateMode);
    }

    internal abstract class IntCodeInstruction
    {
        private readonly string _description;
        protected IntCodeInstructionParameter[] Parameters { get; }
        private int Length => Parameters.Length + 1;

        protected IntCodeInstruction(IntCodeExecutionContext context, int parameterCount)
        {
            var opcode = context.ReadMemory(context.InstructionPointer);
            var parameterValues = context.ReadMemory(context.InstructionPointer + 1, parameterCount);
            Parameters = new IntCodeInstructionParameter[parameterCount];
            for (var i = 0; i < parameterValues.Length; i++)
                Parameters[i] = new IntCodeInstructionParameter(parameterValues[i], opcode / (int)Math.Pow(10, i + 2) % 10 == 1);

            _description = new StringBuilder().Append($"@{context.InstructionPointer:D16}  ")
                                              .Append($"{GetType().Name.ToUpper()}: ")
                                              .Append($"{opcode} (")
                                              .Append(string.Join(", ", parameterValues))
                                              .Append(")")
                                              .ToString();
        }

        protected void MoveToNextInstruction(IntCodeExecutionContext context)
        {
            context.InstructionPointer += Length;
        }

        public override string ToString() => _description;

        public abstract void Execute(IntCodeExecutionContext context);
    }

    internal sealed class Add : IntCodeInstruction
    {
        public const long Opcode = 1;

        public Add(IntCodeExecutionContext context) : base(context, parameterCount: 3) {}

        public override void Execute(IntCodeExecutionContext context)
        {
            var operand1 = context.ReadParameterValue(Parameters[0]);
            var operand2 = context.ReadParameterValue(Parameters[1]);
            var address = Parameters[2].Value;
            context.WriteMemory(address, operand1 + operand2);
            MoveToNextInstruction(context);
        }
    }

    internal sealed class Multiply : IntCodeInstruction
    {
        public const long Opcode = 2;

        public Multiply(IntCodeExecutionContext context) : base(context, parameterCount: 3) {}

        public override void Execute(IntCodeExecutionContext context)
        {
            var operand1 = context.ReadParameterValue(Parameters[0]);
            var operand2 = context.ReadParameterValue(Parameters[1]);
            var address = Parameters[2].Value;
            context.WriteMemory(address, operand1 * operand2);
            MoveToNextInstruction(context);
        }
    }

    internal sealed class Input : IntCodeInstruction
    {
        public const long Opcode = 3;

        public Input(IntCodeExecutionContext context) : base(context, parameterCount: 1) {}

        public override void Execute(IntCodeExecutionContext context)
        {
            var address = Parameters.Single().Value;
            context.WriteMemory(address, context.ReadInput());
            MoveToNextInstruction(context);
        }
    }

    internal sealed class Output : IntCodeInstruction
    {
        public const long Opcode = 4;

        public Output(IntCodeExecutionContext context) : base(context, parameterCount: 1) {}

        public override void Execute(IntCodeExecutionContext context)
        {
            var value = context.ReadParameterValue(Parameters.Single());
            context.WriteOutput(value);
            MoveToNextInstruction(context);
        }
    }

    internal sealed class JumpIfTrue : IntCodeInstruction
    {
        public const long Opcode = 5;

        public JumpIfTrue(IntCodeExecutionContext context) : base(context, parameterCount: 2) {}

        public override void Execute(IntCodeExecutionContext context)
        {
            var value = context.ReadParameterValue(Parameters[0]);
            var address = context.ReadParameterValue(Parameters[1]);
            if (value != 0)
                context.InstructionPointer = address;
            else
                MoveToNextInstruction(context);
        }
    }

    internal sealed class JumpIfFalse : IntCodeInstruction
    {
        public const long Opcode = 6;

        public JumpIfFalse(IntCodeExecutionContext context) : base(context, parameterCount: 2) {}

        public override void Execute(IntCodeExecutionContext context)
        {
            var value = context.ReadParameterValue(Parameters[0]);
            var address = context.ReadParameterValue(Parameters[1]);
            if (value == 0)
                context.InstructionPointer = address;
            else
                MoveToNextInstruction(context);
        }
    }

    internal sealed class LessThan : IntCodeInstruction
    {
        public const long Opcode = 7;

        public LessThan(IntCodeExecutionContext context) : base(context, parameterCount: 3) {}

        public override void Execute(IntCodeExecutionContext context)
        {
            var operand1 = context.ReadParameterValue(Parameters[0]);
            var operand2 = context.ReadParameterValue(Parameters[1]);
            var address = Parameters[2].Value;
            context.WriteMemory(address, operand1 < operand2 ? 1L : 0L);
            MoveToNextInstruction(context);
        }
    }

    internal sealed class EqualTo : IntCodeInstruction
    {
        public const long Opcode = 8;

        public EqualTo(IntCodeExecutionContext context) : base(context, parameterCount: 3) {}

        public override void Execute(IntCodeExecutionContext context)
        {
            var operand1 = context.ReadParameterValue(Parameters[0]);
            var operand2 = context.ReadParameterValue(Parameters[1]);
            var address = Parameters[2].Value;
            context.WriteMemory(address, operand1 == operand2 ? 1L : 0L);
            MoveToNextInstruction(context);
        }
    }

    internal sealed class Halt : IntCodeInstruction
    {
        public const long Opcode = 99;

        public Halt(IntCodeExecutionContext context) : base(context, parameterCount: 0) {}

        public override void Execute(IntCodeExecutionContext context) { }
    }
}
