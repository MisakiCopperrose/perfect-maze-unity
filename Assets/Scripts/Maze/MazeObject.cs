using Attributes;
using Maze.Cell;
using UnityEngine;

namespace Maze
{
    public class MazeObject : MonoBehaviour
    {
        [SerializeField] [Expandable]
        private MazeTypes mazeTypes;

        [SerializeField] [Expandable]
        private MazeData mazeData;

        private void Awake()
        {
            for (var i = 0; i < mazeData.CellCount; i++)
            {
                var prefabWithRotation = GetPrefab(mazeData.Cells[i]);

                var instance = prefabWithRotation.Item1.GetInstance();
                
                instance.transform.SetParent(transform);
                instance.transform.SetPositionAndRotation(
                    mazeData.Cells.IndexToWorldPosition(i), Rotations[prefabWithRotation.Item2]
                );
            }
        }

        private static readonly Quaternion[] Rotations =
        {
            Quaternion.identity,
            Quaternion.Euler(0f, 90f, 0f),
            Quaternion.Euler(0f, 180f, 0f),
            Quaternion.Euler(0f, 270f, 0f)
        };

        private (MazeCell, int) GetPrefab(MazeCellFlags flags) => flags switch
        {
            MazeCellFlags.North => (mazeTypes.DeadEnd, 0),
            MazeCellFlags.East => (mazeTypes.DeadEnd, 1),
            MazeCellFlags.South => (mazeTypes.DeadEnd, 2),
            MazeCellFlags.West => (mazeTypes.DeadEnd, 3),

            MazeCellFlags.North | MazeCellFlags.South => (mazeTypes.Straight, 0),
            MazeCellFlags.East | MazeCellFlags.West => (mazeTypes.Straight, 1),

            MazeCellFlags.North | MazeCellFlags.East => (mazeTypes.CornerOpen, 0),
            MazeCellFlags.East | MazeCellFlags.South => (mazeTypes.CornerOpen, 1),
            MazeCellFlags.South | MazeCellFlags.West => (mazeTypes.CornerOpen, 2),
            MazeCellFlags.West | MazeCellFlags.North => (mazeTypes.CornerOpen, 3),

            MazeCellFlags.All & ~MazeCellFlags.West => (mazeTypes.TJunctionOpen, 0),
            MazeCellFlags.All & ~MazeCellFlags.North => (mazeTypes.TJunctionOpen, 1),
            MazeCellFlags.All & ~MazeCellFlags.East => (mazeTypes.TJunctionOpen, 2),
            MazeCellFlags.All & ~MazeCellFlags.South => (mazeTypes.TJunctionOpen, 3),

            _ => (mazeTypes.XJunctionOpenNE, 0)
        };
    }
}