using System;
using System.Collections.Generic;

namespace Aoc2019_Day23
{
    internal class ReceiveInBatches
    {
        private readonly Action<long[]> _batchAction;
        private List<long> _currentBatch;

        private ReceiveInBatches(int batchSize, Action<long[]> batchAction)
        {
            _currentBatch = new List<long>(batchSize);
            _batchAction = batchAction;
        }

        private void Receiver(long output)
        {
            _currentBatch.Add(output);

            if (_currentBatch.Count == _currentBatch.Capacity)
            {
                _batchAction.Invoke(_currentBatch.ToArray());
                _currentBatch.Clear();
            }
        }

        public static Action<long> Of(int batchSize, Action<long[]> batchAction)
            => new ReceiveInBatches(batchSize, batchAction);

        public static implicit operator Action<long>(ReceiveInBatches d)
            => d.Receiver;
    }
}