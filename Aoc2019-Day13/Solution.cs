using Aoc2019_Day13.Computer;

namespace Aoc2019_Day13
{
    internal class Solution
    {
        public string Title => "Day 13: Care Package";

        public object PartOne()
        {
            var screen = new ConsoleScreenBuffer();
            var computer = new IntCodeComputer();

            computer.OutputTo(ReceiveInBatches.Of(3, batch =>
                                                     {
                                                         var x = (int) batch[0];
                                                         var y = (int) batch[1];
                                                         screen.Set((x, y), (TileType) batch[2]);
                                                     }));

            computer.LoadProgram();
            computer.Run();

            return screen.TileCount(TileType.Block);
        }

        public object? PartTwo()
        {
            const bool showGame = false;

            var screen = new ConsoleScreenBuffer();
            var computer = new IntCodeComputer();
            var renderer = showGame ? new ConsoleScreenRenderer(screen) : null;

            computer.OutputTo(ReceiveInBatches.Of(3, batch =>
                                                     {
                                                         var (x, y, z) = ((int) batch[0], (int) batch[1], batch[2]);
                                                         if ((x, y) == (-1, 0))
                                                         {
                                                             if (screen.GetScore() <= z)
                                                                 screen.SetScore(z);
                                                         }
                                                         else
                                                         {
                                                             screen.Set((x, y), (TileType) z);
                                                         }
                                                     }));

            computer.LoadProgram();

            var player = new PlayerBot(computer, screen, frameIntervalMilliseconds: showGame ? 100 : 0);
            var score = player.PlayPerfectGame();

            return score;
        }
    }
}
