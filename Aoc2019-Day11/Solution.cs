using System;
using System.Collections.Generic;
using System.Linq;
using Aoc2019_Day11.Computer;

namespace Aoc2019_Day11
{
    internal enum PaintColor
    {
        Black = 0,
        White = 1
    }

    internal class PanelGrid
    {
        private readonly IDictionary<(int x, int y), PaintColor> _paintedPanels = new Dictionary<(int x, int y), PaintColor>();

        public PaintColor Get((int x, int y) position)
            => _paintedPanels.ContainsKey(position)
                ? _paintedPanels[position]
                : PaintColor.Black;

        public void Set((int x, int y) position, PaintColor color)
            => _paintedPanels[position] = color;

        public int PaintedPanelCount
            => _paintedPanels.Count;

        public (int left, int top, int right, int bottom) Bounds
            => (_paintedPanels.Keys.Min(p => p.x),
                _paintedPanels.Keys.Min(p => p.y),
                _paintedPanels.Keys.Max(p => p.x),
                _paintedPanels.Keys.Max(p => p.y));
    }

    internal static class PanelGridConsoleRenderer
    {
        public static void Render(PanelGrid panelGrid)
        {
            const char FilledBlock = '\u2588';

            var (minX, minY, maxX, maxY) = panelGrid.Bounds;
            for (var row = 0; row <= maxY - minY; row++)
            {
                for (var col = 0; col <= maxX - minX; col++)
                {
                    var color = panelGrid.Get((minX + col, minY + row));
                    Console.ForegroundColor = color == PaintColor.Black ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write(FilledBlock);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }

    internal enum RotationDirection
    {
        Left = 0,
        Right = 1
    }

    internal class PaintBot
    {
        private readonly (int x, int y)[] _vectors = { (0, -1), (1, 0), (0, 1), (-1, 0) };
        private int _vectorIndex = 0;
        private (int x, int y) _position = (0, 0);

        public (int x, int y) CurrentPosition
            => _position;

        public void Rotate(RotationDirection direction)
        {
            _vectorIndex =
                direction == RotationDirection.Left
                    ? Modulo(_vectorIndex - 1, _vectors.Length)
                    : Modulo(_vectorIndex + 1, _vectors.Length);
        }

        public void MoveForward()
        {
            var (dx, dy) = _vectors[_vectorIndex];
            _position = (_position.x + dx, _position.y + dy);
        }

        private int Modulo(int number, int modulo)
        {
            var result = number % modulo;
            return result < 0
                ? modulo + result
                : result;
        }
    }

    internal class Solution
    {
        public string Title => "Day 11: Space Police";

        public object? PartOne()
        {
            var panelGrid = new PanelGrid();
            var paintBot = new PaintBot();
            panelGrid.Set(paintBot.CurrentPosition, PaintColor.Black);

            RunPaintBot(panelGrid, paintBot);

            return panelGrid.PaintedPanelCount;
        }

        public object? PartTwo()
        {
            var panelGrid = new PanelGrid();
            var paintBot = new PaintBot();
            panelGrid.Set(paintBot.CurrentPosition, PaintColor.White);

            RunPaintBot(panelGrid, paintBot);

            PanelGridConsoleRenderer.Render(panelGrid);
            return "(read the output)";
        }

        private void RunPaintBot(PanelGrid panelGrid, PaintBot paintBot)
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();

            computer.InputFrom(() => (long) panelGrid.Get(paintBot.CurrentPosition));

            var outputsReceived = 0;
            computer.OutputTo(output =>
            {
                if (outputsReceived++ % 2 == 0)
                {
                    panelGrid.Set(paintBot.CurrentPosition, (PaintColor) output);
                }
                else
                {
                    paintBot.Rotate((RotationDirection) output);
                    paintBot.MoveForward();
                }
            });

            computer.Run();
        }
    }
}
