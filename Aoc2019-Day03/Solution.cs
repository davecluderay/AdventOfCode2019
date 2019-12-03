using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day03
{
    internal class Solution
    {
        public string Title => "Day 3: Crossed Wires";

        public object PartOne()
        {
            var data = WireData.Read();
            var intersections = FindIntersectionPoints(data.wire1, data.wire2);
            return intersections.Select(p => Math.Abs(p.x) + Math.Abs(p.y))
                                .OrderBy(d => d)
                                .Cast<int?>()
                                .FirstOrDefault();
        }
        
        public object PartTwo()
        {
            var data = WireData.Read();
            var intersections = FindIntersectionPoints(data.wire1, data.wire2);
            return intersections.Select(p => CalculatePointDistanceAlongWire(data.wire1, p) + CalculatePointDistanceAlongWire(data.wire2, p))
                                .OrderBy(d => d)
                                .Cast<int?>()
                                .FirstOrDefault();
        }

        private static IEnumerable<(int x, int y)> FindIntersectionPoints((int x, int y)[] wire1, (int x, int y)[] wire2)
        {
            for (var index1 = 0; index1 < wire1.Length - 1; index1++)
            for (var index2 = 0; index2 < wire2.Length - 1; index2++)
            {
                var intersect = CalculateIntersectionPoint(
                    wire1[index1], wire1[index1 + 1],
                    wire2[index2], wire2[index2 + 1]);
                
                if (intersect == null) continue;
                if (intersect == (0, 0)) continue;

                yield return intersect.Value;
            }
        }
        
        private static (int x, int y)? CalculateIntersectionPoint((int x, int y) p1, (int x, int y) p2, (int x, int y) q1, (int x, int y) q2)
        {
            if ((p1.x == p2.x) == (q1.x == q2.x)) return null; // Parallel.

            var horizontal = p1.y == p2.y ? (x1: p1.x, x2: p2.x, y: p1.y) : (x1: q1.x, x2: q2.x, y: q1.y);
            var vertical  = p1.x == p2.x ? (x: p1.x, y1: p1.y, y2: p2.y) : (x: q1.x, y1: q1.y, y2: q2.y);
            
            if (horizontal.y < Math.Min(vertical.y1, vertical.y2) || horizontal.y > Math.Max(vertical.y1, vertical.y2)) return null;
            if (vertical.x < Math.Min(horizontal.x1, horizontal.x2) || vertical.x > Math.Max(horizontal.x1, horizontal.x2)) return null;
            
            return (vertical.x, horizontal.y);
        }

        private static int CalculatePointDistanceAlongWire((int x, int y)[] wire, (int x, int y) point)
        {
            var distance = 0;
            for (var index = 0; index < wire.Length - 1; index++)
            {
                var (p1, p2) = (wire[index], wire[index + 1]);

                var offset = FindPointDistanceAlongSegment(p1, p2, point);
                if (offset != null)
                {
                    return distance + offset.Value;
                }
                
                distance = distance + Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
            }

            throw new Exception("Point not found on wire.");
        }
        
        private static int? FindPointDistanceAlongSegment((int x, int y) p1, (int x, int y) p2, (int x, int y) p)
        {
            if (p1.x == p2.x && p1.x == p.x)
            {
                var minY = Math.Min(p1.y, p2.y);
                var maxY = Math.Max(p1.y, p2.y);
                if (minY <= p.y && maxY >= p.y)
                {
                    return Math.Abs(p.y - p1.y);
                }
            }

            if (p1.y == p2.y && p1.y == p.y)
            {
                var minX = Math.Min(p1.x, p2.x);
                var maxX = Math.Max(p1.x, p2.x);
                if (minX <= p.x && maxX >= p.x)
                {
                    return Math.Abs(p.x - p1.x);
                }
            }
                
            return null;
        }
    }
}