using Maze.Cell;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Maze
{
    [BurstCompile]
    public struct MazeGeneratorJob : IJob
    {
        public MazeCellCollection Cells;

        public uint Seed;

        public void Execute()
        {
            var random = new Random(Seed);

            var scratchpad = new NativeArray<(int, MazeCellFlags, MazeCellFlags)>(
                4, Allocator.Temp, NativeArrayOptions.UninitializedMemory
            );

            var activeIndices = new NativeArray<int>(
                Cells.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory
            );

            var firstActiveIndex = 0;
            var lastActiveIndex = 0;

            activeIndices[firstActiveIndex] = random.NextInt(Cells.Length);

            while (firstActiveIndex <= lastActiveIndex)
            {
                var index = activeIndices[lastActiveIndex];

                var availablePassageCount = FindAvailablePassages(index, scratchpad);

                if (availablePassageCount <= 1)
                {
                    lastActiveIndex -= 1;
                }

                if (availablePassageCount > 0)
                {
                    var passage = scratchpad[random.NextInt(0, availablePassageCount)];
                    
                    Cells[index] = Cells[index].Add(passage.Item2);
                    Cells[passage.Item1] = passage.Item3;
                    
                    activeIndices[++lastActiveIndex] = passage.Item1;
                }
            }
        }

        private int FindAvailablePassages(int index, NativeArray<(int, MazeCellFlags, MazeCellFlags)> scratchpad)
        {
            var coordinates = Cells.IndexToCoordinates(index);
            var count = 0;

            if (coordinates.x + 1 < Cells.Width)
            {
                var i = index + Cells.East;

                if (Cells[i] == MazeCellFlags.Empty)
                {
                    scratchpad[count++] = (i, MazeCellFlags.East, MazeCellFlags.West);
                }
            }

            if (coordinates.x > 0)
            {
                var i = index + Cells.West;

                if (Cells[i] == MazeCellFlags.Empty)
                {
                    scratchpad[count++] = (i, MazeCellFlags.West, MazeCellFlags.East);
                }
            }

            if (coordinates.y + 1 < Cells.Height)
            {
                var i = index + Cells.North;

                if (Cells[i] == MazeCellFlags.Empty)
                {
                    scratchpad[count++] = (i, MazeCellFlags.North, MazeCellFlags.South);
                }
            }

            if (coordinates.y > 0)
            {
                var nextIndex = index + Cells.South;

                if (Cells[nextIndex] == MazeCellFlags.Empty)
                {
                    scratchpad[count++] = (nextIndex, MazeCellFlags.South, MazeCellFlags.North);
                }
            }

            return count;
        }
    }
}