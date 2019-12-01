using System;
using System.IO;
using System.Reflection;

namespace Aoc2019
{
    internal static class InputFile
    {
        public static string[] ReadAllLines(string fileName = null)
        {
            var directoryPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var filePath      = Path.Combine(directoryPath, fileName ?? "input.txt");
            if (!File.Exists(filePath)) throw new Exception($"File not found: {filePath}");

            var lines = File.ReadAllLines(filePath);
            Console.WriteLine($"Read {lines.Length} line(s) from input file: {filePath}");
            
            return lines;
        }
    }
}