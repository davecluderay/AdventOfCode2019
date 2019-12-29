using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day20
{
    internal class MazeGraph
    {
        private readonly List<Vertex> _vertices;
        public IReadOnlyCollection<Vertex> Vertices => _vertices.AsReadOnly();

        private MazeGraph(IEnumerable<Vertex> vertices)
        {
            _vertices = vertices.OrderBy(v => v.Type).ThenBy(v => v.Label).ToList();
        }

        public static MazeGraph From(GridMap map)
        {
            var allVertices = new Dictionary<(int row, int column), Vertex>();
            
            // Discover all vertices.
            for (var row = 0; row < map.Rows; row++)
            for (var column = 0; column < map.Columns; column++)
            {
                var position = (row, column);
                if (map.IsEntrance(position))
                {
                    allVertices[position] = new Vertex(VertexType.Entrance, map.GetLabelAt(position));
                }
                else if (map.IsExit(position))
                {
                    allVertices[position] = new Vertex(VertexType.Exit, map.GetLabelAt(position));
                }
                else if (map.IsPortal(position))
                {
                    var vertexType = map.IsOnOuterEdge(position)
                        ? VertexType.PortalAtOuterEdge
                        : VertexType.PortalAtInnerEdge;
                    allVertices[position] = new Vertex(vertexType, map.GetLabelAt(position));
                }
            }

            foreach (var position in allVertices.Keys)
            {
                // Find all immediately connected vertices (and fewest steps).
                var stepsTaken = 0;
                var allVisitedPositions = new HashSet<(int row, int column)> { position };
                var lastVisitedPositions = new HashSet<(int row, int column)> { position };
                while (lastVisitedPositions.Count > 0)
                {
                    var nextPositions = lastVisitedPositions.SelectMany(map.AdjacentPositions)
                                                            .Where(map.IsOpen)
                                                            .Where(p => !allVisitedPositions.Contains(p))
                                                            .ToHashSet();
                    stepsTaken++;

                    lastVisitedPositions.Clear();
                    foreach (var next in nextPositions)
                    {
                        if (allVertices.ContainsKey(next))
                        {
                            allVertices[position].ConnectTo(allVertices[next], stepsTaken);
                        }
                        else
                        {
                            lastVisitedPositions.Add(next);
                        }
                        
                        allVisitedPositions.Add(next);
                    }
                }
            }
            
            return new MazeGraph(allVertices.Values);
        }
        
        public class Vertex
        {
            private readonly List<Edge> _edges = new List<Edge>();
            public VertexType Type { get; }
            public string Label { get; }
            public IReadOnlyCollection<Edge> Edges => _edges.AsReadOnly();

            public Vertex(VertexType type, string label)
            {
                Type = type;
                Label = label;
            }

            public void ConnectTo(Vertex other, int numberOfSteps)
            {
                // If the edge already exists, but has a higher cost, we will replace it.
                var existingEdge = _edges.SingleOrDefault(e => e.To == other);
                if (existingEdge != null)
                {
                    numberOfSteps = Math.Min(numberOfSteps, existingEdge.NumberOfSteps);
                    DisconnectFrom(other);
                }
                
                _edges.Add(new Edge(other, numberOfSteps));
                other._edges.Add(new Edge(this, numberOfSteps));
            }

            private void DisconnectFrom(Vertex other)
            {
                foreach (var edge in _edges.Where(e => e.To == other).ToList())
                {
                    edge.To._edges.RemoveAll(e => e.To == this);
                    _edges.Remove(edge);
                }
            }

            public override string ToString() =>
                Type switch
                {
                    VertexType.Entrance          => $"{Label} (entrance)",
                    VertexType.Exit              => $"{Label} (exit)",
                    VertexType.PortalAtInnerEdge => $"{Label} (inner)",
                    VertexType.PortalAtOuterEdge => $"{Label} (outer)",
                    _                            => base.ToString()
                };
        }

        public class Edge
        {
            public Vertex To { get; }
            public int NumberOfSteps { get; }

            public Edge(Vertex to, int numberOfSteps)
            {
                To = to;
                NumberOfSteps = numberOfSteps;
            }

            public override string ToString() => $" -{NumberOfSteps}-> {To}";
        }
        
        public enum VertexType
        {
            Entrance,
            Exit,
            PortalAtInnerEdge,
            PortalAtOuterEdge
        }
    }
}