using System;

namespace Aoc2019_Day08
{
    internal class ConsoleSpaceImageRenderer
    {
        private const char FilledBlock = '\u2588';
        
        public void Render(SpaceImage image)
        {
            foreach (var row in image.Decode())
            {
                foreach (var pixel in row)
                {
                    switch (pixel)
                    {
                        case PixelColor.Black:
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(FilledBlock);
                            Console.ResetColor();
                            break;
                        }
                        case PixelColor.White:
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(FilledBlock);
                            Console.ResetColor();
                            break;
                        }
                        case PixelColor.Transparent:
                        {
                            Console.Write(' ');
                            break;
                        }
                        default:
                        {
                            throw new Exception($"Unexpected pixel colour value: {pixel}");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        public static void RenderImage(SpaceImage image)
        {
            new ConsoleSpaceImageRenderer().Render(image);
        }
    }
}