using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day24
{
    internal class Layout
    {
        private const char Bug = '#';
        private const char EmptySpace = '.';
        
        private readonly char[,] _data;

        public int Columns { get; }
        public int Rows { get; }

        public Layout(string[] lines)
        {
            Rows = lines.Length;
            Columns = lines[0].Length;
            _data = new char[Rows, Columns];

            foreach (var position in AllPositions)
            {
                _data[position.row, position.column] = lines[position.row][position.column];
            }
        }

        public bool IsBugAt((int x, int y) position) => GetAt(position) == Bug;
        public bool IsEmptySpaceAt((int row, int column) position) => GetAt(position) == EmptySpace;
        public int AdjacentBugCount((int row, int column) to) => GetAdjacentPositions(to).Count(IsBugAt);
        public void SetBugAt((int x, int y) position) => SetAt(position, Bug);
        public void SetEmptySpaceAt((int x, int y) position) => SetAt(position, EmptySpace);
        
        public IEnumerable<(int row, int column)> AllPositions
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

        public int CalculateBiodiversityRating()
        {
            return AllPositions.Select((p, i) => IsBugAt(p) ? (int) Math.Pow(2, i) : 0)
                               .Sum();
        }
        
        private char GetAt((int row, int column) position)
        {
            return _data[position.row, position.column];
        }
        
        private void SetAt((int row, int column) position, char @char)
        {
            _data[position.row, position.column] = @char;
        } 
        
        private IEnumerable<(int row, int column)> GetAdjacentPositions((int row, int column) to)
        {
            var vectors = new (int rows, int columns)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            return vectors.Select(v => (row: to.row + v.rows, column: to.column + v.columns))
                          .Where(pos => pos.row >= 0 && pos.column >= 0 && pos.row < Rows && pos.column < Columns)
                          .ToArray();
        }
    }
}