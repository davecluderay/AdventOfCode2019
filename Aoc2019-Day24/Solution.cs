using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day24
{
    internal class Solution
    {
        public string Title => "Day 24: Planet of Discord";

        public object PartOne()
        {
            var layout = new Layout(InputFile.ReadAllLines());

            var biodiversityRatingsSoFar = new HashSet<int>();
            while (true)
            {
                var bugPositions = new HashSet<(int row, int column)>();
                foreach (var position in layout.AllPositions)
                {
                    if (layout.IsBugAt(position) && layout.AdjacentBugCount(position) == 1)
                    {
                        bugPositions.Add(position);
                    }
                    else if (layout.IsEmptySpaceAt(position))
                    {
                        var adjacentBugCount = layout.AdjacentBugCount(position);
                        if (adjacentBugCount > 0 && adjacentBugCount < 3)
                            bugPositions.Add(position);
                    }
                }

                foreach (var position in layout.AllPositions)
                {
                    if (bugPositions.Contains(position))
                        layout.SetBugAt(position);
                    else
                        layout.SetEmptySpaceAt(position);
                }

                var biodiversityRating = layout.CalculateBiodiversityRating();
                if (biodiversityRatingsSoFar.Contains(biodiversityRating))
                {
                    return biodiversityRating;
                }

                biodiversityRatingsSoFar.Add(biodiversityRating);
            }
        }
        
        public object PartTwo()
        {
            var layout = new RecursiveLayout(InputFile.ReadAllLines());

            const int runForMinutes = 200;
            for (var minute = 1; minute <= runForMinutes; minute++)
            {
                var bugPositions = new HashSet<(RecursiveLayout level, (int row, int column) position)>();

                var positionsToExamine = layout.FindBugsAtAllLevels().ToHashSet();
                foreach (var p in positionsToExamine.SelectMany(p => p.level.GetAdjacentPositions(p.position))
                                                    .ToList())
                {
                    positionsToExamine.Add(p);
                }

                foreach (var p in positionsToExamine)
                {
                    if (p.level.IsBugAt(p.position) && p.level.AdjacentBugCount(p.position) == 1)
                    {
                        bugPositions.Add(p);
                    }
                    else if (p.level.IsEmptySpaceAt(p.position))
                    {
                        var adjacentBugCount = p.level.AdjacentBugCount(p.position);
                        if (adjacentBugCount > 0 && adjacentBugCount < 3)
                            bugPositions.Add(p);
                    }
                }

                foreach (var p in positionsToExamine)
                {
                    if (bugPositions.Contains(p))
                        p.level.SetBugAt(p.position);
                    else
                        p.level.SetEmptySpaceAt(p.position);
                }
            }

            return layout.FindBugsAtAllLevels()
                         .Count();
        }
    }
}
