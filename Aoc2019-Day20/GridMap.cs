using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day20
{
    internal class GridMap
    {
        private readonly HashSet<(int row, int column)> _wallPositions = new HashSet<(int row, int column)>();
        private readonly (int row, int column) _entryPosition;
        private readonly (int row, int column) _exitPosition;
        private readonly Dictionary<(int row, int column), string> _innerEdgePortals = new Dictionary<(int row, int column), string>();
        private readonly Dictionary<(int row, int column), string> _outerEdgePortals = new Dictionary<(int row, int column), string>();
        private readonly char[,] _data;

        public int Columns { get; }
        public int Rows { get; }
        public (int row, int column) EntryPosition => _entryPosition;
        public (int row, int column) ExitPosition => _exitPosition;

        public GridMap(string[] lines)
        {
            Rows = lines.Length;
            Columns = lines[0].Length;
            _data = new char[Rows, Columns];

            var labelPositions = new List<(int row, int column)>();
            
            for (var row = 0; row < Rows; row++)
            for (var column = 0; column < Columns; column++)
            {
                var @char = _data[row, column] = lines[row][column];
                if (@char == '#')
                {
                    _wallPositions.Add((row, column));
                }
                else if (char.IsLetter(@char))
                {
                    labelPositions.Add((row, column));
                }
            }

            var labelledSpacePositions = labelPositions.SelectMany(p => AdjacentPositions(p).Where(IsOpen))
                                                       .ToList();
            foreach (var position in labelledSpacePositions)
            {
                var id = ReadLabelAt(position);
                switch (id)
                {
                    case "AA":
                        _entryPosition = position;
                        break;
                    case "ZZ":
                        _exitPosition = position;
                        break;
                    default:
                        var targetSet = IsOnOuterEdge(position) ? _outerEdgePortals : _innerEdgePortals;
                        targetSet[position] = id;
                        break;
                }
            }
        }

        public char At((int row, int column) position) => _data[position.row, position.column];
        public bool IsOpen((int x, int y) position) => At(position) == '.';
        public bool IsWall((int row, int column) position) => _wallPositions.Contains(position);
        public bool IsEntrance((int row, int column) position) => _entryPosition == position;
        public bool IsExit((int row, int column) position) => _exitPosition == position;
        public bool IsPortal((int row, int column) position) => _innerEdgePortals.ContainsKey(position) || _outerEdgePortals.ContainsKey(position);
        public string GetLabelAt((int row, int column) position) => ReadLabelAt(position);

        public bool IsOnOuterEdge((int row, int column) position) =>
            position.row == _wallPositions.Min(p => p.row) ||
            position.row == _wallPositions.Max(p => p.row) ||
            position.column == _wallPositions.Min(p => p.column) ||
            position.column == _wallPositions.Max(p => p.column);

        public IEnumerable<(int x, int y)> AdjacentPositions((int row, int column) to) => GetAdjacentPositions(to, false);

        private string ReadLabelAt((int row, int column) position)
        {
            var pos1 = GetAdjacentPositions(position, true).Single(p => char.IsLetter(At(p)));
            var pos2 = GetAdjacentPositions(pos1, true).Single(p => char.IsLetter(At(p)));
            return new string(new[] { pos1, pos2 }.OrderBy(p => p.x + p.y)
                                                  .Select(At)
                                                  .ToArray());
        }
        
        private IEnumerable<(int x, int y)> GetAdjacentPositions((int row, int column) to, bool ignorePortals)
        {
            var vectors = new (int x, int y)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            foreach (var pos in vectors.Select(v => (row: to.row + v.x, column: to.column + v.y)))
            {
                if (pos.row < 0 || pos.column < 0 || pos.row >= Rows || pos.column >= Columns)
                {
                    continue;
                }
                
                if (ignorePortals)
                {
                    yield return pos;
                    continue;
                }
                
                if (IsPortal(to) && char.IsLetter(At(pos)))
                {
                    var fromPortals = IsOnOuterEdge(to) ? _outerEdgePortals : _innerEdgePortals;
                    var toPortals = IsOnOuterEdge(to) ? _innerEdgePortals : _outerEdgePortals;
                    var portalId = fromPortals[to];
                    var jumpPos  = toPortals.Single(p => p.Value == portalId).Key;
                    yield return jumpPos;
                    continue;
                }
                
                if (!char.IsLetter(At(pos)))
                {
                    yield return pos;
                }
            }
        }
    }
}