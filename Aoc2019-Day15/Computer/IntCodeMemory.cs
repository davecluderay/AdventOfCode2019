using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day15.Computer
{
    internal class IntCodeMemory
    {
        private readonly List<MemoryBlock> _blocks = new List<MemoryBlock>();

        public void Reset()
        {
            _blocks.Clear();
        }

        public void Set(long position, long[] data)
        {
            if (position < 0) throw new ArgumentException("Must be non-negative.", nameof(position));
            
            var blocksToRemove = new List<MemoryBlock>();
            var blocksToAdd    = new List<MemoryBlock>();

            var newBlock = new MemoryBlock(position, data);
            blocksToAdd.Add(newBlock);
            
            foreach (var block in _blocks.Where(block => block.Overlaps(newBlock)))
            {
                blocksToRemove.Add(block);
                blocksToAdd.AddRange(block.Subtract(newBlock));
            }

            _blocks.RemoveAll(b => blocksToRemove.Contains(b));
            _blocks.AddRange(blocksToAdd);
        }

        public void SetAt(long position, long value)
        {
            if (position < 0) throw new ArgumentException("Must be non-negative.", nameof(position));
            
            // Not worrying about extending adjacent blocks or removing zero blocks for now.
            var block = _blocks.SingleOrDefault(b => b.ContainsPosition(position));
            if (block == null)
            {
                _blocks.Add(block = new MemoryBlock(position, new long[1]));
            }
            block.SetValueAtPosition(position, value);
        }
        
        public long GetAt(long position)
        {
            var block = _blocks.SingleOrDefault(b => b.ContainsPosition(position));
            if (block == null) return 0L;
            return block.GetValueAtPosition(position);
        }

        public IEnumerable<long> GetAt(long position, int count)
        {
            while (count-- > 0)
            {
                yield return GetAt(position++);
            }
        }
        
        private class MemoryBlock
        {
            private readonly long   _position;
            private readonly long[] _data;

            public MemoryBlock(long position, long[] data)
            {
                _position = position;
                _data = data;
            }

            public long FirstPosition => _position;
            public long LastPosition  => _position + _data.LongLength - 1;

            public bool ContainsPosition(long   position)             => FirstPosition <= position && LastPosition >= position;
            public long GetValueAtPosition(long position)             => _data[position - FirstPosition];
            public void SetValueAtPosition(long position, long value) => _data[position - FirstPosition] = value;
            public bool Overlaps(MemoryBlock    block) => block.LastPosition >= FirstPosition && block.FirstPosition <= LastPosition;
            
            public IEnumerable<MemoryBlock> Subtract(MemoryBlock block)
            {
                if (!Overlaps(block)) throw new InvalidOperationException("Cannot subtract non-overlapping blocks.");
                
                if (block.FirstPosition > FirstPosition)
                {
                    var data = new long[block.FirstPosition - FirstPosition];
                    Array.Copy(_data, 0, data, 0, data.Length);
                    yield return new MemoryBlock(FirstPosition, data);
                }

                if (block.LastPosition < LastPosition)
                {
                    var data = new long[LastPosition - block.LastPosition];
                    Array.Copy(_data, block.LastPosition - FirstPosition + 1, data, 0, data.Length);
                    yield return new MemoryBlock(block.LastPosition + 1, data);
                }
            }
        }
    }
}