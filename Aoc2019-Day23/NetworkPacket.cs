namespace Aoc2019_Day23
{
    internal class NetworkPacket
    {
        public int From { get; }
        public int To { get; }
        public long X { get; }
        public long Y { get; }

        public NetworkPacket(int from, int to, long x, long y)
            => (From, To, X, Y) = (from, to, x, y);

        public NetworkPacket(int from, long[] data)
            => (From, To, X, Y) = (from, (int) data[0], data[1], data[2]);

        public override string ToString()
            => $"{To:D2}: (X = {X}, Y = {Y})";
    }
}