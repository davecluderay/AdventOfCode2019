using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aoc2019_Day07.Computer;

namespace Aoc2019_Day07
{
    internal class Solution
    {
        public string Title => "Day 7: Amplification Circuit";

        public object PartOne()
        {
            var amplifiers = CreateAmplifiers(5);
            
            var strongestOutputSignal = 0;
            foreach (var phaseSet in GetPhaseSets(new[] { 0, 1, 2, 3, 4 }))
            {
                var signal = 0;
                for (var index = 0; index < amplifiers.Length; index++)
                {
                    var phase = phaseSet[index];
                    var computer = amplifiers[index];
                    
                    computer.LoadProgram();
                    computer.RunProgram(phase, signal);
                    
                    signal = computer.GetLastOutput();
                }
            
                strongestOutputSignal = Math.Max(signal, strongestOutputSignal);
            }

            return strongestOutputSignal;
        }
        
        public object PartTwo()
        {
            var amplifiers = CreateAmplifiers(5);

            var strongestOutputSignal = 0;
            foreach (var phaseSet in GetPhaseSets(new[] { 5, 6, 7, 8, 9 }))
            {
                var outputQueues = Enumerable.Range(1, amplifiers.Length)
                                             .Select(_ => new BlockingCollection<int>())
                                             .ToArray();

                var runTasks = amplifiers.Select(
                    (amplifier, index) =>
                        {
                            var upstreamOutputQueue = outputQueues[index == 0 ? ^1 : index - 1];
                            var outputQueue         = outputQueues[index];
                        
                            var phase = phaseSet[index];
                            var starterInputs = index == 0 ? new[] { phase, 0 } : new [] { phase };
                            var allInputs = GetCombinedInputStream(starterInputs, upstreamOutputQueue);

                            return Task.Factory.StartNew(
                                () =>
                                    {
                                        amplifier.LoadProgram();
                                        var outputSignals = amplifier.RunProgramIncrementally(allInputs);
                                        try
                                        {
                                            foreach (var outputSignal in outputSignals)
                                            {
                                                outputQueue.Add(outputSignal);
                                            }
                                        }
                                        finally
                                        {
                                            outputQueue.CompleteAdding();
                                        }
                                    });
                        }).ToArray();
                Task.WaitAll(runTasks);
                
                var signal = amplifiers[^1].GetLastOutput();
                strongestOutputSignal = Math.Max(signal, strongestOutputSignal);
            }

            return strongestOutputSignal;
        }
        
        private static IntCodeComputer[] CreateAmplifiers(int count, bool writeDebugToConsole = false)
        {
            return Enumerable.Range(1, count)
                             .Select((x, n) => new IntCodeComputer(new DebugOutput
                                                                   {
                                                                       Name = $"AMP-{n}",
                                                                       WriteToConsole = writeDebugToConsole
                                                                   }))
                             .ToArray();
            
        }
        
        private static IEnumerable<int[]> GetPhaseSets(int[] values)
        {
            foreach (var v1 in values)
            foreach (var v2 in values)
            foreach (var v3 in values)
            foreach (var v4 in values)
            foreach (var v5 in values)
            {
                var set = new[] { v1, v2, v3, v4, v5 };
                if (set.Distinct().Count() == 5)
                    yield return set;
            }
        }

        private static IEnumerable<T> GetCombinedInputStream<T>(IEnumerable<T> preamble, BlockingCollection<T> stream)
        {
            foreach (var item in preamble)
                yield return item;
            
            while (!stream.IsCompleted)
                if (stream.TryTake(out T item))
                    yield return item;
        }
    }
}