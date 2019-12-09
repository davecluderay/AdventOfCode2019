﻿namespace Aoc2019_Day09.Computer
{
    internal enum Opcode
    {
        Add                = 1,
        Multiply           = 2,
        Input              = 3,
        Output             = 4,
        JumpIfTrue         = 5,
        JumpIfFalse        = 6,
        LessThan           = 7,
        Equals             = 8,
        RelativeBaseOffset = 9,
        Halt               = 99
    }
}