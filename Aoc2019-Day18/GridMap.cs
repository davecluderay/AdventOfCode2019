using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Aoc2019_Day18
{
    internal class GridMap
    {
        private readonly HashSet<(int row, int column)> _wallPositions = new HashSet<(int row, int column)>();
        private readonly HashSet<(int row, int column)> _keyPositions  = new HashSet<(int row, int column)>();
        private readonly HashSet<(int row, int column)> _gatePositions = new HashSet<(int row, int column)>();
        private readonly char[,] _data;

        public int Columns { get; }
        public int Rows { get; }
        public (int Row, int Column) StartPosition { get; }

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
                    StartPosition = (row, column);
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
        public bool IsEntryPoint((int row, int column) position) => StartPosition == position;
        
        public IEnumerable<(int x, int y)> AdjacentPositions((int row, int column) to)
        {
            var vectors = new (int x, int y)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            foreach (var pos in vectors.Select(v => (row: to.row + v.x, column: to.column + v.y)))
            {
                if (pos.row >= 0 && pos.column >= 0 && pos.row < Rows && pos.column < Columns)
                    yield return pos;
            }
        }
    }
}