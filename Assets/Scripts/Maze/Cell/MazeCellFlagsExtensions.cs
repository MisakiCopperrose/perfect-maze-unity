using Unity.Mathematics;
using UnityEngine;

namespace Maze.Cell
{
    public static class MazeCellFlagsExtensions
    {
        public static bool Has(this MazeCellFlags flags, MazeCellFlags mask)
        {
            return (flags & mask) == mask;
        }

        public static bool HasAny(this MazeCellFlags flags, MazeCellFlags mask)
        {
            return (flags & mask) != 0;
        }
        
        public static bool HasNot(this MazeCellFlags flags, MazeCellFlags mask)
        {
            return (flags & mask) != mask;
        }

        public static bool HasExactlyOne(this MazeCellFlags flags)
        {
            return flags != 0 && (flags & (flags - 1)) == 0;
        }
        
        public static MazeCellFlags Add(this MazeCellFlags flags, MazeCellFlags mask)
        {
            return flags | mask;
        }

        public static MazeCellFlags Remove(this MazeCellFlags flags, MazeCellFlags mask)
        {
            return flags & ~mask;
        }
        
        public static MazeCellFlags StraightPassages (this MazeCellFlags flags)
        {
            return flags & MazeCellFlags.All;
        }

        public static MazeCellFlags DiagonalPassages (this MazeCellFlags flags)
        {
            return flags & MazeCellFlags.Diagonal;
        }


        public static int2 MazeCellFlagIndexToCoordinates(int index, int2 size)
        {
            var y = index / size.x;
            var x = index - size.x * y;

            return new int2
            {
                x = x,
                y = y
            };
        }

        public static Vector3 MazeCellFlagCoordinatesToWorldPosition(int2 coordinates, int2 size, float y = 0f)
        {
            return new Vector3
            {
                x = 2f * coordinates.x + 1f - size.x,
                y = y,
                z = 2f * coordinates.y + 1f - size.y
            };
        }

        public static Vector3 MazeCellFlagIndexToWorldPosition(int index, int2 size, float y = 0f)
        {
            return MazeCellFlagCoordinatesToWorldPosition(MazeCellFlagIndexToCoordinates(index, size), size, y);
        }
    }
}