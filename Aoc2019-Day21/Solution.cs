using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aoc2019_Day21.Computer;

namespace Aoc2019_Day21
{
    internal class Solution
    {
        public string Title => "Day 21: Springdroid Adventure";

        public object? PartOne()
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();

            computer.InputFrom(GenerateInputs("NOT J T", // SET T=1
                                              "OR D J",  // SET J=1 IF LANDING POSITION (D) HAS FLOOR
                                              "AND A T", // WORK OUT IF PRECEDING POSITIONS ARE ALL FLOOR
                                              "AND B T",
                                              "AND C T",
                                              "NOT T T", // SET T=1 ONLY IF PRECEDING POSITIONS ARE NOT ALL FLOOR
                                              "AND T J", // SET J=1 ONLY IF IT WAS ALREADY SET AND THE PRECEDING POSITIONS ARE NOT ALL FLOOR
                                              "WALK"));
            long result = 0;
            computer.OutputTo(output =>
                              {
                                  if (output < 0 || output > byte.MaxValue)
                                      result = output;
                                  else
                                      Console.Write((char) output);
                              });

            computer.Run();

            return result;
        }

        public object? PartTwo()
        {
            var computer = new IntCodeComputer();
            computer.LoadProgram();


            computer.InputFrom(GenerateInputs("NOT J T", // SET T=1
                                              "OR D J",  // SET J=1 IF LANDING POSITION (D) HAS FLOOR
                                              "AND A T", // WORK OUT IF PRECEDING POSITIONS ARE ALL FLOOR
                                              "AND B T",
                                              "AND C T",
                                              "NOT T T", // SET T=1 ONLY IF PRECEDING POSITIONS ARE NOT ALL FLOOR
                                              "AND T J", // SET J=1 ONLY IF IT WAS ALREADY SET AND THE PRECEDING POSITIONS ARE NOT ALL FLOOR
                                              "AND H T", // SET J=1 ONLY IF IT WAS ALREADY SET AND A SECOND JUMP OR STEP WOULD BE TO FLOOR
                                              "OR E T",
                                              "AND T J",
                                              "RUN"));
            long result = 0;
            computer.OutputTo(output =>
                              {
                                  if (output < 0 || output > byte.MaxValue)
                                      result = output;
                                  else
                                      Console.Write((char) output);
                              });

            computer.Run();

            return result;
        }

        private Queue<long> GenerateInputs(params string[] inputs)
            => new Queue<long>(Encoding.ASCII.GetBytes(string.Join("\n", inputs.Append("")))
                                             .Select(@byte => (long) @byte));
    }
}
