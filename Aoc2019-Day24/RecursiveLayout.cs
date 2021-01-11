using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day24
{
    internal class RecursiveLayout
    {
        private const char Bug = '#';
        private const char EmptySpace = '.';
        private const char InnerRecursion = '?';

        private readonly char[,] _data;
        private readonly Lazy<RecursiveLayout> _innerLevel;
        private readonly Lazy<RecursiveLayout> _outerLevel;

        public int Columns { get; }
        public int Rows { get; }
        public int Level { get; }
        public RecursiveLayout OuterLevel => _outerLevel.Value;
        public RecursiveLayout InnerLevel => _innerLevel.Value;
        public (int row, int column) CentrePosition { get; }
        public bool HasBeenInfested { get; private set; }

        public RecursiveLayout(string[] lines)
            : this(lines, null, null)
        {
        }

        public RecursiveLayout(RecursiveLayout? innerLevel, RecursiveLayout? outerLevel)
            : this(null, innerLevel, outerLevel)
        {
        }

        private RecursiveLayout(string[]? lines, RecursiveLayout? innerLevel, RecursiveLayout? outerLevel)
        {
            Level = innerLevel?.Level - 1 ?? outerLevel?.Level + 1 ?? 0;
            _innerLevel = new Lazy<RecursiveLayout>(() => innerLevel ?? new RecursiveLayout(null, this));
            _outerLevel = new Lazy<RecursiveLayout>(() => outerLevel ?? new RecursiveLayout(this, null));

            Rows = innerLevel?.Rows ?? outerLevel?.Rows ?? lines?.Length ?? 0;
            Columns = innerLevel?.Columns ?? outerLevel?.Columns ?? lines?.First().Length ?? 0;
            CentrePosition = (Rows / 2, Columns / 2);

            _data = new char[Rows, Columns];

            foreach (var position in AllPositionsAtThisLevel)
            {
                if (lines == null)
                    SetAt(position, EmptySpace);
                else
                    SetAt(position,
                          position == CentrePosition ? InnerRecursion : lines[position.row][position.column]);
            }
        }

        public char At((int x, int y) position) => GetAt(position);
        public bool IsBugAt((int x, int y) position) => GetAt(position) == Bug;
        public bool IsEmptySpaceAt((int row, int column) position) => GetAt(position) == EmptySpace;
        public int AdjacentBugCount((int row, int column) to) => GetAdjacentPositions(to).Count(x => x.level.IsBugAt(x.position));
        public void SetBugAt((int x, int y) position) => SetAt(position, Bug);
        public void SetEmptySpaceAt((int x, int y) position) => SetAt(position, EmptySpace);

        public IEnumerable<(RecursiveLayout level, (int row, int column) position)> GetAdjacentPositions((int row, int column) to)
        {
            foreach (var a in GetAdjacentPositionsAtThisLevel(to))
                yield return (level: this, position: a);

            foreach (var a in GetAdjacentPositionsAtOuterLevel(to))
                yield return (level: _outerLevel.Value, position: a);

            foreach (var a in GetAdjacentPositionsAtInnerLevel(to))
                yield return (level: _innerLevel.Value, position: a);
        }

        public IEnumerable<(RecursiveLayout level, (int row, int column) position)> FindBugsAtAllLevels()
        {
            var bugs = FindBugsAtThisLevel().Select(p => (this, p));

            var inner = InnerLevel;
            while (inner.HasBeenInfested)
            {
                var level = inner;
                bugs = bugs.Concat(inner.FindBugsAtThisLevel().Select(p => (level, p)));
                inner = inner.InnerLevel;
            }

            var outer = OuterLevel;
            while (outer.HasBeenInfested)
            {
                var level = outer;
                bugs = bugs.Concat(outer.FindBugsAtThisLevel().Select(p => (level, p)));
                outer = outer.OuterLevel;
            }

            return bugs;
        }

        public override string ToString() => $"Level {Level}";

        private char GetAt((int row, int column) position)
        {
            return _data[position.row, position.column];
        }

        private void SetAt((int row, int column) position, char @char)
        {
            if (@char == Bug) HasBeenInfested = true;
            _data[position.row, position.column] = @char;
        }

        private IEnumerable<(int row, int column)> AllPositionsAtThisLevel
        {
            get
            {
                for (var row = 0; row < Rows; row++)
                for (var column = 0; column < Columns; column++)
                {
                    yield return (row, column);
                }
            }
        }

        private IEnumerable<(int row, int column)> FindBugsAtThisLevel()
        {
            return AllPositionsAtThisLevel.Where(IsBugAt);
        }

        private IEnumerable<(int row, int column)> GetAdjacentPositionsAtThisLevel((int row, int column) to)
        {
            var vectors = new (int rows, int columns)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            return vectors.Select(v => (row: to.row + v.rows, column: to.column + v.columns))
                          .Where(pos => pos.row >= 0 && pos.column >= 0 && pos.row < Rows && pos.column < Columns)
                          .Where(p => p != CentrePosition);
        }

        private IEnumerable<(int row, int column)> GetAdjacentPositionsAtOuterLevel((int row, int column) to)
        {
            // If it's an outer position, it is considered adjacent to the corresponding inner position on level - 1.
            if (to.row == 0)
                yield return (CentrePosition.row - 1, CentrePosition.column);

            if (to.row == Rows - 1)
                yield return (CentrePosition.row + 1, CentrePosition.column);

            if (to.column == 0)
                yield return (CentrePosition.row, CentrePosition.column - 1);

            if (to.column == Columns - 1)
                yield return (CentrePosition.row, CentrePosition.column + 1);
        }

        private IEnumerable<(int row, int column)> GetAdjacentPositionsAtInnerLevel((int row, int column) to)
        {
            // If it's an outer position, it is considered adjacent to the corresponding outer positions on level + 1.
            if (to.row == CentrePosition.row - 1 && to.column == CentrePosition.column)
                for (var column = 0; column < Columns; column++)
                    yield return (0, column);

            if (to.row == CentrePosition.row + 1 && to.column == CentrePosition.column)
                for (var column = 0; column < Columns; column++)
                    yield return (Rows - 1, column);

            if (to.column == CentrePosition.column - 1 && to.row == CentrePosition.row)
                for (var row = 0; row < Rows; row++)
                    yield return (row, 0);

            if (to.column == CentrePosition.column + 1 && to.row == CentrePosition.row)
                for (var row = 0; row < Rows; row++)
                    yield return (row, Columns - 1);
        }
    }
}
