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
            var maxRecursionLevel = 25;

            var map = new GridMap(lines);
            new MapRenderer().Render(map);
            
            var graph = MazeGraph.From(map);

            var journeyFinder = new PartTwoJourneyFinder(maxRecursionLevel);
            return journeyFinder.FindShortestJourneyStepCount(graph);
        }
    }
}