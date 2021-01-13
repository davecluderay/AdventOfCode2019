using Aoc2019_Day15.Computer;

namespace Aoc2019_Day15
{
    internal class Solution
    {
        public string Title => "Day 15: Oxygen System";

        public object PartOne()
        {
            var map = new LayoutMap();

            return FillMap(map);
        }

        public object PartTwo()
        {
            var map = new LayoutMap();

            FillMap(map);

            map.Set(map.OxygenSystemPosition ?? default, SpaceType.Oxygenated);

            var minutesPassed = 0;
            while (map.SpaceCount(SpaceType.Empty) > 0)
            {
                minutesPassed++;

                foreach (var space in map.GetSpaces(SpaceType.Oxygenated))
                foreach (var direction in new[] { Direction.North, Direction.South, Direction.West, Direction.East })
                {
                    var vector = direction.ToUnitVector();
                    var position = (space.x + vector.x, space.y + vector.y);
                    if (map.Get(position) != SpaceType.Wall)
                    {
                        map.Set(position, SpaceType.Oxygenated);
                    }
                }
            }

            return minutesPassed;
        }

        private long FillMap(LayoutMap map, ConsoleLayoutMapRenderer? renderer = null)
        {
            var mapper = new LayoutMapper(map, renderer);

            var computer = new IntCodeComputer();
            computer.LoadProgram();

            computer.InputFrom(() => (long)mapper.ChooseNextDirection());
            computer.OutputTo(output => mapper.HandleOutputStatus((DroidStatus) output));
            computer.Run();

            map.FillRemainingSpaces(SpaceType.Wall);

            return mapper.DistanceToOxygenSystem;
        }
    }
}
