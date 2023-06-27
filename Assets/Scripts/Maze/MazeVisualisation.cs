using Maze.Cell;
using UnityEngine;

namespace Maze
{
    public static class MazeVisualisation
    {
        public static readonly Quaternion[] Rotations =
        {
            Quaternion.identity,
            Quaternion.Euler(0f, 90f, 0f),
            Quaternion.Euler(0f, 180f, 0f),
            Quaternion.Euler(0f, 270f, 0f)
        };

        public static (MazeCell, MazeCellRotation) GetPrefab(MazeCellFlags flags, MazeTypes mazeTypes)
        {
            return flags.StraightPassages() switch
            {
                MazeCellFlags.North => (mazeTypes.DeadEnd, MazeCellRotation.Rotation0),
                MazeCellFlags.East => (mazeTypes.DeadEnd, MazeCellRotation.Rotation90),
                MazeCellFlags.South => (mazeTypes.DeadEnd, MazeCellRotation.Rotation180),
                MazeCellFlags.West => (mazeTypes.DeadEnd, MazeCellRotation.Rotation270),

                MazeCellFlags.North | MazeCellFlags.South => (mazeTypes.Straight, MazeCellRotation.Rotation0),
                MazeCellFlags.East | MazeCellFlags.West => (mazeTypes.Straight, MazeCellRotation.Rotation90),

                MazeCellFlags.North | MazeCellFlags.East => GetCorner(flags, MazeCellRotation.Rotation0, mazeTypes),
                MazeCellFlags.East | MazeCellFlags.South => GetCorner(flags, MazeCellRotation.Rotation90, mazeTypes),
                MazeCellFlags.South | MazeCellFlags.West => GetCorner(flags, MazeCellRotation.Rotation180, mazeTypes),
                MazeCellFlags.West | MazeCellFlags.North => GetCorner(flags, MazeCellRotation.Rotation270, mazeTypes),

                MazeCellFlags.All & ~MazeCellFlags.West => GetTJunction(flags, MazeCellRotation.Rotation0, mazeTypes),
                MazeCellFlags.All & ~MazeCellFlags.North => GetTJunction(flags, MazeCellRotation.Rotation90, mazeTypes),
                MazeCellFlags.All & ~MazeCellFlags.East => GetTJunction(flags, MazeCellRotation.Rotation180, mazeTypes),
                MazeCellFlags.All & ~MazeCellFlags.South => GetTJunction(flags, MazeCellRotation.Rotation270, mazeTypes),

                _ => GetXJunction(flags, mazeTypes)
            };
        }
        
        public static (MazeCell, MazeCellRotation) GetXJunction (MazeCellFlags flags, MazeTypes mazeTypes)
        {
            return flags.DiagonalPassages() switch
            {
                MazeCellFlags.Empty => (mazeTypes.XJunctionClosed, MazeCellRotation.Rotation0),

                MazeCellFlags.NorthEast => (mazeTypes.XJunctionOpenNe, MazeCellRotation.Rotation0),
                MazeCellFlags.SouthEast => (mazeTypes.XJunctionOpenNe, MazeCellRotation.Rotation90),
                MazeCellFlags.SouthWest => (mazeTypes.XJunctionOpenNe, MazeCellRotation.Rotation180),
                MazeCellFlags.NorthWest => (mazeTypes.XJunctionOpenNe, MazeCellRotation.Rotation270),

                MazeCellFlags.NorthEast | MazeCellFlags.SouthEast => (mazeTypes.XJunctionOpenNeSe, MazeCellRotation.Rotation0),
                MazeCellFlags.SouthEast | MazeCellFlags.SouthWest => (mazeTypes.XJunctionOpenNeSe, MazeCellRotation.Rotation90),
                MazeCellFlags.SouthWest | MazeCellFlags.NorthWest => (mazeTypes.XJunctionOpenNeSe, MazeCellRotation.Rotation180),
                MazeCellFlags.NorthWest | MazeCellFlags.NorthEast => (mazeTypes.XJunctionOpenNeSe, MazeCellRotation.Rotation270),

                MazeCellFlags.NorthEast | MazeCellFlags.SouthWest => (mazeTypes.XJunctionOpenNeSw, MazeCellRotation.Rotation0),
                MazeCellFlags.SouthEast | MazeCellFlags.NorthWest => (mazeTypes.XJunctionOpenNeSw, MazeCellRotation.Rotation90),

                MazeCellFlags.Diagonal & ~MazeCellFlags.NorthEast => (mazeTypes.XJunctionClosedNe, MazeCellRotation.Rotation0),
                MazeCellFlags.Diagonal & ~MazeCellFlags.SouthEast => (mazeTypes.XJunctionClosedNe, MazeCellRotation.Rotation90),
                MazeCellFlags.Diagonal & ~MazeCellFlags.SouthWest => (mazeTypes.XJunctionClosedNe, MazeCellRotation.Rotation180),
                MazeCellFlags.Diagonal & ~MazeCellFlags.NorthWest => (mazeTypes.XJunctionClosedNe, MazeCellRotation.Rotation270),

                _ => (mazeTypes.XJunctionOpen, MazeCellRotation.Rotation0),
            };
        }

        public static (MazeCell, MazeCellRotation) GetTJunction(
            MazeCellFlags flags,
            MazeCellRotation rotation,
            MazeTypes mazeTypes
        )
        {
            return (
                flags.RotatedDiagonalPassages(rotation) switch
                {
                    MazeCellFlags.Empty => mazeTypes.TJunctionClosed,
                    MazeCellFlags.NorthEast => mazeTypes.TJunctionOpenNe,
                    MazeCellFlags.SouthEast => mazeTypes.TJunctionOpenSe,
                    _ => mazeTypes.TJunctionOpen
                },
                rotation
            );
        }

        public static (MazeCell, MazeCellRotation) GetCorner(
            MazeCellFlags flags,
            MazeCellRotation rotation,
            MazeTypes mazeTypes
        )
        {
            return (
                flags.HasAny(MazeCellFlags.Diagonal) ? mazeTypes.CornerOpen : mazeTypes.CornerClosed, rotation
            );
        }
    }
}