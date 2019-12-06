using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day06
{
    internal class Solution
    {
        public string Title => "Day 6: Universal Orbit Map";

        public object PartOne()
        {
            var data = InputFile.ReadAllLines()
                                .Select(l => l.Split(')'))
                                .Select(p => (body: p[0], satellite: p[1]));
            var directOrbits = data.ToDictionary(o => o.satellite, o => o.body);
            
            var checksum = directOrbits.Count;
            foreach (var key in directOrbits.Keys)
            {
                var next = directOrbits[key];
                while (directOrbits.ContainsKey(next))
                {
                    next = directOrbits[next];
                    checksum++;
                }
            }
            
            return checksum;
        }
        
        public object PartTwo()
        {
            var data = InputFile.ReadAllLines()
                                .Select(l => l.Split(')'))
                                .Select(p => (body: p[0], satellite: p[1]));
            var directOrbits = data.ToDictionary(o => o.satellite, o => o.body);

            IEnumerable<string> FindOrbitChain(string satellite)
            {
                var next = directOrbits[satellite];
                yield return next;
                while (directOrbits.ContainsKey(next))
                {
                    yield return next = directOrbits[next];
                }
            }
            
            var youOrbitChain = FindOrbitChain("YOU").ToArray();
            var sanOrbitChain = FindOrbitChain("SAN").ToArray();

            var firstCommonBody = youOrbitChain.First(p => sanOrbitChain.Contains(p));
            var youToCommonBody = youOrbitChain.TakeWhile(p => p != firstCommonBody).Count();
            var sanToCommonBody = sanOrbitChain.TakeWhile(p => p != firstCommonBody).Count();
            
            return youToCommonBody + sanToCommonBody;
        }
    }
}