using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve;

/// <summary>
///  this class with represent the Computer Techniques to solve the board such as backTracking 
///  the class inherits from the interface I-solving that will represent the general techniques for solving.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ComputerTechniques<T> : ISolving<T>
{
    private SudokuBoard<T> sudokuBoard;
    HumanTechniques<T> humanTechniques;


    /// <summary>
    /// sets the sudoku board class instance to be in this class for easy access.
    /// also resets the human techniques instance.
    /// </summary>
    /// <param name="sudokuBoard"></param>
    public void SetBoard(SudokuBoard<T> sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
        humanTechniques = new HumanTechniques<T>();
        humanTechniques.SetBoard(sudokuBoard);
    }

    /// <summary>
    /// main function to call all the solving techniques
    /// </summary>
    public bool Solve()
    {
        bool didSolve = false;
        didSolve = BackTracking();
        Validation.ValidateBoard<T>.Validate(
        sudokuBoard.board, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
        if (didSolve == false) throw new UnsolvableBoardException("Board is Unsolvable.");
        return didSolve;

    }
    /// <summary>
    /// the function backtracks and uses brute forcing for the values, 
    /// also using the human techniques to solve the board, and uses also special heuristics to help the solving process to be faster.
    /// </summary>
    /// <returns></returns>
    private bool BackTracking()
    {
        if (sudokuBoard.IsBoardSolved())
            return true;

        var cell = sudokuBoard.FindCellWithLeastPossibilities();
        if (cell == null)
            return false;

        int row = cell.Value.row;
        int col = cell.Value.col;
        HashSet<T> cellPossibilities = sudokuBoard.board[row, col].GetPossibilities();

        List<T> orderedValues = GetValuesOrderedByPriority(row, col, cellPossibilities);


        foreach (T value in orderedValues)
        {
            if (sudokuBoard.CanPlaceValue(row, col, value))
            {
                var savedState = sudokuBoard.SaveBoardState();
                var savedRows = sudokuBoard.SaveRowsState();
                var savedCols = sudokuBoard.SaveColsState();
                var savedBoxes = sudokuBoard.SaveBoxesState();
                sudokuBoard.SetCellValue(row, col, value);

                try
                {
                    humanTechniques.Solve();
                    if (sudokuBoard.IsBoardSolved())
                        return true;
                    if (BackTracking())
                        return true;
                }
                catch (Exception ex)
                {
                }
                sudokuBoard.RestoreBoardState(savedState);
                sudokuBoard.RestorePropertiesState(savedRows, savedCols, savedBoxes);
            }
        }
        return false;
    }




    /// <summary>
    /// the function recieves a cell indexes and his possibillities of values , 
    /// the function creates a list of the value constraints , and checks for each possibility
    /// how many cells on his row/col/box contain it , and adds it to the list.
    /// the list is after this sorted by the constrainting count effect of each value,
    /// and then you convert it to a list of only the values themselves.
    /// </summary>
    /// <param name="row"> the row of the cell </param>
    /// <param name="col"> the column of the cell </param>
    /// <param name="cellPossibilities"> the hashset of the possibilities of the cell</param>
    /// <returns></returns>
    private List<T> GetValuesOrderedByPriority(int row, int col, HashSet<T> cellPossibilities)
    {
        List<(T value, int constrainingCount)> valueConstraints = new List<(T, int)>();
        List<(int row, int col)> Cells = sudokuBoard.GetCellsBesidesItself(row, col);

        foreach (T value in cellPossibilities)
        {
            int constrainingCount = 0;
            foreach ((int row, int col) cell in Cells)
                if (!sudokuBoard.board[row, col].IsPermanent() &&
                    sudokuBoard.board[row, col].GetPossibilities().Contains(value))
                    constrainingCount++;
            valueConstraints.Add((value, constrainingCount));
        }

        return valueConstraints.OrderBy(valueConst => valueConst.constrainingCount).Select(valueConst => valueConst.value).ToList();
    }

}


