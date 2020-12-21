using System.Collections;
using System.Collections.Generic;

namespace Aoc2019_Day23
{
    internal class RecentHistory<T> : IEnumerable<T>
    {
        private readonly int _capacity;
        private readonly T[] _buffer;
        private int _nextPosition;
        private int _count;

        public bool IsFull => _count == _capacity;

        public RecentHistory(int capacity, T[]? values = null)
        {
            _capacity = capacity;
            _nextPosition = 0;
            _count = 0;
            _buffer = new T[capacity];

            if (values == null) return;

            foreach (var value in values)
                Record(value);
        }

        public void Record(T entry)
        {
            _buffer[_nextPosition] = entry;
            if (_count < _capacity) _count++;
            _nextPosition = Modulo(_nextPosition + 1, _capacity);
        }

        public T Get(int position)
        {
            return _buffer[Modulo(_nextPosition - _count + position, _capacity)];
        }

        public void Clear()
        {
            _count = 0;
        }

        private static int Modulo(int value, int modulus)
        {
            var result = value % modulus;
            return result < 0 ? modulus + result : result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = _count - 1; i >= 0; i--)
                yield return Get(i);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}