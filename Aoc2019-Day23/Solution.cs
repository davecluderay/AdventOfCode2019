using System.Collections.Generic;

namespace Aoc2019_Day23
{
    internal class Solution
    {
        public string Title => "Day 23: Category Six";

        public object? PartOne()
        {
            var network = CreateNetwork();

            NetworkPacket? packet = null;
            network.PacketGenerated += p => packet = p.To == NatDevice.Address ? p : packet;

            while (packet == null)
                network.Step();

            return packet?.Y;
        }

        public object PartTwo()
        {

            var network = CreateNetwork();

            HashSet<long> yValuesSoFar = new HashSet<long>();
            long? result = null;
            network.PacketGenerated += p =>
                                       {
                                           if (p.To != 0L) return;
                                           if (yValuesSoFar.Contains(p.Y))
                                               result = p.Y;
                                           else
                                               yValuesSoFar.Add(p.Y);
                                       };

            while (result is null)
                network.Step();

            return result;
        }

        private static Network CreateNetwork()
        {
            var network = new Network();
            network.AddNics(50);
            network.AddNat();
            return network;
        }
    }
}
