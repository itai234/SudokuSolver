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
    private HashSet<string> visitedStates = new HashSet<string>();


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
    /// also using the human techniques to solve the board.
    /// </summary>
    /// <returns></returns>
    private bool BackTracking()
    {
        
        if (sudokuBoard.IsBoardSolved())
            return true;

        var cell = sudokuBoard.FindCellWithLeastPossibilities();
        if (cell == null)
            return false;
    
        int Row = cell.Value.row;
        int Col = cell.Value.col;
        HashSet<T> cellPossibilities = sudokuBoard.board[Row, Col].GetPossibilities();

        string CurrentState  = GetBoardState();
        if (visitedStates.Contains(CurrentState))
            return false;
        visitedStates.Add(CurrentState);
       
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
                    if(sudokuBoard.IsBoardSolved()) return true;
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
    /// the function returns a string representing the board state. 
    /// the purpose is to not go to loops in the backtracking
    /// </summary>
    /// <returns></returns>
    private string GetBoardState()
    {
        StringBuilder boardString = new StringBuilder();
        for (int row = 0; row < sudokuBoard.Size; row++)
        {
            for (int col = 0; col < sudokuBoard.Size; col++)
            {
                boardString.Append(sudokuBoard.board[row, col].IsPermanent()? sudokuBoard.board[row, col].GetValue().ToString(): "0"); 
            }
        }
        return boardString.ToString();
    }
}


