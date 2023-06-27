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
                var prefabWithRotation = MazeVisualisation.GetPrefab(
                    mazeData.Cells[i],
                    mazeTypes
                );

                var instance = prefabWithRotation.Item1.GetInstance();

                instance.transform.SetParent(transform);
                instance.transform.SetPositionAndRotation(
                    MazeCellFlagsExtensions.MazeCellFlagIndexToWorldPosition(i, mazeData.Size),
                    MazeVisualisation.Rotations[(int)prefabWithRotation.Item2]
                );
            }
        }
    }
}