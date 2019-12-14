using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day13.Computer
{
    internal class DebugOutput
    {
        private readonly List<string> _messages = new List<string>();

        public string Name { get; set; }
        public bool WriteToConsole { get; set; }

        public void Write(string message)
        {
            _messages.Add(message);
            if (WriteToConsole)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                
                if (!string.IsNullOrEmpty(Name)) Console.Write($"[{Name}] ");
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public void WriteRawInstruction(IntCodeMemory memory, long instructionPointer, int numberOfParameters)
        {
            var values = memory.GetAt(instructionPointer, numberOfParameters + 1);
            Write($"{instructionPointer}> ({string.Join(", ", values)})");
        }

        public void WriteInstruction(IntCodeMemory memory, long relativeBase, Opcode opcode, params OpcodeParameter[] parameters)
        {
            var parameteDescriptions = parameters.Select(p => new
                {
                    Value = p.DereferencedValue(memory, relativeBase),
                    IsPosition = p.Mode == OpcodeParameterMode.Position
                })
                .Select(p => $"{(p.IsPosition ? '*' : ' ')}{p.Value}");
            Write($"  {opcode.ToString().ToUpperInvariant()} {string.Join(", ", parameteDescriptions)}");
        }

        public IEnumerable<string> GetMessages() => _messages.AsEnumerable();
    }
}