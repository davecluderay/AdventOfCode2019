using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day05.Computer
{
    internal class IntCodeComputer
    {
        // ReSharper disable once EventNeverSubscribedTo.Global
        public event Action<string> Trace = delegate {};

        private readonly IntCodeExecutionContext _context = new IntCodeExecutionContext();

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
            while (true)
            {
                if (!Step()) break;
            }
        }

        public void InputFrom(params long[] inputs)
        {
            var queue = new Queue<long>(inputs);
            _context.SetInputReader(() => queue.Dequeue());
        }

        public void OutputTo(Action<long> action)
        {
            _context.SetOutputWriter(action);
        }

        private bool Step()
        {
            var instruction = ReadInstruction(_context);
            Trace(instruction.ToString());
            instruction.Execute(_context);
            return !(instruction is Halt);
        }

        private static IntCodeInstruction ReadInstruction(IntCodeExecutionContext context)
            => (context.ReadMemory(context.InstructionPointer) % 100)
               switch
               {
                   Add.Opcode         => new Add(context),
                   Multiply.Opcode    => new Multiply(context),
                   Input.Opcode       => new Input(context),
                   Output.Opcode      => new Output(context),
                   Halt.Opcode        => new Halt(context),
                   JumpIfTrue.Opcode  => new JumpIfTrue(context),
                   JumpIfFalse.Opcode => new JumpIfFalse(context),
                   LessThan.Opcode    => new LessThan(context),
                   EqualTo.Opcode     => new EqualTo(context),
                   _                  => throw new NotSupportedException($"Unknown opcode: {context.ReadMemory(context.InstructionPointer)}")
               };
    }
}
