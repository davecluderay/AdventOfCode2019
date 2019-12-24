using System;
using System.Collections.Generic;
using System.Linq;
using Aoc2019_Day19.Computer;

namespace Aoc2019_Day19
{
    internal class BeamScanner
    {
        public long[,] ScanBeam((int x, int y) origin, (int width, int height) dimensions, (int minX, int minY, int maxX, int maxY)? excludeRegion = null)
        {   
            var computer = new IntCodeComputer();

            var outputBuffer = new long[dimensions.height, dimensions.width];
            for (var y = 0; y < outputBuffer.GetLength(0); y++)
            for (var x = 0; x < outputBuffer.GetLength(1); x++)
            {
                if (excludeRegion != null &&
                    x + origin.x >= excludeRegion.Value.minX &&
                    x + origin.x <= excludeRegion.Value.maxX &&
                    y + origin.y >= excludeRegion.Value.minY &&
                    y + origin.y <= excludeRegion.Value.maxY)
                    continue;
                
                computer.LoadProgram();
                var output = computer.RunProgram(InputGenerator(x + origin.x, y + origin.y))
                                     .Single();
                outputBuffer[y, x] = output;    
            }

            return outputBuffer;
        }

        public (int x, int length) GetBeamHorizontalExtent(int atY, int startAtX)
        {
            int? firstX = null;

            var x = startAtX;
            while (true)
            {
                var computer = new IntCodeComputer();
                computer.LoadProgram();
                var output = computer.RunProgram(InputGenerator(x, atY))
                                     .Single();
                    
                if (firstX == null && output == 1)
                    firstX = x;
            
                if (firstX != null && output == 0)
                    return (x: firstX.Value, length: x - firstX.Value);
            
                x++;
            }
        }

        private Func<long> InputGenerator(int x, int y)
        {
            var queue = new Queue<long>(new long[] { x, y });
            return () => queue.Dequeue();
        }
    }
}