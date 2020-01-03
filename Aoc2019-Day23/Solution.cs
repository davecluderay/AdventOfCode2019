using System.Linq;
using Aoc2019_Day23.Computer;

namespace Aoc2019_Day23
{
    internal class Solution
    {
        public string Title => "Day 23: Category Six";

        public object PartOne()
        {
            var computers = Enumerable.Range(0, 50).Select(_ => CreateNicComputer()).ToArray();
            
            var network = new Network();

            var outputStreams = computers.Select((c, a) => c.RunProgramIncrementally(network.GetReceiver(a))
                                                            .GetEnumerator())
                                         .ToArray();

            while (true)
            {
                foreach (var stream in outputStreams)
                {
                    if (!stream.MoveNext() || !stream.Current.IsOutput) continue;
                    var to = stream.Current.OutputValue;
                    
                    while (stream.MoveNext())
                        if (stream.Current.IsOutput) break;
                    var x = stream.Current.OutputValue;
                    
                    while (stream.MoveNext())
                        if (stream.Current.IsOutput) break;
                    var y = stream.Current.OutputValue;

                    network.Send(to, (x, y));

                    if (to == NatDevice.Address)
                    {
                        return y;
                    }
                }
            }
        }
        
        public object PartTwo()
        {
            var computers = Enumerable.Range(0, 50).Select(_ => CreateNicComputer()).ToArray();
            
            var network = new Network();
            var natDevice = new NatDevice(network);

            var outputStreams = computers.Select((c, a) => c.RunProgramIncrementally(network.GetReceiver(a))
                                                            .GetEnumerator())
                                         .ToArray();

            while (true)
            {
                for (var address = 0; address < outputStreams.Length; address++)
                {
                    var stream = outputStreams[address];
                    
                    if (!stream.MoveNext()) continue;

                    if (stream.Current.IsInput)
                    {
                        natDevice.CaptureReceiveAttempt(address, stream.Current.InputValue);
                        continue;
                    }
                    
                    if (stream.Current.IsOutput)
                    {
                        var to = stream.Current.OutputValue;
                    
                        while (stream.MoveNext()) if (stream.Current.IsOutput) break;
                        var x = stream.Current.OutputValue;
                        
                        while (stream.MoveNext()) if (stream.Current.IsOutput) break;
                        var y = stream.Current.OutputValue;

                        if (to == NatDevice.Address)
                        {
                            natDevice.ReceiveNatPacket((x, y));
                        }
                        else
                        {
                            network.Send(to, (x, y));
                            natDevice.CaptureSend(from: address);
                        }
                    }
                }

                var result = natDevice.HandleIdleNetwork();
                if (result.HasValue) return result.Value;
            }
        }

        private IntCodeComputer CreateNicComputer()
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();
            return computer;
        }
    }
}