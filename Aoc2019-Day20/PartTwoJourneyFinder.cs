using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day20
{
    internal class PartTwoJourneyFinder
    {
        private readonly int _maxRecursionLevel;

        public PartTwoJourneyFinder(int maxRecursionLevel = 10)
        {
            _maxRecursionLevel = maxRecursionLevel;
        }
        
        public int FindShortestJourneyStepCount(MazeGraph graph)
        {
            var cache = new Dictionary<string, int>();
            var start = graph.Vertices.Single(v => v.Type == MazeGraph.VertexType.Entrance);
            var journeySoFar = new[] { (node: start, level: 0, numberOfSteps: 0) };
            return Follow(journeySoFar, cache);
        }

        private int Follow((MazeGraph.Vertex node, int level, int numberOfSteps)[] journeySoFar, Dictionary<string, int> cache)
        {
            var key = GetStateKey(journeySoFar);

            var journeyStepsSoFar = journeySoFar.Sum(x => x.numberOfSteps);
            if (journeySoFar.Last().node.Type == MazeGraph.VertexType.Exit)
            {
                return journeyStepsSoFar;
            }
            
            var onwardStepCount = int.MaxValue;
            if (cache.ContainsKey(key))
            {
                onwardStepCount = cache[key];
            }
            else
            {
                var possibleMoves = FindPossibleNextMoves(journeySoFar);
                foreach (var move in possibleMoves)
                {
                    var onwardJourney = journeySoFar.Append((move.node, move.level, move.numberOfSteps));
                    onwardStepCount = Math.Min(onwardStepCount,  Follow(onwardJourney.ToArray(), cache) - journeyStepsSoFar);
                }

                cache[key] = onwardStepCount;
            }

            var totalJourneySteps = (long)journeyStepsSoFar + onwardStepCount;
            return totalJourneySteps > int.MaxValue
                ? int.MaxValue
                : (int)totalJourneySteps;
        }

        private (MazeGraph.Vertex node, int level, int numberOfSteps)[] FindPossibleNextMoves((MazeGraph.Vertex node, int level, int numberOfSteps)[] journeySoFar)
        {
            var visited = journeySoFar.Select(x => (x.level, x.node.Type, x.node.Label)).ToHashSet();
            var current = journeySoFar.Last();
            
            // If the current node is a portal, and the target node wasn't visited yet, we need to follow it.
            if (current.node.Type == MazeGraph.VertexType.PortalAtInnerEdge || current.node.Type == MazeGraph.VertexType.PortalAtOuterEdge)
            {
                var jumpTo = current.node.Edges.Single(e => e.To.Label == current.node.Label);
                var levelAdjustment = current.node.Type == MazeGraph.VertexType.PortalAtInnerEdge ? 1 : -1;
                if (!visited.Contains((current.level + levelAdjustment, jumpTo.To.Type, jumpTo.To.Label)))
                    return new[] { (node: jumpTo.To, level: current.level + levelAdjustment, numberOfSteps: jumpTo.NumberOfSteps) };
            }

            // Start with the adjacent nodes in the graph.
            var candidates = current.node.Edges.Where(e => e.To.Label != current.node.Label)
                                               .Select(e => (node: e.To, current.level, numberOfSteps: e.NumberOfSteps));
            
            // Filter any nodes that aren't valid at the current level.
            var allowedVertexTypes = GetAcceptableOnwardVertexTypesForLevel(current.level);
            candidates = candidates.Where(c => allowedVertexTypes.Contains(c.node.Type));
            
            // Filter out any visited nodes.
            candidates = candidates.Where(c => !visited.Contains((c.level, c.node.Type, c.node.Label)));

            // Done!
            return candidates.OrderBy(x => x.level) // Prefer reducing the recursion level, discourage increasing it.
                             .ThenBy(x => x.numberOfSteps)
                             .ThenBy(x => x.node.Label)
                             .ToArray();
        }

        private MazeGraph.VertexType[] GetAcceptableOnwardVertexTypesForLevel(int level)
        {
            return level == 0
                ? new[] { MazeGraph.VertexType.Entrance, MazeGraph.VertexType.Exit, MazeGraph.VertexType.PortalAtInnerEdge } 
                : level == _maxRecursionLevel
                    ? new[] { MazeGraph.VertexType.PortalAtOuterEdge }
                    : new[] { MazeGraph.VertexType.PortalAtInnerEdge, MazeGraph.VertexType.PortalAtOuterEdge };
        }

        private static string GetStateKey((MazeGraph.Vertex node, int level, int numberOfSteps)[] journey)
        {
            var visitedNodes = journey.OrderBy(x => (x.level, x.node.Label, x.node.Type))
                                      .Select(x => $"{x.level}:{x.node.Type}:{x.node.Label}");
            var current = journey.Last();
            var keyParts = visitedNodes.Append($"{current.level}:{current.node.Type}:{current.node.Label}");
            return string.Join(";", keyParts);
        }
    }
}