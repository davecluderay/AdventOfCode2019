using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day13
{
    internal class ScreenSnapshot
    {
        public IDictionary<(int x, int y), TileType> Tiles { get; }
        public long Score { get; }

        public ScreenSnapshot(IDictionary<(int x, int y), TileType> tiles, long score)
        {
            Tiles = tiles;
            Score = score;
        }
    }
    internal class ConsoleScreenBuffer
    {
        public delegate void ChangedHandler((int x, int y) position, TileType tile);
        public delegate void ScoreChangedHandler(long score);

        public event ChangedHandler Changed = delegate { };
        public event ScoreChangedHandler ScoreChanged = delegate { };

        private readonly IDictionary<(int x, int y), TileType> _tiles = new Dictionary<(int x, int y), TileType>();
        private long _score;

        public TileType Get((int x, int y) position) => _tiles.ContainsKey(position)
                                                            ? _tiles[position]
                                                            : TileType.Empty;

        public (int x, int y) FindPaddlePosition(int x, int y)
            => _tiles.First(t => t.Value == TileType.Paddle).Key;

        public long GetScore() => _score;

        public void Set((int x, int y) position, TileType tile)
        {
            _tiles[position] = tile;
            Changed(position, tile);
        }

        public void SetScore(long score)
        {
            _score = score;
            ScoreChanged(score);
        }

        public int TileCount(TileType tile) => _tiles.Values.Count(t => t == tile);

        public (int left, int top, int right, int bottom) Bounds => (_tiles.Keys.Min(p => p.x),
                                                                     _tiles.Keys.Min(p => p.y),
                                                                     _tiles.Keys.Max(p => p.x),
                                                                     _tiles.Keys.Max(p => p.y));

        public ScreenSnapshot TakeSnapshot()
        {
            return new ScreenSnapshot(new Dictionary<(int x, int y), TileType>(_tiles), _score);
        }

        public void ApplySnapshot(ScreenSnapshot snapshot)
        {
            foreach (var update in snapshot.Tiles.Where(st => st.Value != Get(st.Key)))
                Set(update.Key, update.Value);
            SetScore(snapshot.Score);
        }
    }
}
