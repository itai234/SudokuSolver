using SudokuSolver.DataStructures.Board;
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
    HashSet<T> possibilities;
    HumanTechniques<T> humanTechniques;

    /// <summary>
    /// sets the sudoku board class instance to be in this class for easy access.
    /// </summary>
    /// <param name="sudokuBoard"></param>
    public void SetBoard(SudokuBoard<T> sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
        possibilities = sudokuBoard.CreatePossibilitySet();
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
        return didSolve;

    }

    private bool BackTracking()
    {
        Validation.ValidateBoard<T>.Validate(
      sudokuBoard.board, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);

        if (sudokuBoard.IsBoardSolved())
            return true;

        var cell = sudokuBoard.FindCellWithLeastPossibilities();
        if (cell == null)
            return false;

        int Row = cell.Value.row;
        int Col = cell.Value.col;
        HashSet<T> cellPossibilities = sudokuBoard.board[Row, Col].GetPossibilities();



        foreach (T possibility in cellPossibilities)
        {
            if (sudokuBoard.CanPlaceValue(Row, Col, possibility))
            {
                var savedState = sudokuBoard.SaveBoardState();
                var savedRows = sudokuBoard.SaveRowsState();
                var savedCols = sudokuBoard.SaveColsState();
                var savedBoxes = sudokuBoard.SaveBoxesState();
                sudokuBoard.SetCellValue(Row, Col, possibility);
                try
                {
                    humanTechniques.Solve();
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
}


