using System;

namespace Aoc2019_Day24
{
    internal class LayoutRenderer
    {
        public void Render(Layout layout, int minute = 0)
        {
            const char filledBlock = '\u2588';
            const char space = ' ';

            RenderElapsedTime(minute);
            
            for (var row = 0; row < layout.Rows; row++)
            {
                for (var column = 0; column < layout.Columns; column++)
                {
                    var position = (row, column);
                    var color = layout.IsBugAt(position) ? ConsoleColor.DarkGreen : ConsoleColor.DarkGray;
                    var @char = layout.IsBugAt(position) ? filledBlock : space;
                    
                    Console.BackgroundColor = Console.ForegroundColor = color;
                    Console.Write(@char);
                    Console.ResetColor();
                }

                Console.WriteLine();
            }
            
            Console.WriteLine();
        }

        public void Render(RecursiveLayout layout, int minute = 0)
        {
            RenderElapsedTime(minute);

            // Start from the outermost level.
            var level = layout;
            while (level.OuterLevel.HasBeenInfested)
                level = level.OuterLevel;

            // Work inwards.
            while (level.HasBeenInfested)
            {
                RenderRecursionLevelLabel(level);
                for (var row = 0; row < level.Rows; row++)
                {
                    for (var column = 0; column < level.Columns; column++)
                    {
                        var position = (row, column);
                        var color = level.IsBugAt(position)
                            ? ConsoleColor.DarkGreen
                            : level.CentrePosition == position ? ConsoleColor.Magenta : ConsoleColor.DarkGray;
                        var @char = level.At(position);

                        Console.BackgroundColor = Console.ForegroundColor = color;
                        Console.Write(@char);
                        Console.ResetColor();
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
                
                level = level.InnerLevel;
            }
        }

        private void RenderRecursionLevelLabel(RecursiveLayout level)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{{0,{level.Columns}}}", level.Level);
            Console.ResetColor();
        }

        private void RenderElapsedTime(in int minute)
        {
            if (minute == 0)
                Console.WriteLine("Initial position:");
            else if (minute == 1)
                Console.WriteLine("After 1 minute:");
            else if (minute > 1)    
                Console.WriteLine($"After {minute} minutes:");

            Console.WriteLine();
        }
    }
}