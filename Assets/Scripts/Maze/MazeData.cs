using System.Collections.Generic;
using Maze.Cell;
using Maze.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Jobs;
using Random = UnityEngine.Random;

namespace Maze
{
    [CreateAssetMenu(fileName = "Maze Data", menuName = "Maze/Maze Data", order = 0)]
    public class MazeData : ScriptableObject
    {
        [SerializeField]
        private int2 size;
        
        [SerializeField]
        private uint seed;
        
        [SerializeField, Range(0f, 1f)]
        private float pickLastProbability = 0.5f;
        
        [SerializeField, Range(0f, 1f)]
        private float openDeadEndProbability = 0.5f;
        
        [SerializeField, Range(0f, 1f)]
        private float openArbitraryProbability = 0.25f;
        
        private List<MazeCellFlags> _cells;

        public int2 Size => size;

        public int CellCount => size.x * size.y;

        public IReadOnlyList<MazeCellFlags> Cells
        {
            get
            {
                var cells = new MazeCellCollection(size);
                    
                new MazeGeneratorJob
                {
                    Cells = cells,
                    Seed = seed is 0 ? (uint) Random.Range(0, int.MaxValue) : seed,
                    PickLastProbability = pickLastProbability,
                    OpenDeadEndProbability = openDeadEndProbability,
                    OpenArbitraryProbability = openArbitraryProbability
                }.Schedule().Complete();
                    
                _cells = cells.ToList();
                
                cells.Dispose();

                return _cells;
            }
        }
    }
}