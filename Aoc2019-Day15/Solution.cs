using Aoc2019_Day15.Computer;

namespace Aoc2019_Day15
{
    internal class Solution
    {
        public string Title => "Day 15: Oxygen System";

        public object PartOne()
        {
            var map = new LayoutMap();
            
            FillMap(map);
            
            return map.OxygenSystemPosition;
        }
        
        public object PartTwo()
        {
            var map = new LayoutMap();
            using var renderer = new ConsoleLayoutMapRenderer(map);
            
            FillMap(map, renderer);
            
            map.Set(map.OxygenSystemPosition ?? default, SpaceType.Oxygenated);
            renderer?.Render();
            
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
                        renderer?.Render(position);
                    } 
                }
            }
            
            renderer?.Render();
            
            return minutesPassed;
        }

        private void FillMap(LayoutMap map, ConsoleLayoutMapRenderer renderer = null)
        {
            var mapper = new LayoutMapper(map, renderer);
            
            var computer = new IntCodeComputer();
            computer.LoadProgram();
            
            foreach (var output in computer.RunProgram(() => (long)mapper.ChooseNextDirection()))
            {
                mapper.HandleOutputStatus((DroidStatus) output);
            }

            map.FillRemainingSpaces(SpaceType.Wall);
        }
    }
}