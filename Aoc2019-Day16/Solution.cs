using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aoc2019_Day16
{
    internal class Solution
    {
        public string Title => "Day 16: Flawed Frequency Transmission";

        public object PartOne()
        {
            var signal = InputFile.ReadAllLines()
                                  .Single()
                                  .Select(c => c - '0')
                                  .ToArray();

            return Transform(signal);
        }

        public object PartTwo()
        {
            var signalText = InputFile.ReadAllLines()
                                      .Single();

            var messageOffset = Convert.ToInt32(signalText.Substring(0, 7));

            var signal = signalText.Select(c => c - '0')
                                   .RepeatSequence(10000)
                                   .ToArray();

            return Transform(signal, messageOffset: messageOffset);
        }

        private string Transform(int[] signal, int phases = 100, int messageOffset = 0)
        {
            var inputSignal = (int[])signal.Clone();
            var outputSignal = (int[])signal.Clone();
            for (var phase = 0; phase < phases; phase++)
            {
                Swap(ref inputSignal, ref outputSignal);
                TransformSignal(inputSignal, outputSignal, messageOffset);
            }

            return string.Join("", outputSignal.Skip(messageOffset).Take(8));

            static void Swap<T>(ref T ref1, ref T ref2)
            {
                var temp = ref1;
                ref1 = ref2;
                ref2 = temp;
            }
        }

        private void TransformSignal(int[] inputSignal, int[] outputSignal, int messageOffset = 0)
        {
            // Note: If we are only interested in the output values from a certain offset,
            //       we don't need to calculate the values before that offset in ANY of the phases.
            //       There are some special cases if the offset is in the second half of the signal,
            //       but this is adequate performance and will work for any offset.

            var cumulativeSums = new Dictionary<int, int> { [messageOffset] = 0 };
            for (var index = messageOffset; index < inputSignal.Length; index++)
            {
                cumulativeSums[index + 1] = cumulativeSums[index] + inputSignal[index];
            }

            Parallel.For(messageOffset,
                         inputSignal.Length,
                         index =>
                         {
                             var outputAtIndex = 0;
                             foreach (var segment in TransformPattern.AtIndex(index, inputSignal.Length, messageOffset))
                             {
                                 var sum = cumulativeSums[segment.StartIndex + segment.Length] - cumulativeSums[segment.StartIndex];
                                 outputAtIndex += sum * segment.Multiplier;
                             }

                             outputSignal[index] = Math.Abs(outputAtIndex % 10);
                         });
        }

        private static class TransformPattern
        {
            public class Segment
            {
                public int StartIndex { get; }
                public int Length { get; }
                public int Multiplier { get; }
                public Segment(int startIndex, int length, int multiplier)
                    => (StartIndex, Length, Multiplier) = (startIndex, length, multiplier);
            }

            public static IEnumerable<Segment> AtIndex(int signalIndex, int signalLength, int fromOffset)
            {
                var multiplierSequence = new[] { 0, 1, 0, -1 };

                var isFirstSegment = true;
                var nextMultiplierIndex = 0;
                var nextStartIndex = 0;
                while (true)
                {
                    if (nextStartIndex >= signalLength) yield break;

                    if (nextStartIndex + signalLength < fromOffset) continue;

                    var multiplier = multiplierSequence[nextMultiplierIndex];
                    if (isFirstSegment)
                    {
                        if (signalIndex > 0)
                        {
                            if (multiplier != 0) yield return new Segment(nextStartIndex, signalIndex, multiplier);
                            nextStartIndex = nextStartIndex + signalIndex;
                        }
                        isFirstSegment = false;
                    }
                    else
                    {
                        if (multiplier != 0)
                        {
                            var segmentLength = Math.Min(signalIndex + 1, signalLength - nextStartIndex);
                            yield return new Segment(nextStartIndex, segmentLength, multiplier);
                        }
                        nextStartIndex = nextStartIndex + signalIndex + 1;
                    }

                    nextMultiplierIndex = (nextMultiplierIndex + 1) % multiplierSequence.Length;
                }
            }
        }
    }
}
