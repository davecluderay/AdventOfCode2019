using System;

namespace Aoc2019_Day01
{
    public static class Program
    {
        private static void Run()
        {
            var lines = InputFile.ReadAllLines();
        }

        public static void Main()
        {
            try
            {
                Run();
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            }
            finally
            {
                Console.WriteLine("Done.");
            }
        }
    }
}
