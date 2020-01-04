using System.Linq;
using System.Text;

namespace Aoc2019_Day25
{
    internal class AutoplayAdapter : KeyboardInputAdapter
    {
        public AutoplayAdapter(string[] commands)
        {
            var allCommands = string.Join('\n', commands.AsEnumerable().Append(""));
            foreach (var input in Encoding.ASCII.GetBytes(allCommands))
            {
                Queue.Enqueue(input);
            }
        }
    }
}