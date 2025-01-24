using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve;

/// <summary>
/// this class will represent the human techniques to solve the board
/// and this class inherits from the general ISolving interface that represents the solving techniques
/// </summary>
/// <typeparam name="T"></typeparam>
public class HumanTechniques<T> : ISolving<T>
{
    private SudokuBoard<T> sudokuBoard;
    private int boxSize;
    HashSet<T> possibilities;

    /// <summary>
    /// the function sets the sudoku board property.
    /// and sets the possibilities and box size also.
    /// </summary>
    /// <param name="sudokuBoard"> sudoku board class instance</param>
    public void SetBoard(SudokuBoard<T> sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
        boxSize = sudokuBoard.GetBoxSize();
        possibilities = sudokuBoard.CreatePossibilitySet();
    }

    /// <summary>
    /// main function to call all the solving techniques .
    /// and validate the board
    /// </summary>
    public void Solve()
    {
        
        bool changed = false;
        if (this.sudokuBoard.IsBoardSolved())
            return;
        do
        {
            changed = sudokuBoard.UpdateBoard()
                || LockedCandidatesBlockWithinRowOrCol()
                || LockedCandidatesRowOrColWithinBox();
            Validation.ValidateBoard<T>.Validate(
            sudokuBoard.board, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
        } while (changed && !sudokuBoard.IsBoardSolved());

    }
    /// <summary>
    /// the function checks if there are locked candidates block in a row or a column 
    /// this is the main function , we loop through the rows and check foreach candidate in the possibilities.
    /// </summary>
    /// <returns> returns true if changes were made </returns>
    public bool LockedCandidatesBlockWithinRowOrCol()
    {
        HashSet<T> allPossibilities = sudokuBoard.CreatePossibilitySet();
        bool didChangeRow = false;
        bool didChangeCol = false;

        for (int row = 0; row < sudokuBoard.Size; row++)
        {
            foreach (T candidate in allPossibilities)
            {
                LockedCandidatesBlockWithinRowOrColHelperFunction(row, candidate, ref didChangeRow, ref didChangeCol);
            }
        }
        return didChangeRow || didChangeCol;
    }
    /// <summary>
    /// the function loops through the columns ( you can treat it as rows also ) 
    /// and if the value is non permenant and contains the candidate in his possibilities , 
    /// we add it to dictionary that represent rows and columns or columns and rows.
    /// after the loop is finished i call two helper functions , one for rows and one for cols.
    /// </summary>
    /// <param name="row"> the row to check (it can represent a certain column also ) </param>
    /// <param name="candidate"> the candidate to check</param>
    /// <param name="didChangeRow"> boolean to represent if changes were made to the board in the rows </param>
    /// <param name="didChangeCol"> boolean to represent if changes were made to the board in the cols</param>
    public void LockedCandidatesBlockWithinRowOrColHelperFunction(int row, T candidate, ref bool didChangeRow, ref bool didChangeCol)
    {
        Dictionary<int, List<int>> RowAndCols = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> ColsAndRows = new Dictionary<int, List<int>>();

        for (int col = 0; col < sudokuBoard.Size; col++)
        {
            if (!sudokuBoard.board[row, col].IsPermanent() &&
                sudokuBoard.board[row, col].GetPossibilities().Contains(candidate))
            {
                if (!RowAndCols.ContainsKey(row))
                {
                    RowAndCols[row] = new List<int>();
                }
                RowAndCols[row].Add(col);
            }

            if (!sudokuBoard.board[col, row].IsPermanent() &&
                sudokuBoard.board[col, row].GetPossibilities().Contains(candidate))
            {
                if (!ColsAndRows.ContainsKey(row))
                {
                    ColsAndRows[row] = new List<int>();
                }
                ColsAndRows[row].Add(col);
            }
        }

        LockedCandidatesBlockWithinRowHelperFunctionCondition(RowAndCols, row, candidate, ref didChangeRow);
        LockedCandidatesBlockWithinColHelperFunctionCondition(ColsAndRows, row, candidate, ref didChangeCol);
    }

    /// <summary>
    ///  the function check if this is indeed locked candidates inside the box (or block) 
    ///  it checks if for the values in the dictionary if they are legal and all in the same row.
    ///  if they are , it check if they all in the same box , and if this condition is true,
    ///  it will remove all the possibilities for this certain candidate for the rest of the box excpet them.
    /// </summary>
    /// <param name="RowAndCols"> the dictionary that represent the Rows and columns</param>
    /// <param name="row"> the row to check</param>
    /// <param name="candidate"> the candidate to check </param>
    /// <param name="didChangeRow"> boolean that represents if changes were made</param>
    public void LockedCandidatesBlockWithinRowHelperFunctionCondition(Dictionary<int, List<int>> RowAndCols, int row, T candidate, ref bool didChangeRow)
    {
        if (RowAndCols.ContainsKey(row) && RowAndCols[row].Count >= 1 && RowAndCols[row].Count <= boxSize)
        {
            int boxIndex = sudokuBoard.GetBoxIndex(row, RowAndCols[row][0]);

            bool allInSameBox = RowAndCols[row].All(col => sudokuBoard.GetBoxIndex(row, col) == boxIndex);

            if (allInSameBox)
            {
                List<(int row, int col)> cellsInBox = sudokuBoard.GetCellsInBox(boxIndex);

                foreach (var cell in cellsInBox)
                {
                    if (cell.row != row && !sudokuBoard.board[cell.row, cell.col].IsPermanent() &&
                        sudokuBoard.board[cell.row, cell.col].GetPossibilities().Contains(candidate))
                    {
                        if (!sudokuBoard.board[row, cell.col].GetPossibilities().Contains(candidate))
                        {
                            RemoveCellPossibilityAndUpdate(cell.row, cell.col, candidate);
                            didChangeRow = true;
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    ///  the function check if this is indeed locked candidates inside the box (or block) 
    ///  it checks if for the values in the dictionary if they are legal and all in the same column.
    ///  if they are , it check if they all in the same box , and if this condition is true,
    ///  it will remove all the possibilities for this certain candidate for the rest of the box excpet them.
    /// </summary>
    /// <param name="ColsAndRows"> the dictionary that represent the columns and Rows </param>
    /// <param name="col"> the column to check</param>
    /// <param name="candidate"> the candidate to check </param>
    /// <param name="didChangeCol"> boolean that represents if changes were made</param>
    public void LockedCandidatesBlockWithinColHelperFunctionCondition(Dictionary<int, List<int>> ColsAndRows, int col, T candidate, ref bool didChangeCol)
    {
        if (ColsAndRows.ContainsKey(col) && ColsAndRows[col].Count >= 1 && ColsAndRows[col].Count <= boxSize && sudokuBoard.cols[col].Contains(candidate))
        {
            int boxIndex = sudokuBoard.GetBoxIndex(ColsAndRows[col][0], col);
            bool allInSameBox = ColsAndRows[col].All(row => sudokuBoard.GetBoxIndex(row, col) == boxIndex);

            if (allInSameBox)
            {
                List<(int row, int col)> cellsInBox = sudokuBoard.GetCellsInBox(boxIndex);
                foreach (var cell in cellsInBox)
                {
                    if (cell.col != col && !sudokuBoard.board[cell.row, cell.col].IsPermanent() &&
                        sudokuBoard.board[cell.row, cell.col].GetPossibilities().Contains(candidate))
                    {
                        if (!sudokuBoard.board[cell.row, col].GetPossibilities().Contains(candidate))
                        {
                            RemoveCellPossibilityAndUpdate(cell.row, cell.col, candidate);
                            didChangeCol = true;
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// removes a possibility from a cell and updates the board if that cell is permanent after the possibility is removed.
    /// </summary>
    /// <param name="row"> row of the cell</param>
    /// <param name="col"> column of the cell</param>
    /// <param name="value"> possibility to remvoe</param>
    public void RemoveCellPossibilityAndUpdate(int row, int col, T value)
    {
        if (sudokuBoard.board[row, col].GetPossibilities().Contains(value))
        {

            if (!sudokuBoard.board[row, col].RemovePossibility(value)) return;
            if (sudokuBoard.board[row, col].IsPermanent())
            {
                sudokuBoard.SetCellValue(row, col, sudokuBoard.board[row, col].GetValue());
            }
        }
    }
    /// <summary>
    /// the function checks for locked candidates in row or column that is inside a box, 
    /// meaning if a certain box has a row or column that inside this row or column you have 2 or more possibilities of a value 
    /// and only for this row/column you can remove this possibility for the rest of the row/column
    /// the function loops through out all the boxes and for each box checks all the candidates in it , and calls the helper functions 
    /// for rows, and cols. 
    /// </summary>
    /// <returns> returns true if changes were made </returns>
    public bool LockedCandidatesRowOrColWithinBox()
    {
        bool didChange = false;

        for (int boxIndex = 0; boxIndex < sudokuBoard.Size; boxIndex++)
        {
            foreach (T candidate in possibilities)
            {
                List<(int row, int col)> candidateCells = new List<(int row, int col)>();
                foreach ((int row, int col) in sudokuBoard.GetCellsInBox(boxIndex))
                {
                    if (!sudokuBoard.board[row, col].IsPermanent() &&
                        sudokuBoard.board[row, col].GetPossibilities().Contains(candidate))
                    {
                        candidateCells.Add((row, col));
                    }
                }

                if (candidateCells.Count < 2)
                    continue;
                bool rowChange = LockedCandidatesRowWithinBox(boxIndex, candidate, candidateCells);
                bool colChange = LockedCandidatesColWithinBox(boxIndex, candidate, candidateCells);
                didChange |= (rowChange || colChange);
            }
        }

        return didChange;
    }

    /// <summary>
    /// the function checks first if the candidate cells are only in one row and only,
    /// it they are it updates the rest of the row and remove that candidates possibility in them. 
    /// </summary>
    /// <param name="boxIndex"> a certain box index inside the sudoku board </param>
    /// <param name="candidate"> a value to check inside the box </param>
    /// <param name="candidateCells"> a list that represents each cell in the box .</param>
    /// <returns> return true if changes were made .</returns>
    public bool LockedCandidatesRowWithinBox(int boxIndex, T candidate, List<(int row, int col)> candidateCells)
    {
        bool didChange = false;
        var groupedByRow = candidateCells.GroupBy(cell => cell.row).ToList();
        if (groupedByRow.Count == 1)
        {
            int lockedRow = groupedByRow[0].Key;
            for (int col = 0; col < sudokuBoard.Size; col++)
            {
                if (sudokuBoard.GetBoxIndex(lockedRow, col) != boxIndex)
                {
                    Cell<T> cell = sudokuBoard.board[lockedRow, col];
                    if (!cell.IsPermanent() && cell.GetPossibilities().Contains(candidate))
                    {
                        RemoveCellPossibilityAndUpdate(lockedRow, col, candidate);
                        didChange = true;

                    }
                }
            }
        }

        return didChange;
    }

    /// <summary>
    /// the function checks first if the candidate cells are only in one column and only,
    /// it they are it updates the rest of the column and remove that candidates possibility in them. 
    /// </summary>
    /// <param name="boxIndex"> a certain box index inside the sudoku board </param>
    /// <param name="candidate"> a value to check inside the box </param>
    /// <param name="candidateCells"> a list that represents each cell in the box .</param>
    /// <returns> return true if changes were made .</returns>
    public bool LockedCandidatesColWithinBox(int boxIndex, T candidate, List<(int row, int col)> candidateCells)
    {
        bool didChange = false;
        var groupedByCol = candidateCells.GroupBy(cell => cell.col).ToList();
        if (groupedByCol.Count == 1)
        {
            int lockedCol = groupedByCol[0].Key;
            for (int row = 0; row < sudokuBoard.Size; row++)
            {
                if (sudokuBoard.GetBoxIndex(row, lockedCol) != boxIndex)
                {
                    Cell<T> cell = sudokuBoard.board[row, lockedCol];
                    if (!cell.IsPermanent() && cell.GetPossibilities().Contains(candidate))
                    {
                        RemoveCellPossibilityAndUpdate(row, lockedCol, candidate);
                        didChange = true;
                    }
                }
            }
        }
        return didChange;
    }
}
