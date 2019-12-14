using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Aoc2019_Day13
{
    internal class AutoPlayer
    {
        private int _moves;
        private (int x, int y) _paddlePosition;
        private readonly HashSet<(int targetX, int atMove)> _learnedMoves;

        public IEnumerable<(int targetX, int atMove)> LearnedMoves => _learnedMoves.AsEnumerable();

        public AutoPlayer(IEnumerable<(int targetX, int atMove)> learnedMoves = null)
        {
            _learnedMoves = new HashSet<(int, int)>(learnedMoves ?? Enumerable.Empty<(int, int)>());
        }
        
        public void OnTileChanged((int x, int y) position, TileType tile)
        {
            switch (tile)
            {
                case TileType.Ball:
                    if (position.y == _paddlePosition.y - 1)
                    {
                        _learnedMoves.Add((position.x, _moves));
                        Debug.WriteLine($"Ball reaches bottom position {position.x} on move {_moves}");
                    }
                    else
                    {
                        Debug.WriteLine($"Ball at position {position} on move {_moves}");
                    }
                    break;
                case TileType.Paddle:
                    _paddlePosition = position;
                    Debug.WriteLine($"Paddle at position {position} on move {_moves}");
                    break;
            }
        }

        // ReSharper disable once IteratorNeverReturns
        public IEnumerable<long> GenerateInputs()
        {
            while (true)
            {
                _moves++;
                var hit = _learnedMoves.OrderBy(m => m.atMove).FirstOrDefault(m => m.atMove >= _moves);
                var input = Math.Sign(hit.targetX - _paddlePosition.x);
                Debug.WriteLine($"Generated input: {input}");
                yield return input;
            }
        }
    }
}