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
    }
}