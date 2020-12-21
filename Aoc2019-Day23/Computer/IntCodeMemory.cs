using System;
using System.Collections.Generic;

namespace Aoc2019_Day23.Computer
{
    internal class IntCodeMemory
    {
        private Dictionary<long, long> _storage = new Dictionary<long, long>();

        public void SetAt(long address, long value)
        {
            if (address < 0) throw new ArgumentException("Must be non-negative.", nameof(address));
            if (value == 0 && _storage.ContainsKey(address))
                _storage.Remove(address);
            else
                _storage[address] = value;
        }

        public long GetAt(long address)
        {
            if (address < 0) throw new ArgumentException("Must be non-negative.", nameof(address));
            return _storage.ContainsKey(address)
                ? _storage[address]
                : 0L;
        }

        public IDictionary<long, long> Copy()
            => new Dictionary<long, long>(_storage);

        public void Replace(IDictionary<long, long> with)
            => _storage = new Dictionary<long, long>(with);
    }
}
