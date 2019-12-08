using System;
using System.Linq;

namespace Aoc2019_Day08
{
    internal class Solution
    {
        public string Title => "Day 8: Space Image Format";

        public object PartOne()
        {
            var image = SpaceImage.ReadImage(25, 6);
            
            var layer = image.GetRawLayerData()
                             .OrderBy(l => l.Count(pixel => (int)pixel == 0))
                             .First();
            var checksum = layer.Count(pixel => (int)pixel == 1) *
                           layer.Count(pixel => (int)pixel == 2);
            return checksum;
        }

        public object PartTwo()
        {
            var image = SpaceImage.ReadImage(25, 6);

            ConsoleSpaceImageRenderer.RenderImage(image);
            Console.WriteLine();

            return "See console output!";
        }
    }
}