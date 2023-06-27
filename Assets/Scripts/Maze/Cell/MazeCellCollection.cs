using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;

namespace Maze.Cell
{
    public struct MazeCellCollection : IDisposable
    {
        [NativeDisableParallelForRestriction]
        private NativeArray<MazeCellFlags> _cells;
        private readonly int2 _size;
        
        public int2 Size => _size;
        
        public int Width => _size.x;
        
        public int Height => _size.y;
        
        public int North => _size.x;
        
        public int East => 1;
        
        public int South => -_size.x;
        
        public int West => -1;
        
        public int Length => _cells.Length;

        public MazeCellCollection(int2 size)
        {
            _size = size;
            _cells = new NativeArray<MazeCellFlags>(size.x * size.y, Allocator.Persistent);
        }

        public List<MazeCellFlags> ToList()
        {
            return _cells.ToList();
        }

        public MazeCellFlags this[int index]
        {
            get => _cells[index];
            set => _cells[index] = value;
        }
        
        public MazeCellFlags Set (int index, MazeCellFlags mask)
        {
            return _cells[index] = _cells[index].Add(mask);
        }

        public MazeCellFlags Unset (int index, MazeCellFlags mask)
        {
            return _cells[index] = _cells[index].Remove(mask);
        }

        public void Dispose()
        {
            if (_cells.IsCreated)
            {
                _cells.Dispose();
            }
        }
    }
}