using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day18
{
    internal class FeatureGraph
    {
        private readonly Dictionary<MapFeature, Node> _allNodes;
        public int NumberOfKeys => _allNodes.Keys.Count(f => f.Type == FeatureType.Key);
        public MapFeature[] FindEntryPointFeatures() => _allNodes.Keys.Where(f => f.Type == FeatureType.EntryPoint).ToArray();

        private FeatureGraph(IEnumerable<Node> allNodes)
        {
            _allNodes = allNodes.ToDictionary(node => node.Feature, node => node);
        }

        public Node FindNode(MapFeature feature) => _allNodes[feature];

        public Node? FindGateNode(char letter)
            => _allNodes.Values.SingleOrDefault(n => n.Feature.Type == FeatureType.Gate && n.Feature.Letter == letter);

        public static FeatureGraph From(GridMap gridMap)
        {
            var entryPoints = gridMap.StartPositions.Select(p => new Node(new MapFeature(FeatureType.EntryPoint, p, '@')))
                                                    .ToArray();
            var allNodes    = entryPoints.ToDictionary(p => p.Feature, p => p);
            
            var nodesToExamine = new Stack<Node>();
            foreach (var entryPoint in entryPoints)
                nodesToExamine.Push(entryPoint);

            while (nodesToExamine.Count > 0)
            {
                var examineNode = nodesToExamine.Pop();
                foreach (var connection in FindDirectlyAccessibleFeatures(gridMap, examineNode.Feature))
                {
                    Node node;
                    if (allNodes.ContainsKey(connection.feature))
                    {
                        node = allNodes[connection.feature];
                    }
                    else
                    {
                        allNodes[connection.feature] = node = new Node(connection.feature);
                        nodesToExamine.Push(node);
                    }
                    
                    examineNode.Connect(node, connection.numberOfSteps);
                }
            }

            return new FeatureGraph(allNodes.Values);
        }

        public FeatureGraph Copy()
        {
            var copyNodes = new Dictionary<MapFeature, Node>();
            
            foreach (var nodeToProcess in _allNodes.Values)
            {
                var copy = copyNodes.ContainsKey(nodeToProcess.Feature)
                    ? copyNodes[nodeToProcess.Feature]
                    : copyNodes[nodeToProcess.Feature] = new Node(nodeToProcess.Feature);
                foreach (var connection in nodeToProcess.Connections)
                {
                    if (copyNodes.ContainsKey(connection.Node.Feature))
                    {
                        copy.Connect(copyNodes[connection.Node.Feature], connection.NumberOfSteps);
                    }
                }
            }

            return new FeatureGraph(copyNodes.Values);
        }

        public FeatureGraph Without(Node excludeNode)
        {
            var copyGraph = Copy();

            copyGraph.Remove(copyGraph.FindNode(excludeNode.Feature));
            
            return copyGraph;
        }
        
        public void Remove(Node nodeToRemove)
        {
            nodeToRemove = _allNodes[nodeToRemove.Feature];
            foreach (var pair in Combine.IntoAllPossiblePairs(nodeToRemove.Connections.ToList()))
            {
                var numberOfSteps = pair.Item1.NumberOfSteps + pair.Item2.NumberOfSteps;
                pair.Item1.Node.Connect(pair.Item2.Node, numberOfSteps);
            }
            nodeToRemove.DisconnectAll();
            _allNodes.Remove(nodeToRemove.Feature);
        }

        private static (MapFeature feature, int numberOfSteps)[] FindDirectlyAccessibleFeatures(GridMap gridMap, MapFeature startAt)
        {
            var results                = new List<(MapFeature feature, int numberOfSteps)>();
            var visitedPositions       = new HashSet<(int row, int column)>();
            var stepsTaken             = 0;
            var positionsAfterLastStep = new[] { startAt.Position };
            while (true)
            {
                stepsTaken++;
                var newPositions = positionsAfterLastStep.SelectMany(gridMap.AdjacentPositions)
                                                         .Where(p => !visitedPositions.Contains(p))
                                                         .Where(p => p != startAt.Position)
                                                         .Distinct()
                                                         .ToArray();
                if (!newPositions.Any()) break;

                foreach (var newPosition in newPositions)
                {
                    visitedPositions.Add(newPosition);
                    
                    if (gridMap.IsGate(newPosition))
                    {
                        results.Add((new MapFeature(FeatureType.Gate, newPosition, gridMap.At(newPosition)), stepsTaken));
                    }
                    else if (gridMap.IsKey(newPosition))
                    {
                        results.Add((new MapFeature(FeatureType.Key, newPosition, gridMap.At(newPosition)), stepsTaken));
                    }
                    else if (gridMap.IsEntryPoint(newPosition))
                    {
                        results.Add((new MapFeature(FeatureType.EntryPoint, newPosition, gridMap.At(newPosition)), stepsTaken));
                    }
                }
                
                positionsAfterLastStep = newPositions.Where(p => !gridMap.IsWall(p))
                                                     .Where(p => !gridMap.IsGate(p))
                                                     .Where(p => !gridMap.IsKey(p))
                                                     .ToArray();
            }

            return results.ToArray();
        }
        
        internal class Node
        {
            public  MapFeature Feature { get; }
            public  IEnumerable<Connection> Connections => _connections.AsEnumerable();
            private readonly List<Connection> _connections;

            public Node(MapFeature feature)
            {
                Feature = feature;
                _connections = new List<Connection>();
            }

            public override string ToString() => Feature.ToString();

            public void Connect(Node otherNode, int numberOfSteps)
            {
                if (this == otherNode) return;
                var existing = _connections.SingleOrDefault(c => c.Node.Feature == otherNode.Feature);
                if (existing != null)
                {
                    numberOfSteps = Math.Min(numberOfSteps, existing.NumberOfSteps);
                    _connections.Remove(existing);
                    otherNode._connections.Remove(otherNode._connections.Single(c => c.Node == this));
                }
                
                _connections.Add(new Connection(otherNode, numberOfSteps));
                otherNode._connections.Add(new Connection(this, numberOfSteps));
            }

            public void DisconnectAll()
            {
                foreach (var connection in _connections.ToArray())
                {
                    _connections.Remove(connection);
                    connection.Node._connections.Remove(connection.Node._connections.Single(c => c.Node == this));
                }
            }
        }

        internal class Connection
        {
            public Node Node          { get; }
            public int  NumberOfSteps { get; }

            public Connection(Node node, int numberOfSteps)
                => (Node, NumberOfSteps) = (node, numberOfSteps);

            public override string ToString() => $" -{NumberOfSteps}-> {Node}";
        }
    }
}
