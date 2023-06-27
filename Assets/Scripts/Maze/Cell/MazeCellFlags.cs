using System;

namespace Maze.Cell
{
    [Flags]
    public enum MazeCellFlags : byte
    {
        Empty = 0,
        
        North = 1 << 0,
        East = 1 << 1,
        South = 1 << 2,
        West = 1 << 3,
        
        All = North | East | South | West,
        
        NorthEast = 1 << 4,
        SouthEast = 1 << 5,
        SouthWest = 1 << 6,
        NorthWest = 1 << 7,
        
        Diagonal = NorthEast | SouthEast | SouthWest | NorthWest,
    }
}