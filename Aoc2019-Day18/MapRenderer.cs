using System;

namespace Aoc2019_Day18
{
    internal class MapRenderer
    {
        public void Render(GridMap gridMap)
        {
            const char filledBlock = '\u2588';

            for (var row = 0; row < gridMap.Rows; row++)
            {
                for (var column = 0; column < gridMap.Columns; column++)
                {
                    if (gridMap.IsWall((row, column)))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(filledBlock);
                        Console.ResetColor();
                    }
                    else
                    {
                        if (gridMap.IsKey((row, column)))
                        {
                            Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (gridMap.IsGate((row, column)))
                        {
                            Console.BackgroundColor = ConsoleColor.Magenta;
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        if (gridMap.At((row, column)) == '.')
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }

                        Console.Write(gridMap.At((row, column)));
                        Console.ResetColor();
                    }
                }

                Console.WriteLine();
            }
        }
    }
}