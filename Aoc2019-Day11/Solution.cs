using System.Linq;
using Aoc2019_Day11.Computer;

namespace Aoc2019_Day11
{
    internal class Solution
    {
        public string Title => "Day 11: Space Police";

        public object PartOne()
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();

            var panelGrid = new PanelGrid();
            var paintBot = new PaintBot();

            var outputs = computer.RunProgram(() => (long) panelGrid.Get(paintBot.CurrentPosition));
            
            foreach (var (output, index) in outputs.Select((o, i) => (o, i)))
            {
                if (index % 2 == 0)
                {
                    panelGrid.Set(paintBot.CurrentPosition, (PaintColor)output);
                }
                else
                {
                    paintBot.Rotate((RotationDirection) output);
                    paintBot.MoveForward();
                };
            }

            return panelGrid.PaintedPanelCount;
        }
        
        public object PartTwo()
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();
            
            var panelGrid = new PanelGrid();
            var paintBot = new PaintBot();
            
            panelGrid.Set((0, 0), PaintColor.White);
            
            var outputs = computer.RunProgram(() => (long) panelGrid.Get(paintBot.CurrentPosition));
            
            foreach (var (output, index) in outputs.Select((o, i) => (o, i)))
            {
                if (index % 2 == 0)
                {
                    panelGrid.Set(paintBot.CurrentPosition, (PaintColor) output);
                }
                else
                {
                    paintBot.Rotate((RotationDirection) output);
                    paintBot.MoveForward();
                };
            }

            PanelGridConsoleRenderer.Render(panelGrid);

            return "See console output!";
        }
    }
}