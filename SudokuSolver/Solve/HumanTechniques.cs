using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    private bool IsFirstRound;

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
        IsFirstRound = true;
    }

    /// <summary>
    /// main function to call all the solving techniques .
    /// and validate the board
    /// </summary>
    public bool Solve()
    {
        if (IsFirstRound)
        {
            IsFirstRound = false;
            return SolveForFirstRound();
        }
        else
        {
            return SolveForBoards();
        }
    }
    private bool SolveForFirstRound()
    {
        bool didChange = false;
        bool changed = false;
        if(sudokuBoard.Size == 9 )
            changed =  ApplyNakedSets();
        Validation.ValidateBoard<T>.Validate(
            sudokuBoard.board, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
        if (this.sudokuBoard.IsBoardSolved())
            return true;
        do
        {
            changed = sudokuBoard.UpdateBoard()
                 || LockedCandidatesBlockWithinRowOrCol()
                 || LockedCandidatesRowOrColWithinBox()
                 || HiddenSingle();
            didChange = didChange | changed;
            Validation.ValidateBoard<T>.Validate(
             sudokuBoard.board, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
        } while (changed && !sudokuBoard.IsBoardSolved());
        return didChange;
    }

   
    private bool SolveForBoards()
    {
        bool didChange = false;
        bool changed = false;
        if (this.sudokuBoard.IsBoardSolved())
            return true;
        do
        {
            if (this.sudokuBoard.Size <=9)
                changed = sudokuBoard.UpdateBoard()
                        || HiddenSingle();
            if (this.sudokuBoard.Size == 16)
                changed = sudokuBoard.UpdateBoard()
                         || LockedCandidatesBlockWithinRowOrCol()
                        || LockedCandidatesRowOrColWithinBox()
                        || HiddenSingle();

            if (this.sudokuBoard.Size > 16)
                changed = sudokuBoard.UpdateBoard()
                    || HiddenSingle();

            didChange = didChange | changed;
            Validation.ValidateBoard<T>.Validate(
             sudokuBoard.board, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
        } while (changed && !sudokuBoard.IsBoardSolved());
        return didChange;
    }
  


    /// <summary>
    /// the function checks if there are locked candidates block in a row or a column 
    /// this is the main function , we loop through the rows and check foreach candidate in the possibilities.
    /// </summary>
    /// <returns> returns true if changes were made </returns>
    private bool LockedCandidatesBlockWithinRowOrCol()
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
    private void LockedCandidatesBlockWithinRowOrColHelperFunction(int row, T candidate, ref bool didChangeRow, ref bool didChangeCol)
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
    private void LockedCandidatesBlockWithinRowHelperFunctionCondition(Dictionary<int, List<int>> RowAndCols, int row, T candidate, ref bool didChangeRow)
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
    private void LockedCandidatesBlockWithinColHelperFunctionCondition(Dictionary<int, List<int>> ColsAndRows, int col, T candidate, ref bool didChangeCol)
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
    private void RemoveCellPossibilityAndUpdate(int row, int col, T value)
    {
        if (sudokuBoard.board[row, col].GetPossibilities().Contains(value))
        {

            if (!sudokuBoard.board[row, col].RemovePossibility(value)) return;
            if (sudokuBoard.board[row, col].IsPermanent())
            {
                sudokuBoard.RemoveValueFromPossibilities(row, col, sudokuBoard.board[row, col].GetValue());
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
                //bool colChange = LockedCandidatesColWithinBox(boxIndex, candidate, candidateCells);
                didChange |= (rowChange);
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

        if (groupedByRow.Count == 1 && groupedByRow[0].Count() == candidateCells.Count)
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


    ///// <summary>
    ///// the function checks first if the candidate cells are only in one column and only,
    ///// it they are it updates the rest of the column and remove that candidates possibility in them. 
    ///// </summary>
    ///// <param name="boxIndex"> a certain box index inside the sudoku board </param>
    ///// <param name="candidate"> a value to check inside the box </param>
    ///// <param name="candidateCells"> a list that represents each cell in the box .</param>
    ///// <returns> return true if changes were made .</returns>
    //public bool LockedCandidatesColWithinBox(int boxIndex, T candidate, List<(int row, int col)> candidateCells)
    //{
    //    bool didChange = false;
    //    var groupedByCol = candidateCells.GroupBy(cell => cell.col).ToList();
    //    if (groupedByCol.Count == 1 && groupedByCol[0].Count() == candidateCells.Count)
    //    {
    //        int lockedCol = groupedByCol[0].Key;
    //        for (int row = 0; row < sudokuBoard.Size; row++)
    //        {
    //            if (sudokuBoard.GetBoxIndex(row, lockedCol) != boxIndex)
    //            {
    //                Cell<T> cell = sudokuBoard.board[row, lockedCol];
    //                if (!cell.IsPermanent() && cell.GetPossibilities().Contains(candidate))
    //                {
    //                    RemoveCellPossibilityAndUpdate(row, lockedCol, candidate);
    //                    didChange = true;
    //                }
    //            }
    //        }
    //    }
    //    return didChange;
    //}






    /// <summary>
    /// main function to call when wanting to summon the naked sets checking.
    /// the function checks from 2 pairs, to the board size -2 (included) .
    /// </summary>
    /// <returns> return true if changes were made .</returns>
    private bool ApplyNakedSets()
    {
        bool didChange = false;
        for (int SetSize = 2; SetSize < 7; SetSize++)
           didChange |= NakedSet(SetSize);
        return didChange;
    }
    /// <summary>
    /// this function loops through the board size, and for each location , it will call the
    /// main function to find the sets -> it sends her each iteration, for each location in the board size 
    /// ( from 1 to the size of the board ) , all the rows,cols, and boxes cells.
    /// </summary>
    /// <param name="SetSize"> the size of the set to find (pairs, triples) ... </param>
    /// <returns> returns true if changes in the board were made. </returns>
    private bool NakedSet(int SetSize)
    {
        bool didChange = false;
        for (int location = 0; location < sudokuBoard.Size; location++)
        {
            if (NakedSetFind(GetRowCells(location), SetSize))
                didChange = true;
            if (NakedSetFind(GetColumnCells(location), SetSize))
                didChange = true;
            if (NakedSetFind(GetBoxCells(location), SetSize))
                didChange = true;
        }
        return didChange;
    }

    /// <summary>
    /// This is the main function to find the naked sets.
    /// The function takes an input a list of cells in a row/col/box and the set size (pairs, triples).
    /// The function generates all the potential Sets of the row/col/box cells.
    /// And for each Set it checks whether it is a valid set for naked sets ->
    /// (A set of cells in which each possibility is unique to that set, meaning that no other cells in the same ->
    /// row, column, or box can contain those possibilities.)
    /// If a valid set is found , All the possibilities from this set will be removed from the other Cells 
    /// int the row/col/box.
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="setSize"></param>
    /// <returns></returns>
    private bool NakedSetFind(List<Cell<T>> cells, int setSize)
    {
        bool didChange = false;
        List<List<Cell<T>>> potentialSets = GetCombinations(cells, setSize);

        foreach (List<Cell<T>> set in potentialSets)
        {
            HashSet<T> nakedCandidates = new HashSet<T>();

            foreach (Cell<T> cell in set)
                foreach (T possibility in cell.GetPossibilities())
                   nakedCandidates.Add(possibility); 

            if (nakedCandidates.Count() == setSize)
                foreach (Cell<T> cell in cells)
                    if (!cell.IsPermanent()&& !set.Contains(cell))         
                        foreach (T candidate in nakedCandidates)
                            if (cell.RemovePossibility(candidate))
                                didChange = true;  
        }
        return didChange;
    }


    /// <summary>
    ///The function receives a list of Cells representing a row/col/box , 
    ///and a setSize , the function will recursively generate all the sets of cells that are possibile
    /// and return them.
    /// </summary>
    /// <param name="Cells"></param>
    /// <param name="SetSize"></param>
    /// <returns></returns>
    private List<List<Cell<T>>> GetCombinations(List<Cell<T>> Cells, int SetSize)
    {
        List<List<Cell<T>>> combinations = new List<List<Cell<T>>>();

        if (SetSize == 0)
        {
            combinations.Add(new List<Cell<T>>());
            return combinations;
        }

        for (int i = 0; i <= Cells.Count - SetSize; i++)
        {
            //recursive call to find all the possibile combinations of the setsize -1 , from the remaining cells --> Cells.Skip(i + 1)
            List<List<Cell<T>>> remainingCombinations = GetCombinations(Cells.Skip(i + 1).ToList(), SetSize - 1);

            foreach (List<Cell<T>> combination in remainingCombinations)
            {
                List<Cell<T>> result = new List<Cell<T>> { Cells[i] };
                result.AddRange(combination);
                combinations.Add(result);
            }
        }
        return combinations;
    }


    /// <summary>
    /// the function returns a list of cells representing a certain Row.
    /// </summary>
    /// <param name="row"> a certain row </param>
    /// <returns></returns>
    private List<Cell<T>> GetRowCells(int row)
    {
        List<Cell<T>> cells = new List<Cell<T>>();
        for (int col = 0; col < sudokuBoard.Size; col++)
        {
            cells.Add(sudokuBoard.board[row, col]);
        }
        return cells;
    }


    /// <summary>
    /// the function returns a list of cells representing a certain Column
    /// </summary>
    /// <param name="col"> a certain column</param>
    /// <returns></returns>
    private List<Cell<T>> GetColumnCells(int col)
    {
        List<Cell<T>> cells = new List<Cell<T>>();
        for (int row = 0; row < sudokuBoard.Size; row++)
        {
            cells.Add(sudokuBoard.board[row, col]);
        }
        return cells;
    }
    /// <summary>
    /// the function returns a list of cells representing a certain box.
    /// </summary>
    /// <param name="boxIndex"> a certain box index.</param>
    /// <returns></returns>
    private List<Cell<T>> GetBoxCells(int boxIndex)
    {
        List<Cell<T>> cells = new List<Cell<T>>();
        foreach ((int row, int col) in sudokuBoard.GetCellsInBox(boxIndex))
        {
            cells.Add(sudokuBoard.board[row, col]);
        }
        return cells;
    }

    private bool HiddenSingle()
    {
        bool didChange = false;
        for (int location = 0; location < sudokuBoard.Size; location++)
        {
            if (HiddenSingleFind(GetRowCells(location)))
                didChange = true;
            if (HiddenSingleFind(GetColumnCells(location)))
                didChange = true;
            if (HiddenSingleFind(GetBoxCells(location)))
                didChange = true;
        }
        return didChange;
    }
    private bool HiddenSingleFind(List<Cell<T>> Cells) 
    {
        bool didChange = false;
        int row = 0, col = 0;
        int count = 0; 
        bool flag = false;

        foreach(T option in sudokuBoard.CreatePossibilitySet())
        {
            count = 0;
            flag = true;
            foreach (Cell<T> cell in Cells)
            {
                if (cell.IsPermanent() && cell.GetValue().Equals(option))
                    flag = false;
                if(!cell.IsPermanent() && cell.GetPossibilities().Contains(option))
                {
                    count++;
                    row = cell.GetRow();    
                    col = cell.GetCol();    
                }
            }
            if(count == 1 && flag)
                sudokuBoard.SetCellValue(row,col,option);     
        }
        return false;
    }
}
