using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day12
{
    internal class Solution
    {
        public string Title => "Day 12: The N-Body Problem";

        public object PartOne()
        {
            var bodies = LoadBodies().ToArray();

            for (int step = 0; step < 1000; step++)
            {
                // Adjust velocities.
                foreach (var pair in Combine.IntoAllPossiblePairs(bodies))
                {
                    var xOffset = Math.Sign(pair.Item2.Position.x - pair.Item1.Position.x);
                    pair.Item1.Velocity.x += xOffset;
                    pair.Item2.Velocity.x -= xOffset;
                    
                    var yOffset = Math.Sign(pair.Item2.Position.y - pair.Item1.Position.y);
                    pair.Item1.Velocity.y += yOffset;
                    pair.Item2.Velocity.y -= yOffset;
                    
                    var zOffset = Math.Sign(pair.Item2.Position.z - pair.Item1.Position.z);
                    pair.Item1.Velocity.z += zOffset;
                    pair.Item2.Velocity.z -= zOffset;
                }
                
                // Apply velocities.
                foreach (var body in bodies)
                {
                    body.Position.x += body.Velocity.x;
                    body.Position.y += body.Velocity.y;
                    body.Position.z += body.Velocity.z;
                }
            }
            
            return bodies.Sum(CalculateTotalEnergy);
            
            int CalculateTotalEnergy(Body body)
            {
                var potentialEnergy = Math.Abs(body.Position.x) + Math.Abs(body.Position.y) + Math.Abs(body.Position.z);
                var kineticEnergy   = Math.Abs(body.Velocity.x) + Math.Abs(body.Velocity.y) + Math.Abs(body.Velocity.z);
                return potentialEnergy * kineticEnergy;
            }
        }

        public object PartTwo()
        {
            var bodies = LoadBodies().ToArray();

            var repeatFrequencies = new[]
                                    {
                                        FindRepeatFrequencyOnAxis(bodies.Select(b => (b.Position.x, b.Velocity.x)).ToArray()),
                                        FindRepeatFrequencyOnAxis(bodies.Select(b => (b.Position.y, b.Velocity.y)).ToArray()),
                                        FindRepeatFrequencyOnAxis(bodies.Select(b => (b.Position.z, b.Velocity.z)).ToArray())
                                    };

            return Calculate.LowestCommonMultiple(repeatFrequencies);
            
            long FindRepeatFrequencyOnAxis((int position, int velocity)[] positionsAndVelocities)
            {
                var positions  = positionsAndVelocities.Select(b => b.position).ToArray();
                var velocities = positionsAndVelocities.Select(b => b.velocity).ToArray();
            
                var history = new HashSet<string>();
                while (true)
                {
                    var asText = $"{string.Join(';', positions.Concat(velocities))}";
                    if (history.Contains(asText)) return history.Count;
                
                    history.Add(asText);
                
                    // Adjust velocities.
                    foreach (var indexes in Combine.IntoAllPossiblePairs(Enumerable.Range(0, positions.Length).ToArray()))
                    {
                        var delta = Math.Sign(positions[indexes.Item2] - positions[indexes.Item1]);
                        velocities[indexes.Item1] += delta;
                        velocities[indexes.Item2] -= delta;
                    }
            
                    // Apply velocities.
                    for (int index = 0; index < positions.Length; index++)
                    {
                        positions[index] += velocities[index];
                    }
                }
            }
        }

        private static Body[] LoadBodies(string fileName = null) => InputFile.ReadAllLines(fileName)
                                                                             .Select(Body.Parse)
                                                                             .ToArray();
    }
}