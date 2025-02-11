using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve.SolvingUtilities;

public static class SolvingUtilities<T>
{
    /// <summary>
    /// the function returns a list of cells representing a certain Row.
    /// </summary>
    /// <param name="row"> a certain row </param>
    /// <returns></returns>
    public static List<Cell<T>> GetRowCells(int row,  SudokuBoard<T> grid)
    {
        List<Cell<T>> cells = new List<Cell<T>>();
        for (int col = 0; col < grid.Size; col++)
        {
            cells.Add(grid.BoardGrid[row, col]);
        }
        return cells;
    }


    /// <summary>
    /// the function returns a list of cells representing a certain Column
    /// </summary>
    /// <param name="col"> a certain column</param>
    /// <returns></returns>
    public static List<Cell<T>> GetColumnCells(int col,  SudokuBoard<T> grid)
    {
        List<Cell<T>> cells = new List<Cell<T>>();
        for (int row = 0; row < grid.Size; row++)
        {
            cells.Add(grid.BoardGrid[row, col]);
        }
        return cells;
    }
    /// <summary>
    /// the function returns a list of cells representing a certain box.
    /// </summary>
    /// <param name="boxIndex"> a certain box index.</param>
    /// <returns></returns>
    public static List<Cell<T>> GetBoxCells(int boxIndex,  SudokuBoard<T> grid)
    {
        List<Cell<T>> cells = new List<Cell<T>>();
        foreach ((int row, int col) in grid.GetCellsInBox(boxIndex))
        {
            cells.Add(grid.BoardGrid[row, col]);
        }
        return cells;
    }
}

