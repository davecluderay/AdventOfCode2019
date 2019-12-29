using System;

namespace Aoc2019_Day20
{
    internal class Solution
    {
        public string Title => "Day 20: Donut Maze";

        public object PartOne()
        {
            var lines = InputFile.ReadAllLines();
            var map = new GridMap(lines);
            new MapRenderer().Render(map);
            
            var journeyFinder = new PartOneJourneyFinder();
            return journeyFinder.FindShortestJourneyStepCount(map);
        }
        
        public object PartTwo()
        {
            var lines = InputFile.ReadAllLines();

            var map = new GridMap(lines);
            new MapRenderer().Render(map);
            
            var graph = MazeGraph.From(map);

            int maxRecursionLevel = 1;
            while (true)
            {
                var journeyFinder = new PartTwoJourneyFinder(maxRecursionLevel++);
                
                var result = journeyFinder.FindShortestJourneyStepCount(graph);
                if (result < int.MaxValue) return result;
            }
        }
    }
}