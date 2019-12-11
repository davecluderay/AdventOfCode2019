using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day11
{
    internal class PanelGrid
    {
        private readonly IDictionary<(int x, int y), PaintColor> _paintedPanels = new Dictionary<(int x, int y), PaintColor>();

        public PaintColor Get((int x, int y) position) => _paintedPanels.ContainsKey(position)
            ? _paintedPanels[position]
            : PaintColor.Black;

        public void Set((int x, int y) position, PaintColor color) => _paintedPanels[(position)] = color;

        public int PaintedPanelCount => _paintedPanels.Count;

        public (int left, int top, int right, int bottom) Bounds => (_paintedPanels.Keys.Min(p => p.x),
                                                                     _paintedPanels.Keys.Min(p => p.y),
                                                                     _paintedPanels.Keys.Max(p => p.x),
                                                                     _paintedPanels.Keys.Max(p => p.y));
    }
}