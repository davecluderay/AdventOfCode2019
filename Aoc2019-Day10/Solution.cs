namespace Aoc2019_Day10
{
    internal class Solution
    {
        public string Title => "Day 10: Monitoring Station";

        public object PartOne()
        {
            var asteroids = LoadAsteroidPositions();
            return SelectMonitoringStationAsteroid(asteroids).NumberOfVisibleAsteroids;
        }

        public object PartTwo()
        {
            var asteroids = LoadAsteroidPositions();
            var station = SelectMonitoringStationAsteroid(asteroids).Position;
            var vaporized = Vaporize(station, asteroids);
            var number200 = vaporized.Skip(199).First();
            return number200.X * 100 + number200.Y;
        }

        private static (Position Position, int NumberOfVisibleAsteroids) SelectMonitoringStationAsteroid(IReadOnlyCollection<Position> asteroids)
        {
            return asteroids.Select(a => (Position: a,
                                          AngleCount: asteroids.Where(x => x != a)
                                                               .GroupBy(x => CalculateAngle(a, x))
                                                               .Count()))
                            .MaxBy(g => g.AngleCount)!;
        }

        private static IEnumerable<Position> Vaporize(Position station, IReadOnlyCollection<Position> asteroids)
        {
            var orientations = asteroids.Where(a => a != station)
                                        .Select(a => (Position: a,
                                                      Angle: CalculateAngle(station, a),
                                                      Distance: CalculateDistance(station, a)))
                                        .GroupBy(x => x.Angle)
                                        .Select(g => (Angle: g.Key,
                                                      RemainingAsteroids: new Queue<Position>(g.OrderBy(a => a.Distance)
                                                                                               .Select(a => a.Position))))
                                        .OrderBy(x => x.Angle)
                                        .ToList();

            while (orientations.Count > 0)
            {
                foreach (var orientation in orientations)
                {
                    if (orientation.RemainingAsteroids.Count > 0)
                    {
                        yield return orientation.RemainingAsteroids.Dequeue();
                    }
                }

                orientations.RemoveAll(o => o.RemainingAsteroids.Count == 0);
            }
        }

        private static double CalculateAngle(Position p1, Position p2)
        {
            var angle = Math.Atan2(p2.X - p1.X, p1.Y - p2.Y);
            if (angle < 0) angle += 2 * Math.PI;
            return Math.Round(angle, 3);
        }

        private static double CalculateDistance(Position p1, Position p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        private static IReadOnlyList<Position> LoadAsteroidPositions(string? fileName = null)
        {
            return File.ReadAllLines(fileName ?? "./input.txt")
                       .SelectMany((row, y) => row.Select((@char, x) => (point: new Position(x, y), @char)))
                       .Where(x => x.@char == '#')
                       .Select(x => x.point)
                       .ToList()
                       .AsReadOnly();
        }
    }

    internal record struct Position(int X, int Y)
    {
        public static implicit operator Position((int X, int Y) position)
        {
            return new(position.X, position.Y);
        }
    }
}
