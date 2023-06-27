using Maze.Cell;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Random = Unity.Mathematics.Random;

namespace Maze.Jobs
{
    [BurstCompile]
    public struct MazeGeneratorJob : IJob
    {
        public MazeCellCollection Cells;
        public float PickLastProbability;
        public float OpenDeadEndProbability;
        public float OpenArbitraryProbability;
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
                var pickLast = random.NextFloat() < PickLastProbability;

                int randomActiveIndex, index;

                if (pickLast)
                {
                    randomActiveIndex = 0;
                    index = activeIndices[lastActiveIndex];
                }
                else
                {
                    randomActiveIndex = random.NextInt(firstActiveIndex, lastActiveIndex + 1);
                    index = activeIndices[randomActiveIndex];
                }

                var availablePassageCount = FindAvailablePassages(index, scratchpad);

                if (availablePassageCount <= 1)
                {
                    if (pickLast)
                    {
                        lastActiveIndex -= 1;
                    }
                    else
                    {
                        activeIndices[randomActiveIndex] = activeIndices[firstActiveIndex++];
                    }
                }

                if (availablePassageCount <= 0) 
                    continue;
                
                var passage = scratchpad[random.NextInt(0, availablePassageCount)];

                Cells.Set(index, passage.Item2);
                Cells[passage.Item1] = passage.Item3;

                activeIndices[++lastActiveIndex] = passage.Item1;
            }

            if (OpenDeadEndProbability > 0f)
            {
                random = OpenDeadEnds(random, scratchpad);
            }

            if (!(OpenArbitraryProbability > 0f))
                return;

            OpenArbitraryPassages(random);
        }

        private Random OpenArbitraryPassages(Random random)
        {
            for (var i = 0; i < Cells.Length; i++)
            {
                var coordinates = MazeCellFlagsExtensions.MazeCellFlagIndexToCoordinates(i, Cells.Size);

                if (coordinates.x > 0 && random.NextFloat() < OpenDeadEndProbability)
                {
                    Cells.Set(i, MazeCellFlags.West);
                    Cells.Set(i + Cells.West, MazeCellFlags.East);
                }

                if (coordinates.y <= 0 || !(random.NextFloat() < OpenArbitraryProbability))
                    continue;

                Cells.Set(i, MazeCellFlags.South);
                Cells.Set(i + Cells.South, MazeCellFlags.North);
            }

            return random;
        }

        private Random OpenDeadEnds(Random random, NativeArray<(int, MazeCellFlags, MazeCellFlags)> scratchpad)
        {
            for (var i = 0; i < Cells.Length; i++)
            {
                var cell = Cells[i];

                if (!cell.HasExactlyOne() || !(random.NextFloat() < OpenDeadEndProbability)) 
                    continue;
                
                var availablePassageCount = FindClosedPassages(i, scratchpad, cell);

                var passage = scratchpad[random.NextInt(0, availablePassageCount)];

                Cells[i] = cell.Add(passage.Item2);
                Cells.Set(i + passage.Item1, passage.Item3);
            }

            return random;
        }

        private int FindClosedPassages(
            int index,
            NativeArray<(int, MazeCellFlags, MazeCellFlags)> scratchpad,
            MazeCellFlags exclude
        )
        {
            var coordinates = MazeCellFlagsExtensions.MazeCellFlagIndexToCoordinates(index, Cells.Size);
            var count = 0;

            if (exclude != MazeCellFlags.East && coordinates.x + 1 < Cells.Width)
            {
                scratchpad[count++] = (Cells.East, MazeCellFlags.East, MazeCellFlags.West);
            }

            if (exclude != MazeCellFlags.West && coordinates.x > 0)
            {
                scratchpad[count++] = (Cells.West, MazeCellFlags.West, MazeCellFlags.East);
            }

            if (exclude != MazeCellFlags.North && coordinates.y + 1 < Cells.Height)
            {
                scratchpad[count++] = (Cells.North, MazeCellFlags.North, MazeCellFlags.South);
            }

            if (exclude != MazeCellFlags.South && coordinates.y > 0)
            {
                scratchpad[count++] = (Cells.South, MazeCellFlags.South, MazeCellFlags.North);
            }

            return count;
        }

        private int FindAvailablePassages(int index, NativeArray<(int, MazeCellFlags, MazeCellFlags)> scratchpad)
        {
            var coordinates = MazeCellFlagsExtensions.MazeCellFlagIndexToCoordinates(index, Cells.Size);
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

            if (coordinates.y <= 0)
                return count;

            index += Cells.South;

            if (Cells[index] == MazeCellFlags.Empty)
            {
                scratchpad[count++] = (index, MazeCellFlags.South, MazeCellFlags.North);
            }
            
            return count;
        }
    }
}