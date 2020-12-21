using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using Aoc2019_Day13.Computer;

namespace Aoc2019_Day13
{
    internal class PlayerBot
    {
        private readonly IntCodeComputer _computer;
        private readonly ConsoleScreenBuffer _screen;
        private readonly TimeSpan? _frameInterval;
        private readonly Stack<GameSnapshot> _snapshots = new Stack<GameSnapshot>();
        private readonly HashSet<string> _snapshotsExplored = new HashSet<string>();

        private Action _snapshottingAction;
        private (int x, int y)? _currentPosition;
        private (int x, int y)? _ballPosition;
        private int? _targetX;

        public PlayerBot(IntCodeComputer computer, ConsoleScreenBuffer screen, int frameIntervalMilliseconds = 0)
        {
            _computer = computer;
            _screen = screen;
            _frameInterval = frameIntervalMilliseconds > 0 ? (TimeSpan?)TimeSpan.FromMilliseconds(frameIntervalMilliseconds) : null;

            _screen.Changed += OnScreenChanged;
            _computer.InputFrom(ProvideInput);
            _computer.PokeMemory(0L, 2L);

            _snapshots.Push(new GameSnapshot(_computer.TakeSnapshot(),
                                             _screen.TakeSnapshot(),
                                             _currentPosition,
                                             _ballPosition));
            _snapshottingAction = DoNothing;
        }

        public long PlayPerfectGame()
        {
            while (!_computer.IsHalted)
            {
                _snapshottingAction.Invoke();
                _computer.Step();
            }

            return _screen.GetScore();
        }

        private void OnScreenChanged((int x, int y) position, TileType tile)
        {
            switch (tile)
            {
                case TileType.Ball:
                    if (position.y == _currentPosition?.y)
                    {
                        var snapshot = _snapshots.Peek();
                        if (snapshot.PossibleTargetX.Count == 0)
                        {
                            snapshot.PossibleTargetX.Enqueue(position.x);
                            if (position.x > _screen.Bounds.left)
                                snapshot.PossibleTargetX.Enqueue(position.x - 1);
                            if (position.x < _screen.Bounds.right)
                                snapshot.PossibleTargetX.Enqueue(position.x + 1);
                        }
                        _snapshottingAction = RestoreSnapshot;
                    }
                    else if (position.y < _ballPosition?.y && _ballPosition?.y + 1 == _currentPosition?.y)
                    {
                        _snapshottingAction = TakeSnapshot;
                    }

                    _ballPosition = position;
                    break;
                case TileType.Paddle:
                    _currentPosition = position;
                    break;
            }
        }

        private long ProvideInput()
        {
            if (_frameInterval != null)
                Thread.Sleep(_frameInterval.Value);

            var result = 0L;

            if (_currentPosition?.x == _targetX) result = 0L;
            if (_currentPosition?.x < _targetX) result = 1L;
            if (_currentPosition?.x > _targetX) result = -1L;

            return result;
        }

        private void TakeSnapshot()
        {
            var snapshot = new GameSnapshot(_computer.TakeSnapshot(),
                                            _screen.TakeSnapshot(),
                                            _currentPosition,
                                            _ballPosition);

            if (!_snapshotsExplored.Contains(snapshot.EquivalenceHash))
            {
                _snapshots.Push(snapshot);
                _snapshotsExplored.Add(snapshot.EquivalenceHash);
            }

            _snapshottingAction = DoNothing;
        }

        private void RestoreSnapshot()
        {
            var snapshot = _snapshots.Peek();
            _computer.ApplySnapshot(snapshot.ComputerSnapshot);
            _screen.ApplySnapshot(snapshot.ScreenSnapshot);
            _currentPosition = snapshot.CurrentPosition;
            _ballPosition = snapshot.BallPosition;

            _snapshottingAction = DoNothing;

            _targetX = snapshot.PossibleTargetX.Dequeue();

            if (snapshot.PossibleTargetX.Count == 0)
                _snapshots.Pop();
        }

        private void DoNothing() { }

        private class GameSnapshot
        {
            public string EquivalenceHash { get; }
            public IntCodeDebugSnapshot ComputerSnapshot { get; }
            public ScreenSnapshot ScreenSnapshot { get; }
            public (int x, int y)? CurrentPosition { get; }
            public (int x, int y)? BallPosition { get; }
            public Queue<int> PossibleTargetX { get; } = new Queue<int>();

            public GameSnapshot(IntCodeDebugSnapshot computerSnapshot, ScreenSnapshot screenSnapshot, (int x, int y)? currentPosition, (int x, int y)? ballPosition)
            {
                EquivalenceHash = CalculateSha1Hash(computerSnapshot);
                ComputerSnapshot = computerSnapshot;
                ScreenSnapshot = screenSnapshot;
                CurrentPosition = currentPosition;
                BallPosition = ballPosition;
            }

            private static string CalculateSha1Hash(IntCodeDebugSnapshot snapshot)
            {
                using var stream = new MemoryStream();
                stream.Write(BitConverter.GetBytes(snapshot.InstructionPointer));
                stream.Write(BitConverter.GetBytes(snapshot.RelativeBase));
                foreach (var key in snapshot.Memory.Keys.OrderBy(k => k))
                {
                    stream.Write(BitConverter.GetBytes(key));
                    stream.Write(BitConverter.GetBytes(snapshot.Memory[key]));
                }
                stream.Flush();
                return SHA1.Create()
                           .ComputeHash(stream.GetBuffer())
                           .Aggregate(new StringBuilder(),
                                      (a, v) => a.Append(v.ToString("x2")),
                                      a => a.ToString());
            }
        }
    }
}
