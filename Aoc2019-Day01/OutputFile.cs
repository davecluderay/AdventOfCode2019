using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Aoc2019_Day01
{
    internal static class OutputFile
    {
        public static void WriteAllLines(IEnumerable<string> lines, string fileName = null)
        {
            var directoryPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var filePath      = Path.Combine(directoryPath, fileName ?? "output.txt");

            File.WriteAllLines(filePath, lines);
        }
    }
}