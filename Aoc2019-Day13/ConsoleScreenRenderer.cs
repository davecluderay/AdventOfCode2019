using System;

namespace Aoc2019_Day13
{
    internal class ConsoleScreenRenderer
    {
        private readonly ConsoleScreenBuffer _buffer;

        public ConsoleScreenRenderer(ConsoleScreenBuffer buffer)
        {
            _buffer = buffer;
            _buffer.Changed += OnBufferChanged;
            _buffer.ScoreChanged += OnScoreChanged;
        }

        private void OnBufferChanged((int x, int y) position, TileType tile)
        {
            RenderAt(position);
        }

        private void OnScoreChanged(long score)
        {
            RenderScore(score);
        }

        public void RenderAt((int x, int y) position)
        {
            var (minX, minY, _, _) = _buffer.Bounds;
            var tile = _buffer.Get((minX + position.x, minY + position.y));
            Render(_buffer, position, tile);
        }

        public void RenderScore(long score)
        {
            var restorePosition = (x: Console.CursorLeft, y: Console.CursorTop);

            Console.CursorLeft = 0;
            Console.CursorTop = _buffer.Bounds.bottom + 2;
            Console.Write("SCORE: [ ");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(score);
            Console.ResetColor();
            Console.Write(" ]");

            Console.CursorLeft = restorePosition.x;
            Console.CursorTop = restorePosition.y;
        }

        private static void Render(ConsoleScreenBuffer buffer, (int x, int y) position, TileType tile)
        {
            var @char = GetChar(tile);
            var color = GetColor(tile);

            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = color;
            Console.Write(@char);

            Console.ResetColor();
            Console.SetCursorPosition(0, buffer.Bounds.bottom + 4);
        }

        private static ConsoleColor GetColor(TileType tile)
        {
            switch (tile)
            {
                case TileType.Wall:   return ConsoleColor.DarkRed;
                case TileType.Block:  return ConsoleColor.DarkGreen;
                case TileType.Paddle: return ConsoleColor.Magenta;
                case TileType.Ball:   return ConsoleColor.Yellow;
                case TileType.Empty:  return ConsoleColor.White;
                default:              throw new ArgumentException("Unrecognised tile type.", nameof(tile));
            }
        }

        private static char GetChar(TileType tile)
        {
            const char filledBlock     = '\u2588';
            const char space           = ' ';

            switch (tile)
            {
                case TileType.Wall:   return filledBlock;
                case TileType.Block:  return filledBlock;
                case TileType.Paddle: return filledBlock;
                case TileType.Ball:   return filledBlock;
                case TileType.Empty:  return space;
                default:              throw new ArgumentException("Unrecognised tile type.", nameof(tile));
            }
        }
    }
}
