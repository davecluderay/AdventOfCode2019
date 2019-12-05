using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day05
{
    internal class IntCodeComputer
    {
        private const int OutputMemoryAddress = 0;
        
        private readonly DebugOutput _debug;

        private int[] _memory = Array.Empty<int>();
        private List<int> _outputs = new List<int>();

        public IntCodeComputer(DebugOutput debug = null)
        {
            _debug = debug ?? new DebugOutput();
        }

        public void LoadProgram(string fileName = null)
        {
            _memory = InputFile.ReadAllText(fileName)
                               .Split(',')
                               .Select(int.Parse)
                               .ToArray();
            _outputs = new List<int>();
        }

        public int[] GetIntermediateOutput() => _outputs.Take(_outputs.Count - 1).ToArray();
        public int GetLastOutput() => _outputs.LastOrDefault();

        public void RunProgram(params int[] inputs)
        {
            var inputQueue = new Queue<int>(inputs);
            var instructionPointer = 0;
            var running = true;
            while (running)
            {
                var opcode = (Opcode)(_memory[instructionPointer] % 100);
                switch (opcode)
                {
                    case Opcode.Add:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 3);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 3);
                        var operand1 = parameters[0].DereferencedValue(_memory);
                        var operand2 = parameters[1].DereferencedValue(_memory);
                        var address = parameters[2].RawValue;
                        
                        _debug.WriteInstruction(_memory, opcode, parameters);
                        
                        SetMemory(address, operand1 + operand2);
                        instructionPointer += 4;
                        break;
                    }
                    case Opcode.Multiply:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 3);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 3);
                        var operand1 = parameters[0].DereferencedValue(_memory);
                        var operand2 = parameters[1].DereferencedValue(_memory);
                        var address = parameters[2].RawValue;
                        
                        _debug.WriteInstruction(_memory, opcode, parameters);
                        
                        SetMemory(address, operand1 * operand2);
                        instructionPointer += 4;
                        break;
                    }
                    case Opcode.Input:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 1);

                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 1);
                        var input = inputQueue.Dequeue();
                        var address = parameters[0].RawValue;
                        
                        _debug.WriteInstruction(_memory, opcode, parameters);
                        
                        SetMemory(address, input);
                        instructionPointer += 2;
                        break;
                    }
                    case Opcode.Output:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 1);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 1);
                        var operand = parameters[0].DereferencedValue(_memory);
                        
                        _debug.WriteInstruction(_memory, opcode, parameters);
                        
                        SetMemory(OutputMemoryAddress, operand);
                        instructionPointer += 2;
                        break;
                    }
                    case Opcode.JumpIfTrue:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 2);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 2);
                        var operand = parameters[0].DereferencedValue(_memory);
                        var address = parameters[1].DereferencedValue(_memory);
                        
                        _debug.WriteInstruction(_memory, opcode, parameters);
                        
                        instructionPointer = (operand != 0) ? address : (instructionPointer + 3);
                        break;
                    }
                    case Opcode.JumpIfFalse:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 2);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 2);
                        var operand    = parameters[0].DereferencedValue(_memory);
                        var address    = parameters[1].DereferencedValue(_memory);
                        
                        _debug.WriteInstruction(_memory, opcode, parameters);
                        
                        instructionPointer = (operand == 0) ? address : (instructionPointer + 3);
                        break;
                    }
                    case Opcode.LessThan:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 3);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 3);
                        var operand1 = parameters[0].DereferencedValue(_memory);
                        var operand2 = parameters[1].DereferencedValue(_memory);
                        var address = parameters[2].RawValue;
                        
                        _debug.WriteInstruction(_memory, opcode, parameters);
                        
                        SetMemory(address, (operand1 < operand2) ? 1 : 0);
                        instructionPointer += 4;
                        break;
                    }
                    case Opcode.Equals:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 3);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 3);
                        var operand1   = parameters[0].DereferencedValue(_memory);
                        var operand2   = parameters[1].DereferencedValue(_memory);
                        var address    = parameters[2].RawValue;
                        
                        _debug.WriteInstruction(_memory, opcode, parameters);
                        
                        SetMemory(address, (operand1 == operand2) ? 1 : 0);
                        instructionPointer += 4;
                        break;
                    }
                    case Opcode.Halt:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 0);
                        _debug.WriteInstruction(_memory, opcode);
                        
                        running = false;
                        instructionPointer += 1;
                        break;
                    }
                    default:
                        throw new Exception($"Unrecognised opcode: {opcode}");
                }
            }
        }

        private void SetMemory(int address, int value)
        {
            _debug.Write($"Writing {value} to position {address}");
            if (address == OutputMemoryAddress) _outputs.Add(value);
            _memory[address] = value;
        }
    }
}