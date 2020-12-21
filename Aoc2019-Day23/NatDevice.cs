using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day23
{
    internal class NatDevice
    {
        public const int Address = 255;

        private readonly Network _network;
        private readonly IDictionary<int, RecentHistory<NetworkOperation>> _nicHistoriesByAddress
            = new Dictionary<int, RecentHistory<NetworkOperation>>();

        private NetworkPacket? _lastNatPacket;

        public NatDevice(Network network)
        {
            _network = network;
            network.PacketGenerated += OnPacketGenerated;
            network.Received += OnReceived;
            network.ReceiveFailed += OnReceiveFailed;
        }

        public void Step()
        {
            if (_lastNatPacket == null) return;
            if (_nicHistoriesByAddress.Count == 0) return;
            if (_nicHistoriesByAddress.Values.All(history => history.IsFull && history.All(op => op == NetworkOperation.FailedReceive)))
            {
                _network.SendPacket(new NetworkPacket(Address, 0, _lastNatPacket.X, _lastNatPacket.Y));
                foreach (var history in _nicHistoriesByAddress.Values)
                    history.Clear();
            }
        }

        private void OnPacketGenerated(NetworkPacket packet)
        {
            if (packet.To == Address)
                _lastNatPacket = packet;
            else
                RecordOperation(packet.From, NetworkOperation.Send);
        }

        private void OnReceived(int address)
        {
            RecordOperation(address, NetworkOperation.Receive);
        }

        private void OnReceiveFailed(int address)
        {
            RecordOperation(address, NetworkOperation.FailedReceive);
        }

        private void RecordOperation(int address, NetworkOperation operation)
        {
            if (address == Address) return;

            if (!_nicHistoriesByAddress.ContainsKey(address))
                _nicHistoriesByAddress[address] = new RecentHistory<NetworkOperation>(2);

            _nicHistoriesByAddress[address].Record(operation);
        }

        private enum NetworkOperation { Receive, FailedReceive, Send }
    }
}
