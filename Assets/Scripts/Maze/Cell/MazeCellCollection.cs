using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Maze.Cell
{
    public struct MazeCellCollection : IDisposable
    {
        [NativeDisableParallelForRestriction]
        private NativeArray<MazeCellFlags> _cells;
        private readonly int2 _size;
        
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

        public int2 IndexToCoordinates(int index)
        {
            var y = index / _size.x;
            var x = index - _size.x * y;

            return new int2
            {
                x = x,
                y = y
            };
        }

        public Vector3 CoordinatesToWorldPosition(int2 coordinates, float y = 0f)
        {
            return new Vector3
            {
                x = 2f * coordinates.x + 1f - _size.x,
                y = y,
                z = 2f * coordinates.y + 1f - _size.y
            };
        }

        public Vector3 IndexToWorldPosition(int index, float y = 0f)
        {
            return CoordinatesToWorldPosition(IndexToCoordinates(index), y);
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