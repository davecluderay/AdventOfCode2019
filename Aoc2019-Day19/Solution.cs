namespace Aoc2019_Day19
{
    internal class Solution
    {
        public string Title => "Day 19: Tractor Beam";

        public object PartOne()
        {
            var buffer = new BeamScanner().ScanBeam(origin: (0, 0),
                                                    dimensions: (50, 50));

            new ConsoleBeamRenderer().RenderBeam(buffer);

            var affectedPoints = 0;
            foreach (var value in buffer)
                if (value == 1)
                    affectedPoints++;

            return affectedPoints;
        }

        public object? PartTwo()
        {
            var origin = FindOriginOfNearestSquareToFitBeam(startAtY: 1280);

            var buffer = new BeamScanner().ScanBeam(origin: (origin.x - 10, origin.y - 10),
                                                    dimensions: (120, 120),
                                                    excludeRegion: (origin.x, origin.y, origin.x + 99, origin.y + 99));
            new ConsoleBeamRenderer().RenderBeam(buffer,
                                                 objectAt: (10, 10, 109, 109));

            return origin.x * 10_000 + origin.y;

            (int x, int y) FindOriginOfNearestSquareToFitBeam(int startAtY)
            {
                var scanner = new BeamScanner();
                var lastExtent = (x: 0, length: 0);
                var y          = startAtY;
                while (true)
                {
                    var extent = scanner.GetBeamHorizontalExtent(atY: y, startAtX: lastExtent.x);
                    if (extent.length >= 100)
                    {
                        var extentAfter100 = scanner.GetBeamHorizontalExtent(atY: y + 99, extent.x);
                        var overlap = CalculateOverlap(extent, extentAfter100);

                        if (overlap.length >= 100)
                        {
                            return (overlap.x, y);
                        }
                    }

                    lastExtent = extent;
                    y++;
                }
            }

            (int x, int length) CalculateOverlap((int x, int length) extent1, (int x, int length) extent2)
            {
                var (left, right) = extent1.x < extent2.x ? (extent1, extent2) : (extent2, extent1);
                var overlapRight = left.x + left.length - 1;
                var overlapLeft = right.x;
                return (x: overlapLeft, length: overlapRight - overlapLeft + 1);
            }
        }
    }
}
