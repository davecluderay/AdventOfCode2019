using System;
using System.Collections.Generic;

using Aoc2019_Day23.Computer;

namespace Aoc2019_Day23
{
    internal class Network
    {
        private readonly List<IntCodeComputer> _nics = new List<IntCodeComputer>();
        private readonly List<Queue<long>> _inputQueues = new List<Queue<long>>();
        private NatDevice? _nat;

        public event Action<NetworkPacket> PacketGenerated = delegate {};
        public event Action<int> Received = delegate {};
        public event Action<int> ReceiveFailed = delegate {};

        public void AddNat()
        {
            _nat = new NatDevice(this);
        }

        public void AddNics(int count)
        {
            while (--count >= 0)
                AddNic();
        }

        public void Step()
        {
            if (_nics.Count == 0) throw new InvalidOperationException("Nothing to run yet!");

            foreach (var nic in _nics)
            {
                nic.Step();
            }

            _nat?.Step();
        }

        public void SendPacket(NetworkPacket packet)
        {
            PacketGenerated(packet);
            if (packet.To >= _inputQueues.Count) return;
            var queue = _inputQueues[packet.To];
            queue.Enqueue(packet.X);
            queue.Enqueue(packet.Y);
        }

        private void AddNic()
        {
            var address = _nics.Count;
            var inputQueue = new Queue<long>(new[] { (long) address });

            var nic = new IntCodeComputer();
            nic.LoadProgram();

            nic.InputFrom(() => ReceiveData(address));
            nic.OutputTo(ReceiveInBatches.Of(3, batch => SendPacket(new NetworkPacket(address, batch))));

            _inputQueues.Add(inputQueue);
            _nics.Add(nic);
        }

        private long ReceiveData(int address)
        {
            var inputQueue = _inputQueues[address];
            if (inputQueue.Count > 0)
            {
                Received(address);
                return inputQueue.Dequeue();
            }

            ReceiveFailed(address);
            return -1L;
        }
    }
}
