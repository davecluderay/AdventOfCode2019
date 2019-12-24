using System;

namespace Aoc2019_Day19
{
    internal class ConsoleBeamRenderer
    {
        public void RenderBeam(long[,] outputBuffer, (int minX, int minY, int maxX, int maxY)? objectAt = null)
        {
            for (var y = 0; y < outputBuffer.GetLength(0); y++)
            {
                for (var x = 0; x < outputBuffer.GetLength(1); x++)
                {
                    if (objectAt != null &&
                        x >= objectAt.Value.minX &&
                        x <= objectAt.Value.maxX &&
                        y >= objectAt.Value.minY &&
                        y <= objectAt.Value.maxY)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write('O');
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(outputBuffer[y, x] == 1 ? '#' : ' ');
                    }
                }
                Console.WriteLine();
            }
        }
    }
}