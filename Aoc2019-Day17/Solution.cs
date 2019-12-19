using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aoc2019_Day17.Computer;

namespace Aoc2019_Day17
{
    internal class Solution
    {
        public string Title => "Day 17: Set and Forget";

        public object PartOne()
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();
            var screen = new string(computer.RunProgram().Select(o => (char) o).ToArray());

            var scaffoldPositions = FindScaffoldPositions(screen);
            var intersections = FindIntersections(scaffoldPositions);
            return intersections.Sum(p => p.x * p.y);

            IEnumerable<(int x, int y)> FindScaffoldPositions(IEnumerable<char> screenBuffer)
            {
                var (x, y) = (0, 0);
                foreach (var @char in screenBuffer)
                {
                    switch (@char)
                    {
                        case '.':
                            x++;
                            break;
                        case '#':
                        case '<':
                        case '>':
                        case '^':
                        case 'v':
                            yield return (x++, y);
                            break;
                        case '\n':
                            x = 0;
                            y++;
                            break;
                    }
                }
            }
            
            IEnumerable<(int x, int y)> FindIntersections(IEnumerable<(int x, int y)> positions)
            {
                var map = new HashSet<(int x, int y)>(positions);
                var (minX, minY, maxX, maxY) = (map.Min(p => p.x), map.Min(p => p.y),
                                                map.Max(p => p.x), map.Max(p => p.y));
                foreach (var (x, y) in map)
                {
                    var adjacentPositions = new (int x, int y)[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) };
                    var adjacentScaffoldCount = adjacentPositions
                                                .Where(p => p.x >= minX && p.x <= maxX && p.y >= minY && p.y <= maxY)
                                                .Count(p => map.Contains((p.x, p.y)));
                    if (adjacentScaffoldCount > 2) yield return (x, y);
                }
            }
        }

        public object PartTwo()
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();
            computer.SetMemory(0, 2);

            var inputs = GenerateInputs("A,B,A,B,C,C,B,C,B,A",
                                        "R,12,L,8,R,12",
                                        "R,8,R,6,R,6,R,8",
                                        "R,8,L,8,R,8,R,4,R,4",
                                        "n");
            var outputs = computer.RunProgram(inputs).ToArray();

            return outputs.Last();
            
            Func<long> GenerateInputs(string main, string a, string b, string c, string continuousVideoFeed)
            {
                var data = new Queue<long>(Encoding.ASCII.GetBytes(string.Join("\n", main, a, b, c, continuousVideoFeed, ""))
                                                   .Select(@byte => (long)@byte));
                return () => data.Dequeue();
            }
        }
    }
}