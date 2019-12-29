using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day20
{
    internal class PartOneJourneyFinder
    {
        public int FindShortestJourneyStepCount(GridMap map)
        {
            var stepsTaken = 0;
            
            var allVisitedPositions  = new HashSet<(int row, int column)>();
            var lastVisitedPositions = new HashSet<(int row, int column)> { map.EntryPosition };
            while (lastVisitedPositions.Count > 0)
            {
                var nextPositions = lastVisitedPositions.SelectMany(map.AdjacentPositions)
                                                        .Where(map.IsOpen)
                                                        .Where(p => !allVisitedPositions.Contains(p))
                                                        .ToHashSet();
                stepsTaken++;

                if (nextPositions.Contains(map.ExitPosition))
                    break;

                foreach (var next in nextPositions)
                    allVisitedPositions.Add(next);
                lastVisitedPositions = nextPositions;
            }
            
            return stepsTaken;
        }
    }
}