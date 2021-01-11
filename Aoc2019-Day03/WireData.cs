using System;
using System.Collections.Generic;
using System.Linq;
using Aoc2019_Day03;

internal static class WireData
{
    public static ((int x, int y)[] wire1, (int x, int y)[] wire2) Read(string? fileName = null)
    {
        var lines = InputFile.ReadAllLines(fileName);

        return (ReadLineAsPoints(lines[0]).ToArray(), ReadLineAsPoints(lines[1]).ToArray());
    }

    private static IEnumerable<(int x, int y)> ReadLineAsPoints(string line)
    {
        var (x, y) = (0, 0);
        yield return (x, y);
        foreach (var vector in line.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var direction = vector[0];
            var distance  = int.Parse(vector.Substring(1));
            switch (direction)
            {
                case 'L':
                    x -= +distance;
                    break;
                case 'R':
                    x += distance;
                    break;
                case 'U':
                    y -= distance;
                    break;
                case 'D':
                    y += distance;
                    break;
            }
            yield return (x, y);
        }
    }
}
