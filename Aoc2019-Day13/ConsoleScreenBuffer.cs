using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day13
{
    internal class ConsoleScreenBuffer
    {
        private readonly IDictionary<(int x, int y), TileType> _tiles = new Dictionary<(int x, int y), TileType>();

        public TileType Get((int x, int y) position) => _tiles.ContainsKey(position)
            ? _tiles[position]
            : TileType.Empty;

        public void Set((int x, int y) position, TileType tile) => _tiles[position] = tile;

        public int TileCount(TileType tile) => _tiles.Values.Count(t => t == tile);

        public (int left, int top, int right, int bottom) Bounds => (_tiles.Keys.Min(p => p.x),
                                                                     _tiles.Keys.Min(p => p.y),
                                                                     _tiles.Keys.Max(p => p.x),
                                                                     _tiles.Keys.Max(p => p.y));
    }
}