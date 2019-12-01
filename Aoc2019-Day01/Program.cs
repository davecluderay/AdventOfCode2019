using System;
using System.Linq;

namespace Aoc2019_Day01
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                var solution = new Solution();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(solution.Title);
                Console.ResetColor();

                var result1 = solution.PartOne();
                
                Console.Write("Part One: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result1);
                Console.ResetColor();
            
                var result2 = solution.PartTwo();
                
                Console.Write("Part Two: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(result2);
                Console.ResetColor();
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(exception);
            }
            finally
            {
                Console.WriteLine("Done.");
            }
        }
    }
}
