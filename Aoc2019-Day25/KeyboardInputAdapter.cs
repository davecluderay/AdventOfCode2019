using System;
using System.Collections.Generic;
using System.Text;

namespace Aoc2019_Day25
{
    internal class KeyboardInputAdapter
    {
        protected readonly Queue<byte> Queue = new Queue<byte>();

        public long GetNextInput()
        {
            if (Queue.Count == 0)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                while (line == "?")
                {
                    Console.WriteLine("Suggestions: take, drop, inv, <direction>");
                    Console.Write("> ");
                    line = Console.ReadLine();
                }

                foreach (var input in Encoding.ASCII.GetBytes($"{line}\n"))
                {
                    Queue.Enqueue(input);
                }
            }
            else
            {
                Console.Write(Encoding.ASCII.GetString(new[] { Queue.Peek() }));
            }

            return Queue.Dequeue();
        }
    }
}
