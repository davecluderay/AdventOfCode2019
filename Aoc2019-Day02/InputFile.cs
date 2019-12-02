using System;
using System.IO;
using System.Reflection;

namespace Aoc2019_Day02
{
    internal static class InputFile
    {
        public static string[] ReadAllLines(string fileName = null)
        {
            var directoryPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var filePath      = Path.Combine(directoryPath, fileName ?? "input.txt");
            if (!File.Exists(filePath)) throw new Exception($"File not found: {filePath}");

            return File.ReadAllLines(filePath);
        }
    }
}