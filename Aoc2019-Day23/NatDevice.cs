using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day23
{
    internal class NatDevice
    {
        public static readonly long Address = 255;
            
        private enum OperationType { Receive, FailedReceive, Send }
            
        private readonly Network _network;
        private (long x, long y)? _latestReceivedPacket;
            
        private readonly HashSet<long> _forwardedPacketYValues
            = new HashSet<long>();
        private readonly ConcurrentDictionary<long, (OperationType type, long count)> _lastOperationsByAddress
            = new ConcurrentDictionary<long, (OperationType type, long count)>();

        public NatDevice(Network network)
        {
            _network = network;
        }
            
        public void ReceiveNatPacket((long x, long y) packet)
        {
            _latestReceivedPacket = packet;
        }

        public void CaptureSend(long from)
        {
            RecordLastOperation(from, OperationType.Send);
        }

        public void CaptureReceiveAttempt(long address, long result)
        {
            var type = result == -1 ? OperationType.FailedReceive : OperationType.Receive;
            RecordLastOperation(address, type);
        }

        private void RecordLastOperation(long address, OperationType type)
        {
            var count = 1L;
            if (_lastOperationsByAddress.TryGetValue(address, out var operation))
            {
                count = operation.type == type ? operation.count + 1 : 1;
            }

            _lastOperationsByAddress[address] = (type, count);
        }

        public long? HandleIdleNetwork()
        {
            var allNodesContinuouslyFailingToReceive = _lastOperationsByAddress.Values.All(o => o.type == OperationType.FailedReceive && o.count > 1);
            if (allNodesContinuouslyFailingToReceive && !_network.HasUnhandledPackets() && _latestReceivedPacket != null)
            {
                _network.Send(0, _latestReceivedPacket.Value);
                    
                if (_forwardedPacketYValues.Contains(_latestReceivedPacket.Value.y))
                    return _latestReceivedPacket.Value.y;
                    
                _forwardedPacketYValues.Add(_latestReceivedPacket.Value.y);
            }

            return null;
        }
    }
}