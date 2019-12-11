namespace Aoc2019_Day11
{
    internal class PaintBot
    {
        private readonly (int x, int y)[] _vectors     = new (int x, int y)[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
        private          int              _vectorIndex = 0;
        private          (int x, int y)   _position    = (0, 0);

        public (int x, int y) CurrentPosition => _position;

        public void Rotate(RotationDirection direction)
        {
            _vectorIndex = 
                direction == RotationDirection.Left
                    ? _vectorIndex <= 0 ? _vectors.Length - 1 : _vectorIndex - 1
                    : _vectorIndex >= _vectors.Length - 1 ? 0 : _vectorIndex + 1;
            
        }

        public void MoveForward()
        {
            var (dx, dy) = _vectors[_vectorIndex];
            _position = (_position.x + dx, _position.y + dy);
        }
    }
}