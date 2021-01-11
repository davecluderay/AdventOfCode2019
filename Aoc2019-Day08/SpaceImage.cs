using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2019_Day08
{
    internal class SpaceImage
    {
        private readonly PixelColor[]            _imageData;
        public           int                     Width     { get; }
        public           int                     Height    { get; }
        public           IEnumerable<PixelColor> ImageData => _imageData.AsEnumerable();

        public SpaceImage(PixelColor[] imageData, int width, int height)
        {
            if (imageData.Length % (width * height) != 0) throw new ArgumentException("Unexpected data size for the supplied dimensions.");
            _imageData = imageData;
            Width = width;
            Height = height;
        }

        public static SpaceImage ReadImage(int width, int height, string? fileName = null)
        {
            var imageData = InputFile.ReadAllLines(fileName)
                                     .Single()
                                     .Select(@char => (PixelColor)@char - (int) '0')
                                     .ToArray();

            return new SpaceImage(imageData, width, height);
        }

        public IEnumerable<PixelColor[]> GetRawLayerData()
        {
            var layerLength = Width * Height;
            var layerCount  = _imageData.Length / layerLength;
            for (int layer = 0; layer < layerCount; layer++)
            {
                var segment = _imageData.AsSpan(layer * layerLength, layerLength);
                yield return segment.ToArray();
            }
        }

        public IEnumerable<PixelColor[]> Decode()
        {
            var layers = GetRawLayerData().ToArray();
            for (var y = 0; y < Height; y++)
            {
                var currentRow = new PixelColor[Width];
                for (var x = 0; x < Width; x++)
                {
                    var pixel = layers.Select(l => l[x + y * Width])
                                      .Where(p => p != PixelColor.Transparent)
                                      .Cast<PixelColor?>()
                                      .FirstOrDefault() ?? PixelColor.Transparent;
                    currentRow[x] = pixel;
                }

                yield return currentRow;
            }
        }
    }
}
