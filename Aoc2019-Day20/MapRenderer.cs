using System;

namespace Aoc2019_Day20
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
                        if (gridMap.IsPortal((row, column)))
                        {
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        else if (gridMap.IsEntrance((row, column)))
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else if (gridMap.IsExit((row, column)))
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else if (gridMap.At((row, column)) == '.')
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }
                        else if (char.IsLetter(gridMap.At((row, column))))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
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