using System;

namespace Aoc2019_Day13
{
    internal static class ConsoleScreenRenderer
    {
        public static void Render(ConsoleScreenBuffer buffer, (int x, int y) position)
        {
            var (minX, minY, _, _) = buffer.Bounds;
            var tile = buffer.Get((minX + position.x, minY + position.y));
            Render(buffer, position, tile);
        }
        
        public static void RenderScore(ConsoleScreenBuffer buffer, long score)
        {
            var restorePosition = (x: Console.CursorLeft, y: Console.CursorTop);
            
            Console.CursorLeft = 0;
            Console.CursorTop = buffer.Bounds.bottom + 2;
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
            const char filledCircle    = '\u25cf';
            const char filledRectangle = '\u25ac';
            const char space           = ' ';
            
            switch (tile)
            {
                case TileType.Wall:   return filledBlock;
                case TileType.Block:  return filledBlock;
                case TileType.Paddle: return filledRectangle;
                case TileType.Ball:   return filledCircle;
                case TileType.Empty:  return space;
                default:              throw new ArgumentException("Unrecognised tile type.", nameof(tile));
            }
        }
    }
}