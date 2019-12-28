using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day18
{
    internal class GridMap
    {
        private readonly HashSet<(int row, int column)> _wallPositions = new HashSet<(int row, int column)>();
        private readonly HashSet<(int row, int column)> _keyPositions  = new HashSet<(int row, int column)>();
        private readonly HashSet<(int row, int column)> _gatePositions = new HashSet<(int row, int column)>();
        private readonly HashSet<(int row, int column)> _startPositions = new HashSet<(int row, int column)>();
        private readonly char[,] _data;

        public int Columns { get; }
        public int Rows { get; }

        public (int row, int column)[] StartPositions => _startPositions.ToArray();

        public GridMap(string[] lines)
        {
            Rows = lines.Length;
            Columns = lines[0].Length;
            _data = new char[Rows, Columns];

            for (var row = 0; row < Rows; row++)
            for (var column = 0; column < Columns; column++)
            {
                var @char = _data[row, column] = lines[row][column];
                if (@char == '@')
                    _startPositions.Add((row, column));
                if (@char == '#')
                    _wallPositions.Add((row, column));
                else if (char.IsLower(@char))
                    _keyPositions.Add((row, column));
                else if (char.IsUpper(@char))
                    _gatePositions.Add((row, column));
            }
        }

        public char At((int row, int column)     position) => _data[position.row, position.column];
        public bool IsWall((int row, int column) position) => _wallPositions.Contains(position);
        public bool IsKey((int row, int column)  position) => _keyPositions.Contains(position);
        public bool IsGate((int row, int column) position) => _gatePositions.Contains(position);
        public bool IsEntryPoint((int row, int column) position) => _startPositions.Contains(position);
        
        public IEnumerable<(int x, int y)> AdjacentPositions((int row, int column) to)
        {
            var vectors = new (int x, int y)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            foreach (var pos in vectors.Select(v => (row: to.row + v.x, column: to.column + v.y)))
            {
                if (pos.row >= 0 && pos.column >= 0 && pos.row < Rows && pos.column < Columns)
                    yield return pos;
            }
        }

        public void IsolateQuadrants()
        {
            SetWallAt((39, 40));
            SetWallAt((40, 40));
            SetWallAt((41, 40));
            SetWallAt((40, 39));
            SetWallAt((40, 41));
            SetStartPositionAt((39, 39));
            SetStartPositionAt((41, 39));
            SetStartPositionAt((39, 41));
            SetStartPositionAt((41, 41));
        }

        private void SetWallAt((int row, int column) position)
        {
            _data[position.row, position.column] = '#';
            _startPositions.Remove(position);
            _gatePositions.Remove(position);
            _keyPositions.Remove(position);
            _wallPositions.Add(position);
        }

        private void SetStartPositionAt((int row, int column) position)
        {
            _data[position.row, position.column] = '@';
            _startPositions.Add(position);
            _gatePositions.Remove(position);
            _keyPositions.Remove(position);
            _wallPositions.Remove(position);
        }
    }
}