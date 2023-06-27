using Maze.Cell;
using Unity.Jobs;

namespace Maze.Jobs
{
    public struct MazeFindDiagonalJob : IJobFor
    {
        public MazeCellCollection Cells;

        public void Execute(int index)
        {
            var cell = Cells[index];

            if (
                cell.Has(MazeCellFlags.North | MazeCellFlags.East) &&
                Cells[index + Cells.North + Cells.East].Has(MazeCellFlags.South | MazeCellFlags.West)
            )
            {
                cell = cell.Add(MazeCellFlags.NorthEast);
            }

            if (
                cell.Has(MazeCellFlags.North | MazeCellFlags.West) &&
                Cells[index + Cells.North + Cells.West].Has(MazeCellFlags.South | MazeCellFlags.East)
            )
            {
                cell = cell.Add(MazeCellFlags.NorthWest);
            }

            if (
                cell.Has(MazeCellFlags.South | MazeCellFlags.East) &&
                Cells[index + Cells.South + Cells.East].Has(MazeCellFlags.North | MazeCellFlags.West)
            )
            {
                cell = cell.Add(MazeCellFlags.SouthEast);
            }

            if (
                cell.Has(MazeCellFlags.South | MazeCellFlags.West) &&
                Cells[index + Cells.South + Cells.West].Has(MazeCellFlags.North | MazeCellFlags.East)
            )
            {
                cell = cell.Add(MazeCellFlags.SouthWest);
            }

            Cells[index] = cell;
        }
    }
}