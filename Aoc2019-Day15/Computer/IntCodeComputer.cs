using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day15.Computer
{
    internal class IntCodeComputer
    {
        private readonly IntCodeExecutionContext _context = new IntCodeExecutionContext();

        public bool IsHalted => _context.IsHalted;

        // ReSharper disable once EventNeverSubscribedTo.Global
        public event Action<string> Trace = delegate {};

        public void LoadProgram(string? fileName = null)
        {
            _context.WriteMemory(0L, InputFile.ReadAllText(fileName)
                                              .Split(',')
                                              .Select(long.Parse)
                                              .ToArray());
            _context.InstructionPointer = 0;
        }

        public void Run()
        {
            while (!IsHalted)
            {
                Step();
            }
        }

        public void InputFrom(params long[] inputs)
            => InputFrom(new Queue<long>(inputs));

        public void InputFrom(Queue<long> queue)
            => InputFrom(queue.Dequeue);

        public void InputFrom(Func<long> function)
            => _context.SetInputReader(function);

        public void OutputTo(Queue<long> queue)
            => OutputTo(queue.Enqueue);

        public void OutputTo(Action<long> action)
            => _context.SetOutputWriter(action);

        public void PokeMemory(long address, long value)
            => _context.WriteMemory(address, value);

        public void Step()
        {
            var instruction = ReadInstruction(_context);
            Trace(instruction.ToString());
            instruction.Execute(_context);
        }

        public IntCodeDebugSnapshot TakeSnapshot()
            => _context.TakeSnapshot();

        public void ApplySnapshot(IntCodeDebugSnapshot snapshot)
            => _context.ApplySnapshot(snapshot);

        private static IntCodeInstruction ReadInstruction(IntCodeExecutionContext context)
            => (context.ReadMemory(context.InstructionPointer) % 100)
                switch
                {
                    Add.Opcode => new Add(context),
                    Multiply.Opcode => new Multiply(context),
                    Input.Opcode => new Input(context),
                    Output.Opcode => new Output(context),
                    Halt.Opcode => new Halt(context),
                    JumpIfTrue.Opcode => new JumpIfTrue(context),
                    JumpIfFalse.Opcode => new JumpIfFalse(context),
                    LessThan.Opcode => new LessThan(context),
                    EqualTo.Opcode => new EqualTo(context),
                    AdjustRelativeBase.Opcode => new AdjustRelativeBase(context),
                    _ => throw new NotSupportedException($"Unknown opcode: {context.ReadMemory(context.InstructionPointer)}")
                };
    }
}
