using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Aoc2019_Day15
{
    internal class LayoutMap
    {
        public (int x, int y) CurrentDroidPosition { get; private set; }
        public (int x, int y)? OxygenSystemPosition { get; set; }
        private IDictionary<(int x, int y), SpaceType> Spaces { get; }

        public LayoutMap()
        {
            CurrentDroidPosition = (0, 0);
            Spaces = new Dictionary<(int x, int y), SpaceType>
                     {
                         { (0, 0), SpaceType.Droid }
                     };
        }

        public void MoveDroid(Direction direction)
        {
            var nextPosition = CalculateAdjacentPosition(direction);
            Set(CurrentDroidPosition, SpaceType.Empty);
            Set(nextPosition, SpaceType.Droid);
            CurrentDroidPosition = nextPosition;
        }

        public void MarkWall(Direction direction)
        {
            var nextPosition = CalculateAdjacentPosition(direction);
            Set(nextPosition, SpaceType.Wall);
        }

        public (int x, int y) CalculateAdjacentPosition(Direction direction)
        {
            var vector = direction.ToUnitVector();
            return (CurrentDroidPosition.x + vector.x, CurrentDroidPosition.y + vector.y);
        }
        
        public SpaceType Get((int x, int y) position) => Spaces.ContainsKey(position) ? Spaces[position] : SpaceType.Unexplored;
        
        public void Set((int x, int y) position, SpaceType color) => Spaces[(position)] = color;

        public void FillRemainingSpaces(SpaceType spaceType)
        {
            var bounds = Bounds;
            for (var y = bounds.top; y <= bounds.bottom; y++)
            for (var x = bounds.left; x <= bounds.right; x++)
            {
                if (Get((x, y)) == SpaceType.Unexplored)
                    Set((x, y), spaceType);
            }
        }

        public int SpaceCount(SpaceType spaceType)
            => Spaces.Values.Count(s => s == spaceType);
        
        public IEnumerable<(int x, int y)> GetSpaces(SpaceType spaceType)
            => Spaces.Where(s => s.Value == spaceType)
                     .Select(s => s.Key)
                     .ToArray();

        public (int left, int top, int right, int bottom) Bounds => (Spaces.Keys.Min(p => p.x),
                                                                     Spaces.Keys.Min(p => p.y),
                                                                     Spaces.Keys.Max(p => p.x),
                                                                     Spaces.Keys.Max(p => p.y));
    }
}