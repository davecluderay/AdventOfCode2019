using System;
using System.Collections.Generic;
using System.Linq;
using Aoc2019_Day07.Computer;

namespace Aoc2019_Day07
{
    internal class Solution
    {
        public string Title => "Day 7: Amplification Circuit";

        public object PartOne()
            => FindMaximumThrusterSignal(phasesStartAt: 0);

        public object PartTwo()
            => FindMaximumThrusterSignal(phasesStartAt: 5);

        private long FindMaximumThrusterSignal(int phasesStartAt)
        {
            var maximumSignal = 0L;
            foreach (var phaseSet in GetPhaseSets(phasesStartAt))
            {
                // Create the amplifiers.
                var amps = Enumerable.Range(1, phaseSet.Length)
                                     .Select(n => CreateAmplifier())
                                     .ToArray();

                // Chain them together by their input/output queues.
                var outputQueues = amps.Select(_ => new Queue<long>())
                                       .ToArray();
                for (var i = 0; i < amps.Length; i++)
                {
                    var j = (i + 1) % amps.Length;
                    amps[i].OutputTo(outputQueues[i]);
                    outputQueues[i].Enqueue(phaseSet[j]);
                    amps[j].InputFrom(outputQueues[i]);
                }

                // Set the initial input signal and run all amplifiers to completion.
                outputQueues.Last().Enqueue(0L);
                while (amps.Any(a => !a.IsHalted))
                {
                    for (var i = 0; i < amps.Length; i++)
                    {
                        // Run the amp until it generates another output or completes.
                        var initialQueueCount = outputQueues[i].Count;
                        while (!amps[i].IsHalted && initialQueueCount == outputQueues[i].Count)
                        {
                            amps[i].Step();
                        }
                    }
                }

                // Track the maximum signal achieved so far.
                maximumSignal = Math.Max(maximumSignal, outputQueues[^1].Last());
            }

            return maximumSignal;
        }

        private IEnumerable<int[]> GetPhaseSets(int startAt)
        {
            var values = Enumerable.Range(startAt, 5).ToArray();
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

        private IntCodeComputer CreateAmplifier()
        {
            var amplifier = new IntCodeComputer();
            amplifier.LoadProgram();

            return amplifier;
        }
    }
}
