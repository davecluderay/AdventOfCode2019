using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day18
{
    internal class JourneyFinder
    {
        public int FindShortestJourneyStepCount(FeatureGraph featureGraph)
        {
            var cachedFewestSteps = new Dictionary<string, int>();
            var numberOfKeys = featureGraph.NumberOfKeys;
            return Follow(featureGraph.Copy(),
                          new (MapFeature, int numberOfSteps)[] { (featureGraph.FindEntryPointFeature(), 0) });

            int Follow(FeatureGraph intermediateGraph, (MapFeature feature, int numberOfSteps)[] journeySoFar)
            {
                var keysFound = journeySoFar.Count(x => x.feature.Type == FeatureType.Key);
                if (keysFound < numberOfKeys)
                {
                    var currentNode     = intermediateGraph.FindNode(journeySoFar.Last().feature);
                    var visitedFeatures = journeySoFar.Select(n => n.feature).ToHashSet();
                    var journeySteps    = int.MaxValue;
                    
                    if (currentNode.Feature.Type == FeatureType.Key)
                    {
                        var removeGateNode = intermediateGraph.FindGateNode(currentNode.Feature.Letter);
                        if (removeGateNode != null)
                            intermediateGraph.Remove(removeGateNode);
                    }

                    var state = new string(journeySoFar.Select(x => x.feature.Letter).OrderBy(l => l).Append('-').Append(journeySoFar.Last().feature.Letter).ToArray());
                    if (cachedFewestSteps.ContainsKey(state))
                    {
                        return cachedFewestSteps[state] + journeySoFar.Sum(n => n.numberOfSteps);
                    }
                    
                    foreach (var next in FindAccessibleKeyNodes(currentNode, visitedFeatures))
                    {
                        var copyGraph = intermediateGraph.Without(currentNode);
                        journeySteps = Math.Min(journeySteps, Follow(copyGraph, journeySoFar.Append((next.node.Feature, next.numberOfSetps)).ToArray()));
                        cachedFewestSteps[state] = journeySteps - journeySoFar.Sum(n => n.numberOfSteps);
                    }
                    return journeySteps;
                }

                return journeySoFar.Sum(n => n.numberOfSteps);
            }
        }

        private static (FeatureGraph.Node node, int numberOfSetps)[] FindAccessibleKeyNodes(FeatureGraph.Node startAt, HashSet<MapFeature> previouslyVisited)
        {
            var results = new List<(FeatureGraph.Node node, int numberOfSetps)>();

            var examined = new HashSet<MapFeature> { startAt.Feature };

            var lastExamined = new[] { (node: startAt, numberOfSteps: 0) };
            while (true)
            {
                var nextToExamine = lastExamined.SelectMany(x => x.node.Connections.Select(c => (node: c.Node, numberOfSteps: x.numberOfSteps + c.NumberOfSteps )))
                                                .Where(x => !examined.Contains(x.node.Feature))
                                                .ToArray();
                if (!nextToExamine.Any()) break;

                var stopAtFeatures = new List<MapFeature>();
                foreach (var examine in nextToExamine)
                {
                    examined.Add(examine.node.Feature);
                    
                    var feature = examine.node.Feature;
                    if (feature.Type == FeatureType.Key)
                    {
                        if (!previouslyVisited.Contains(examine.node.Feature))
                        {
                            results.Add(examine);
                            stopAtFeatures.Add(examine.node.Feature);
                        }
                    }
                    else
                    {
                        stopAtFeatures.Add(examine.node.Feature);
                    }
                }

                lastExamined = nextToExamine.Where(x => !stopAtFeatures.Contains(x.node.Feature))
                                            .ToArray();
            }

            return results.OrderBy(r => r.numberOfSetps)
                          .ToArray();
        }
    }
}