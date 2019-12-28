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
            var totalNumberOfKeys = featureGraph.NumberOfKeys;
            return Follow(featureGraph.Copy(),
                          featureGraph.FindEntryPointFeatures()
                                      .Select((f, i) => (RowNotInTableException: i, feature: f, numberOfSteps: 0))
                                      .ToArray());
            
            int Follow(FeatureGraph intermediateGraph, (int robot, MapFeature feature, int numberOfSteps)[] journeySoFar)
            {
                // If we have found all keys, we are done.
                var numberOfKeysFound = journeySoFar.Count(x => x.feature.Type == FeatureType.Key);
                if (numberOfKeysFound == totalNumberOfKeys)
                    return journeySoFar.Sum(x => x.numberOfSteps);

                // Based on the current state of all journeys, have we already cached the fewest steps to completion?
                var currentPositions = journeySoFar.GroupBy(x => x.robot)
                                                   .Select(g => (robot: g.Key, lastPosition: g.Last().feature))
                                                   .OrderBy(x => x.robot)
                                                   .ToArray();
                var foundKeys = journeySoFar.Where(x => x.feature.Type == FeatureType.Key)
                                            .Select(x => x.feature.Letter)
                                            .OrderBy(k => k)
                                            .ToArray();
                var state = new string(foundKeys) + ";" + string.Join(",", currentPositions.Select(p => $"{p.robot}:{p.lastPosition.Letter}").ToArray());
                if (cachedFewestSteps.ContainsKey(state))
                {
                    return cachedFewestSteps[state] + journeySoFar.Sum(n => n.numberOfSteps);
                }
                
                // Remove the corresponding gates for any keys that were collected on the last iteration.
                foreach (var keyPosition in currentPositions.Where(p => p.lastPosition.Type == FeatureType.Key))
                {   
                    var removeGateNode = intermediateGraph.FindGateNode(keyPosition.lastPosition.Letter);
                    if (removeGateNode != null)
                        intermediateGraph.Remove(removeGateNode);
                }
                
                // Follow all possible sequences from this point.
                var visitedFeatures = journeySoFar.Select(n => n.feature).ToHashSet();
                var nextMoves = currentPositions.SelectMany(p =>
                                                         {
                                                             var fromNode = intermediateGraph.FindNode(p.lastPosition);
                                                             return FindAccessibleKeyNodes(fromNode, visitedFeatures)
                                                                 .Select(x => (p.robot, fromNode, toNode: x.node, x.numberOfSteps));
                                                         });
                var journeySteps = int.MaxValue;
                foreach (var nextMove in nextMoves)
                {
                    var copyGraph = intermediateGraph.Without(nextMove.fromNode);
                    var copyJourney = journeySoFar.Append((nextMove.robot, feature: nextMove.toNode.Feature, nextMove.numberOfSteps))
                                                  .ToArray();
                    journeySteps = Math.Min(journeySteps, Follow(copyGraph, copyJourney));
                    cachedFewestSteps[state] = journeySteps - journeySoFar.Sum(n => n.numberOfSteps);
                }
                return journeySteps; 
            }
        }

        private static (FeatureGraph.Node node, int numberOfSteps)[] FindAccessibleKeyNodes(FeatureGraph.Node startAt, HashSet<MapFeature> previouslyVisited)
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