using System;

namespace Aoc2019_Day11
{
    internal static class PanelGridConsoleRenderer
    {
        public static void Render(PanelGrid panelGrid)
        {
            const char FilledBlock = '\u2588';

            var (minX, minY, maxX, maxY) = panelGrid.Bounds;
            for (var row = 0; row <= maxY - minY; row++)
            {
                for (var col = 0; col <= maxX - minX; col++)
                {
                    var color = panelGrid.Get((minX + col, minY + row));
                    Console.ForegroundColor = color == PaintColor.Black ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write(FilledBlock);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}