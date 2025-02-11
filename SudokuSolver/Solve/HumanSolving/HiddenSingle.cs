using SudokuSolver.DataStructures.Board;
using SudokuSolver.Solve.SolvingUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve.HumanHeuristics;

public static class HiddenSingle<T>
{
    /// <summary>
    /// main function for the hidden single, 
    /// hidden singe - if in a certain row,col, or box , you have one possibility only for a option
    /// than that option should be put as permenant for the cell holding that option. 
    /// the function checks for each row col and box and calls the Helper function,
    /// to check for each row col and box in the board.
    /// </summary>
    /// <returns> returns true if changes were made . </returns>
    public static bool HiddenSingles(SudokuBoard<T> grid)
    {
        bool didChange = false;
        for (int location = 0; location < grid.Size; location++)
        {
            if (HiddenSingleFind(SolvingUtilities<T>.GetRowCells(location, grid), grid))
                didChange = true;
            if (HiddenSingleFind(SolvingUtilities<T>.GetColumnCells(location, grid), grid))
                didChange = true;
            if (HiddenSingleFind(SolvingUtilities<T>.GetBoxCells(location, grid), grid))
                didChange = true;
        }
        return didChange;
    }
    /// <summary>
    /// the function recieves a list of cells that represents a certain row/col/box 
    /// and check if there is hidden single.
    /// </summary>
    /// <param name="Cells"></param>
    /// <returns> returns true if changes were made.  </returns>
    public static bool HiddenSingleFind(List<Cell<T>> Cells, SudokuBoard<T> grid)
    {
        bool didChange = false;
        int row = 0, col = 0;
        int count = 0;
        bool flag = false;

        foreach (T option in grid.CreatePossibilitySet())
        {
            count = 0;
            flag = true;
            foreach (Cell<T> cell in Cells)
            {
                if (cell.IsPermanent() && cell.GetValue().Equals(option))
                    flag = false;

                if (!cell.IsPermanent() && cell.GetPossibilities().Contains(option))
                {
                    count++;
                    row = cell.GetRow();
                    col = cell.GetCol();
                }
            }

            if (count == 1 && flag)
            {
                grid.SetCellValue(row, col, option);
                didChange = true;
            }
        }
        return didChange;
    }
}

