using Maze.Cell;
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
        
        private MazeCellCollection _cells;

        public int CellCount => size.x * size.y;

        public MazeCellCollection Cells
        {
            get
            {
                if (_cells.Length == CellCount) 
                    return _cells;
                
                var cells = new MazeCellCollection(size);
                    
                new MazeGeneratorJob
                {
                    Cells = cells,
                    Seed = seed is 0 ? (uint) Random.Range(0, int.MaxValue) : seed
                }.Schedule().Complete();
                    
                _cells = cells;

                return _cells;
            }
        }

        private void OnDestroy()
        {
            _cells.Dispose();
        }
    }
}