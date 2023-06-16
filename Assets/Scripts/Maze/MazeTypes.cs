using Maze.Cell;
using UnityEngine;

namespace Maze
{
    [CreateAssetMenu(fileName = "Maze Assets", menuName = "Maze/Maze Assets", order = 0)]
    public class MazeTypes : ScriptableObject
    {
        [SerializeField]
        private MazeCell deadEnd;

        [SerializeField]
        private MazeCell straight;

        [SerializeField]
        private MazeCell cornerClosed;

        [SerializeField]
        private MazeCell cornerOpen;

        [SerializeField]
        private MazeCell tJunctionClosed;

        [SerializeField]
        private MazeCell tJunctionOpenNE;

        [SerializeField]
        private MazeCell tJunctionOpenSE;

        [SerializeField]
        private MazeCell tJunctionOpen;

        [SerializeField]
        private MazeCell xJunctionClosed;

        [SerializeField]
        private MazeCell xJunctionOpenNE;

        [SerializeField]
        private MazeCell xJunctionOpenNE_SE;

        [SerializeField]
        private MazeCell xJunctionOpenNE_SW;

        [SerializeField]
        private MazeCell xJunctionClosedNE;

        [SerializeField]
        private MazeCell xJunctionOpen;

        public MazeCell DeadEnd => deadEnd;
        
        public MazeCell Straight => straight;
        
        public MazeCell CornerClosed => cornerClosed;
        
        public MazeCell CornerOpen => cornerOpen;
        
        public MazeCell TJunctionClosed => tJunctionClosed;
        
        public MazeCell TJunctionOpenNE => tJunctionOpenNE;
        
        public MazeCell TJunctionOpenSE => tJunctionOpenSE;
        
        public MazeCell TJunctionOpen => tJunctionOpen;
        
        public MazeCell XJunctionClosed => xJunctionClosed;
        
        public MazeCell XJunctionOpenNE => xJunctionOpenNE;
        
        public MazeCell XJunctionOpenNE_SE => xJunctionOpenNE_SE;
        
        public MazeCell XJunctionOpenNE_SW => xJunctionOpenNE_SW;
        
        public MazeCell XJunctionClosedNE => xJunctionClosedNE;
    }
}