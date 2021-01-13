using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day10
{
    internal class Solution
    {
        public string Title => "Day 10: Monitoring Station";

        public object PartOne()
        {
            var asteroids = LoadAsteroidPoints();

            var pairsWithVisibility = InAllPossiblePairs(asteroids)
                                      .Where(pair => asteroids.All(a => !IsPointDirectlyBetween(a, pair.Item1, pair.Item2)))
                                      .ToArray();
            return asteroids.Select(a => new
                                         {
                                             Asteroid = a,
                                             Count = pairsWithVisibility.Count(p => p.Item1 == a || p.Item2 == a)
                                         })
                            .OrderByDescending(a => a.Count)
                            .First()
                            .Count;
        }

        public object? PartTwo()
        {
            var laser = (22, 28);
            var asteroids = LoadAsteroidPoints()
                            .Where(asteroid => laser != asteroid)
                            .ToArray();

            var remaining = asteroids
                            .Select(asteroid => new
                                                {
                                                    Asteroid = asteroid,
                                                    Angle = CalculateAngle(laser, asteroid),
                                                    Distance = CalculateDistance(laser, asteroid)
                                                })
                            .GroupBy(a => a.Angle)
                            .OrderBy(g => g.Key)
                            .Select(g => new
                                         {
                                             Angle = g.Key,
                                             Asteroids = new Queue<(int x, int y)>(g.OrderBy(x => x.Distance)
                                                                                    .Select(x => x.Asteroid))
                                         })
                            .ToList();

            var vapourised = 0;
            while (remaining.Any())
            {
                foreach (var angle in remaining)
                {
                    var asteroid = angle.Asteroids.Dequeue();
                    if (++vapourised == 200) return $"{asteroid.x * 100 + asteroid.y}";
                }

                remaining.RemoveAll(a => !a.Asteroids.Any());
            }

            return null;
        }

        private double CalculateDistance((int x, int y) laser, (int x, int y) asteroid)
        {
            return Math.Sqrt(Math.Pow(asteroid.x - laser.x, 2) + Math.Pow(asteroid.y - laser.y, 2));
        }

        private double CalculateAngle((int x, int y) laser, (int x, int y) asteroid)
        {
            var angle = Math.Atan2(asteroid.x - laser.x, laser.y - asteroid.y) * 180 / Math.PI;
            if (angle < 0) angle = 360 + angle;
            return angle;
        }

        private static (int x, int y)[] LoadAsteroidPoints(string? fileName = null)
        {
            return InputFile.ReadAllLines(fileName)
                            .SelectMany((row, y) => row.Select((@char, x) => (point: (x, y), @char)))
                            .Where(x => x.@char == '#')
                            .Select(x => x.point)
                            .ToArray();
        }

        private static bool IsPointDirectlyBetween((int x, int y) point, (int x, int y) start, (int x, int y) end)
        {
            if (point == start || point == end) return false;
            return IsPointOnLine((start, end), point);
        }

        private static bool IsPointOnLine(((int x, int y) p, (int x, int y) q) line, (int x, int y) point)
        {
            var (p, q) = (line.p, line.q);

            if (point == p || point == q) return true;

            if (point.x < Math.Min(p.x, q.x) ||
                point.x >  Math.Max(p.x, q.x) ||
                point.y < Math.Min(p.y, q.y) ||
                point.y > Math.Max(p.y, q.y)) return false;

            if (p.x == q.x || p.y == q.y) return true;

            var expectedY = point.x * (q.y - p.y) / (decimal) (q.x - p.x) +
                            (p.y - (q.y - p.y) * p.x / (decimal) (q.x - p.x));
            return point.y == expectedY;
        }

        private static IEnumerable<(T, T)> InAllPossiblePairs<T>(IReadOnlyList<T> items)
        {
            for (var index0 = 0; index0 < items.Count - 1; index0++)
            for (var index1 = index0 + 1; index1 < items.Count; index1++)
                yield return (items[index0], items[index1]);
        }
    }
}
