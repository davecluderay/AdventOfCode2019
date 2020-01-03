using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Aoc2019_Day23
{
    internal class Network
    {
        private readonly ConcurrentDictionary<long, HostQueue> _hostQueues = new ConcurrentDictionary<long, HostQueue>();

        public void Send(long address, (long x, long y) packet) => GetQueueFor(address).Send(packet);
        public Func<long> GetReceiver(long address) => () => GetQueueFor(address).Receive();
        public bool HasUnhandledPackets() => _hostQueues.Any(q => !q.Value.IsEmpty);

        private HostQueue GetQueueFor(long address)
        {
            return _hostQueues.GetOrAdd(address, id => new HostQueue(id));
        }
        
        private class HostQueue
        {
            private readonly ConcurrentQueue<long> _queue = new ConcurrentQueue<long>();

            public bool IsEmpty => _queue.IsEmpty;
            
            public HostQueue(long address)
            {
                _queue.Enqueue(address);
            }

            public long Receive()
            {
                return _queue.TryDequeue(out long result) ? result : -1L;
            }

            public void Send((long x, long y) packet)
            {
                _queue.Enqueue(packet.x);
                _queue.Enqueue(packet.y);
            }
        }
    }
}