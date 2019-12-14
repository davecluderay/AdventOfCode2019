using System;
using System.Collections.Generic;
using System.Threading;
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
            computer.LoadProgram();
            
            foreach (var batch in computer.RunProgram().InBatchesOf(3))
            {
                var x = (int) batch[0];
                var y = (int) batch[1];
                screen.Set((x, y), (TileType) batch[2]);
            }
            
            return screen.TileCount(TileType.Block);
        }
        
        public object PartTwo()
        {
            IEnumerable<(int targetX, int atMove)> learnedMoves = null;
            while (true)
            {
                var screen = new ConsoleScreenBuffer();
                var computer = new IntCodeComputer();
                computer.LoadProgram();
                computer.SetMemory(0, 2);
            
                var score = 0L;
                
                var autoPlayer = new AutoPlayer(learnedMoves);
                foreach (var batch in computer.RunProgram(autoPlayer.GenerateInputs())
                                              .InBatchesOf(3))
                {
                    var x = (int) batch[0];
                    var y = (int) batch[1];
            
                    if (x == -1 && y == 0)
                    {
                        score = batch[2];
                    }
                    else
                    {
                        var tile = (TileType) batch[2];
            
                        screen.Set((x, y), tile);
                        autoPlayer.OnTileChanged((x, y), tile);
                    }
                }
            
                if (screen.TileCount(TileType.Block) == 0)
                {
                    return score;
                }
                
                learnedMoves = autoPlayer.LearnedMoves;
            }
        }
    }
}