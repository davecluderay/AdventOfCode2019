namespace Aoc2019_Day15
{
    internal enum Direction
    {
        Unknown = 0,
        North   = 1,
        South   = 2,
        West    = 3,
        East    = 4
    }

    internal static class DirectionExtensions
    {
        public static bool IsVertical(this Direction direction)
            => direction == Direction.North || direction == Direction.South;
        
        public static bool IsHorizontal(this Direction direction)
            => direction == Direction.West || direction == Direction.East;

        public static Direction Reverse(this Direction direction)
        {
            return direction switch
            {
                Direction.North => Direction.South,
                Direction.South => Direction.North,
                Direction.West  => Direction.East,
                Direction.East  => Direction.West,
                _               => Direction.Unknown
            };
        }

        public static (int x, int y) ToUnitVector(this Direction direction)
        {
            return direction switch
            {
                Direction.North => (0, -1),
                Direction.South => (0, 1),
                Direction.West  => (-1, 0),
                Direction.East  => (1, 0),
                _               => (0, 0)
            };
        }
    }
}