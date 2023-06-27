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
        private MazeCell tJunctionOpenNe;
        
        [SerializeField]
        private MazeCell tJunctionOpenSe;

        [SerializeField]
        private MazeCell tJunctionOpen;

        [SerializeField]
        private MazeCell xJunctionClosed;

        [SerializeField]
        private MazeCell xJunctionOpenNe;

        [SerializeField]
        private MazeCell xJunctionOpenNeSe;

        [SerializeField]
        private MazeCell xJunctionOpenNeSw;

        [SerializeField]
        private MazeCell xJunctionClosedNe;

        [SerializeField]
        private MazeCell xJunctionOpen;

        public MazeCell DeadEnd => deadEnd;
        
        public MazeCell Straight => straight;
        
        public MazeCell CornerClosed => cornerClosed;
        
        public MazeCell CornerOpen => cornerOpen;
        
        public MazeCell TJunctionClosed => tJunctionClosed;
        
        public MazeCell TJunctionOpenNe => tJunctionOpenNe;
        
        public MazeCell TJunctionOpenSe => tJunctionOpenSe;
        
        public MazeCell TJunctionOpen => tJunctionOpen;
        
        public MazeCell XJunctionClosed => xJunctionClosed;
        
        public MazeCell XJunctionOpenNe => xJunctionOpenNe;
        
        public MazeCell XJunctionOpenNeSe => xJunctionOpenNeSe;
        
        public MazeCell XJunctionOpenNeSw => xJunctionOpenNeSw;
        
        public MazeCell XJunctionClosedNe => xJunctionClosedNe;
        
        public MazeCell XJunctionOpen => xJunctionOpen;
    }
}