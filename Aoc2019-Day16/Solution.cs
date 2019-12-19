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

        private string Transform(int[] signal, int iterations = 100, int messageOffset = 0)
        {
            var inputSignal = (int[])signal.Clone();
            var outputSignal = (int[])signal.Clone();
            for (var n = 0; n < iterations; n++)
            {
                Console.Write('.');
                Swap(ref inputSignal, ref outputSignal);
                TransformSignal(inputSignal, outputSignal);
            }
            Console.WriteLine();

            return string.Join("", outputSignal.Skip(messageOffset).Take(8));

            void Swap<T>(ref T ref1, ref T ref2)
            {
                var temp = ref1;
                ref1 = ref2;
                ref2 = temp;
            }
        }

        private void TransformSignal(int[] inputSignal, int[] outputSignal)
        {
            var cumulativeSums = new long[inputSignal.Length + 1];
            for (var index = 0; index < inputSignal.Length; index++)
            {
                cumulativeSums[index + 1] = (cumulativeSums[index] + inputSignal[index]);
            }

            Parallel.For(0,
                         inputSignal.Length,
                         (index, _) =>
                         {
                             var outputAtIndex = 0;
                             foreach (var segment in TransformPattern.AtIndex(index, inputSignal.Length))
                             {
                                 outputAtIndex += (int) (cumulativeSums[segment.StartIndex + segment.Length] - cumulativeSums[segment.StartIndex]) * segment.Multiplier;
                             }

                             outputSignal[index] = Math.Abs(outputAtIndex) % 10;
                         });
        }

        private class TransformPattern
        {
            public class Segment
            {
                public int StartIndex { get; }
                public int Length { get; }
                public int Multiplier { get; }

                public Segment(int startIndex, int length, int multiplier)
                {
                    StartIndex = startIndex;
                    Length = length;
                    Multiplier = multiplier;
                }
            }

            public static IEnumerable<Segment> AtIndex(int signalIndex, int signalLength)
            {
                var multiplierSequence = new[] { 0, 1, 0, -1 };

                var isFirstSegment = true;
                var nextMultiplierIndex = 0;
                var nextStartIndex = 0;
                while (true)
                {
                    if (nextStartIndex >= signalLength) yield break;

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
