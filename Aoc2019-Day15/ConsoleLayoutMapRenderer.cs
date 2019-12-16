using System;
using System.Collections.Generic;

namespace Aoc2019_Day15
{
    internal class ConsoleLayoutMapRenderer : IDisposable
    {
        private readonly LayoutMap _map;
        private readonly int       _positionOffsetY;

        public ConsoleLayoutMapRenderer(LayoutMap map)
        {
            _map = map;
            _positionOffsetY = Console.CursorTop;
            Console.CursorVisible = false;
        }
        
        public void Render()
        {
            var (minX, minY, maxX, maxY) = _map.Bounds;
            for (var row = minY; row <= maxY; row++)
            for (var col = minX; col <= maxX; col++)
                Render((col, row));
        }

        public void Render(IEnumerable<(int x, int y)> positions)
        {
            foreach (var position in positions)
                Render(position);
        }
        
        public void Render((int x, int y) position)
        {
            Render(position, _map.Get(position));
        }

        private void Render((int x, int y) position, SpaceType spaceType)
        {
            var @char = GetChar(spaceType);
            var color = GetColor(spaceType, position);

            var (minX, minY, _, _) = _map.Bounds;
            var transformedPosition = (x: position.x - minX, y: position.y - minY);
            Console.SetCursorPosition(transformedPosition.x, transformedPosition.y + _positionOffsetY);
            Console.BackgroundColor = color.background;
            Console.ForegroundColor = color.foreground;
            Console.Write(@char);
            Console.ResetColor();
        }

        public void Dispose()
        {
            var (_, minY, _, maxY) = _map.Bounds;
            Console.CursorLeft = 0;
            Console.CursorTop = _positionOffsetY + maxY - minY + 2;
            Console.CursorVisible = true;
        }

        private (ConsoleColor background, ConsoleColor foreground) GetColor(SpaceType spaceType, (int x, int y) position)
        {
            switch (spaceType)
            {
                case SpaceType.Wall:       return (ConsoleColor.DarkRed, ConsoleColor.DarkRed);
                case SpaceType.Droid:      return (ConsoleColor.DarkCyan, ConsoleColor.DarkCyan);
                case SpaceType.Empty:      return (SelectEmptyColor(), Console.ForegroundColor);
                case SpaceType.Oxygenated: return (ConsoleColor.DarkCyan, ConsoleColor.Cyan);
                case SpaceType.Unexplored: return (ConsoleColor.Gray, ConsoleColor.DarkGray);
                default:                   throw new ArgumentException($"Unrecognised space type: {spaceType}", nameof(spaceType));
            }
            
            ConsoleColor SelectEmptyColor()
            {
                if (position == (0, 0))
                    return ConsoleColor.DarkMagenta;
                if (_map.OxygenSystemPosition != null && position == _map.OxygenSystemPosition)
                    return ConsoleColor.DarkGreen;
                return ConsoleColor.DarkGray;
            }
        }

        private char GetChar(SpaceType spaceType)
        {
            const char filledBlock = '\u2588';
            const char filledCircle = '\u25cf';
            const char hatchedBlock = '\u25a4';
            const char space       = ' ';

            switch (spaceType)
            {
                case SpaceType.Wall:       return filledBlock;
                case SpaceType.Droid:      return filledBlock;
                case SpaceType.Empty:      return space;
                case SpaceType.Unexplored: return hatchedBlock;
                case SpaceType.Oxygenated: return filledCircle;
                default:                   throw new ArgumentException($"Unrecognised space type: {spaceType}", nameof(spaceType));
            }
        }
    }
}