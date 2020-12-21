using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day15
{
    internal class LayoutMapper
    {
        private static readonly Direction[] Directions = Enum.GetValues(typeof(Direction))
                                                             .Cast<Direction>()
                                                             .Where(v => v != Direction.Unknown)
                                                             .ToArray();

        private readonly LayoutMap _map;
        private readonly ConsoleLayoutMapRenderer? _renderer;
        private readonly Stack<Direction> _breadcrumbs = new Stack<Direction>();

        private Direction _currentDirection;
        private bool _currentlyBacktracking;

        public int DistanceToOxygenSystem { get; private set; }

        public LayoutMapper(LayoutMap map, ConsoleLayoutMapRenderer? renderer = null)
        {
            _map = map;
            _renderer = renderer;
        }

        public void HandleOutputStatus(DroidStatus status)
        {
            var initialBounds   = _map.Bounds;
            var renderPositions = new List<(int x, int y)> { _map.CurrentDroidPosition };

            switch (status)
            {
                case DroidStatus.BlockedByWall:
                    _map.MarkWall(_currentDirection);
                    renderPositions.Add(_map.CalculateAdjacentPosition(_currentDirection));
                    break;
                case DroidStatus.Moved:
                    if (!_currentlyBacktracking) _breadcrumbs.Push(_currentDirection.Reverse());
                    _map.MoveDroid(_currentDirection);
                    renderPositions.Add(_map.CurrentDroidPosition);
                    break;
                case DroidStatus.AtOxygenSystem:
                    if (!_currentlyBacktracking) _breadcrumbs.Push(_currentDirection.Reverse());
                    _map.MoveDroid(_currentDirection);
                    renderPositions.Add(_map.CurrentDroidPosition);
                    _map.OxygenSystemPosition = _map.CurrentDroidPosition;
                    DistanceToOxygenSystem = _breadcrumbs.Count;
                    break;
            }

            if (_map.Bounds != initialBounds)
                _renderer?.Render();
            else
                _renderer?.Render(renderPositions);
        }

        public Direction ChooseNextDirection()
        {
            var chosenDirection = Direction.Unknown;
            var backtracking = false;

            // Choose an unexplored direction if possible.
            var exploreDirections = Directions.Where(d =>_map.Get(_map.CalculateAdjacentPosition(d)) == SpaceType.Unexplored)
                                              .ToArray();
            if (exploreDirections.Any())
            {
                chosenDirection = exploreDirections.First();
            }

            // If all directions from the current position are explored, backtrack.
            if (chosenDirection == Direction.Unknown && _breadcrumbs.Any())
            {
                chosenDirection = _breadcrumbs.Pop();
                backtracking = true;
            }

            _currentDirection = chosenDirection;
            _currentlyBacktracking = backtracking;

            return _currentDirection;
        }
    }
}
