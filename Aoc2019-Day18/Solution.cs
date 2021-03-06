namespace Aoc2019_Day18
{
    internal class Solution
    {
        public string Title => "Day 18: Many-Worlds Interpretation";

        public object PartOne()
        {
            var map = new GridMap(InputFile.ReadAllLines());

            var featureGraph = FeatureGraph.From(map);
            var journeyFinder = new JourneyFinder();
            var numberOfSteps = journeyFinder.FindShortestJourneyStepCount(featureGraph);
            return numberOfSteps;
        }
        
        public object PartTwo()
        {
            var map = new GridMap(InputFile.ReadAllLines());
            map.IsolateQuadrants();
            
            var featureGraph  = FeatureGraph.From(map);
            var journeyFinder = new JourneyFinder();
            var numberOfSteps = journeyFinder.FindShortestJourneyStepCount(featureGraph);
            return numberOfSteps;
        }
    }
}
