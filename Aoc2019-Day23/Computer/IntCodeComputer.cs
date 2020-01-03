using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day23.Computer
{
    internal class IntCodeComputer
    {
        private readonly DebugOutput _debug;

        private readonly IntCodeMemory _memory = new IntCodeMemory();

        public IntCodeComputer(DebugOutput debug = null)
        {
            _debug = debug ?? new DebugOutput();
        }

        public void LoadProgram(string fileName = null)
        {
            var program = InputFile.ReadAllText(fileName)
                                   .Split(',')
                                   .Select(long.Parse)
                                   .ToArray();
            _memory.Set(0, program);
        }

        public IEnumerable<long> RunProgram()
        {
            return RunProgram(NoInputGenerator);
        }
        
        public IEnumerable<long> RunProgram(Func<long> inputGenerator)
        {
            return RunProgramIncrementally(inputGenerator)
                   .Where(r => r.IsOutput)
                   .Select(r => r.OutputValue);
        }

        public IEnumerable<IncrementalRunResult> RunProgramIncrementally()
        {
            return RunProgramIncrementally(NoInputGenerator);
        }

        public IEnumerable<IncrementalRunResult> RunProgramIncrementally(Func<long> inputGenerator)
        {
            var instructionPointer = 0L;
            var relativeBase = 0L;
            var running = true;
            while (running)
            {
                var opcode = (Opcode)(_memory.GetAt(instructionPointer) % 100);
                switch (opcode)
                {
                    case Opcode.Add:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 3);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 3);
                        var operand1 = parameters[0].DereferencedValue(_memory, relativeBase);
                        var operand2 = parameters[1].DereferencedValue(_memory, relativeBase);
                        var address = parameters[2].Value(relativeBase);
                        
                        _debug.WriteInstruction(_memory, relativeBase, opcode, parameters);
                        
                        SetMemory(address, operand1 + operand2);

                        yield return IncrementalRunResult.Empty;
                        instructionPointer += 4;
                        break;
                    }
                    case Opcode.Multiply:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 3);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 3);
                        var operand1 = parameters[0].DereferencedValue(_memory, relativeBase);
                        var operand2 = parameters[1].DereferencedValue(_memory, relativeBase);
                        var address = parameters[2].Value(relativeBase);
                        
                        _debug.WriteInstruction(_memory, relativeBase, opcode, parameters);

                        SetMemory(address, operand1 * operand2);
                        
                        yield return IncrementalRunResult.Empty;
                        instructionPointer += 4;
                        break;
                    }
                    case Opcode.Input:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 1);

                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 1);
                        var input = inputGenerator.Invoke();
                        var address = parameters[0].Value(relativeBase);
                        
                        _debug.WriteInstruction(_memory, relativeBase, opcode, parameters);
                        
                        SetMemory(address, input);
                        
                        yield return IncrementalRunResult.ForInput(input);
                        instructionPointer += 2;
                        break;
                    }
                    case Opcode.Output:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 1);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 1);
                        var operand = parameters[0].DereferencedValue(_memory, relativeBase);
                        
                        _debug.WriteInstruction(_memory, relativeBase, opcode, parameters);

                        yield return IncrementalRunResult.ForOutput(operand);
                        instructionPointer += 2;
                        break;
                    }
                    case Opcode.JumpIfTrue:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 2);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 2);
                        var operand = parameters[0].DereferencedValue(_memory, relativeBase);
                        var address = parameters[1].DereferencedValue(_memory, relativeBase);
                        
                        _debug.WriteInstruction(_memory, relativeBase, opcode, parameters);
                        
                        yield return IncrementalRunResult.Empty;
                        instructionPointer = (operand != 0) ? address : (instructionPointer + 3);
                        break;
                    }
                    case Opcode.JumpIfFalse:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 2);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 2);
                        var operand    = parameters[0].DereferencedValue(_memory, relativeBase);
                        var address    = parameters[1].DereferencedValue(_memory, relativeBase);
                        
                        _debug.WriteInstruction(_memory, relativeBase, opcode, parameters);
                        
                        yield return IncrementalRunResult.Empty;
                        instructionPointer = (operand == 0) ? address : (instructionPointer + 3);
                        break;
                    }
                    case Opcode.LessThan:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 3);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 3);
                        var operand1 = parameters[0].DereferencedValue(_memory, relativeBase);
                        var operand2 = parameters[1].DereferencedValue(_memory, relativeBase);
                        var address = parameters[2].Value(relativeBase);
                        
                        _debug.WriteInstruction(_memory, relativeBase, opcode, parameters);
                        
                        SetMemory(address, (operand1 < operand2) ? 1 : 0);

                        yield return IncrementalRunResult.Empty;
                        instructionPointer += 4;
                        break;
                    }
                    case Opcode.Equals:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 3);
                        
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 3);
                        var operand1   = parameters[0].DereferencedValue(_memory, relativeBase);
                        var operand2   = parameters[1].DereferencedValue(_memory, relativeBase);
                        var address    = parameters[2].Value(relativeBase);
                        
                        _debug.WriteInstruction(_memory, relativeBase, opcode, parameters);
                        
                        SetMemory(address, (operand1 == operand2) ? 1 : 0);

                        yield return IncrementalRunResult.Empty;
                        instructionPointer += 4;
                        break;
                    }
                    case Opcode.RelativeBaseOffset:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 1);
                        var parameters = OpcodeParameter.ReadParameters(_memory, instructionPointer, 1);
                        var operand    = parameters[0].DereferencedValue(_memory, relativeBase);

                        _debug.WriteInstruction(_memory, relativeBase, opcode, parameters);

                        relativeBase += operand;

                        yield return IncrementalRunResult.Empty;
                        instructionPointer += 2;
                        break;
                    }
                    case Opcode.Halt:
                    {
                        _debug.WriteRawInstruction(_memory, instructionPointer, 0);
                        _debug.WriteInstruction(_memory, relativeBase, opcode);
                        
                        running = false;
                        instructionPointer += 1;
                        break;
                    }
                    default:
                        throw new Exception($"Unrecognised opcode: {opcode}");
                }
            }
        }

        private void SetMemory(long position, long value)
        {
            _debug.Write($"Writing {value} to position {position}");
            _memory.SetAt(position, value);
        }

        private long NoInputGenerator()
        {
            throw new InvalidOperationException("No input available.");
        }
        
        public struct IncrementalRunResult
        {
            private long? _outputValue;
            private long? _inputValue;
        
            public bool IsOutput => _outputValue.HasValue;
            public bool IsInput => _inputValue.HasValue;
            public long OutputValue => _outputValue ?? throw new InvalidOperationException("Last instruction was not an output.");
            public long InputValue => _inputValue ?? throw new InvalidOperationException("Last instruction was not an input.");

            private IncrementalRunResult(long? output, long? input)
            {
                _outputValue = output;
                _inputValue = input;
            }
        
            public static IncrementalRunResult ForOutput(long output) => new IncrementalRunResult(output, null);
            public static IncrementalRunResult ForInput(long input) => new IncrementalRunResult(null, input);
            public static readonly IncrementalRunResult Empty = new IncrementalRunResult(null, null);
        }
    }
}