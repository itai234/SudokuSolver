using SudokuSolver.DataStructures.Board;
using SudokuSolver.Solve.HumanHeuristics;
using SudokuSolver.Solve.HumanSolving;
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
    private SudokuBoard<T> _sudokuBoard;
    private bool _isFirstRound;
    private int _minNumForSets = 2;
    private int _maxNumForSets = 7;

    /// <summary>
    /// the function sets the sudoku board property.
    /// and sets the possibilities and box size also.
    /// </summary>
    /// <param name="sudokuBoard"> sudoku board class instance</param>
    public void SetBoard(SudokuBoard<T> sudokuBoard)
    {
        this._sudokuBoard = sudokuBoard;

        _isFirstRound = true;
    }

    /// <summary>
    /// main function to call all the solving techniques .
    /// and validate the board
    /// </summary>
    public bool Solve()
    {
        if (_sudokuBoard.IsBoardSolved())
            return true;
        if (_isFirstRound)
        {
            _isFirstRound = false;
            return SolveForFirstRound();
        }
        else
        {
            return SolveForBoards();
        }
    }
    /// <summary>
    /// the function solves the boards for the first round 
    /// it means for the first time that the board is inputed , 
    /// all the heuristics will be executed on the board.
    /// </summary>
    /// <returns></returns>
    private bool SolveForFirstRound()
    {
        bool didChange = false;
        bool changed = false;
        
        if (_sudokuBoard.Size == 9)
            changed = NakedSets<T>.ApplyNakedSets(_minNumForSets, _maxNumForSets,_sudokuBoard);
        Validation.ValidateBoard<T>.Validate(
            _sudokuBoard.BoardGrid, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
        if (this._sudokuBoard.IsBoardSolved())
            return true;
        do
        {
            changed = _sudokuBoard.UpdateBoard()
                 || LockedCandidates<T>.LockedCandidatesBlockWithinRowOrCol(_sudokuBoard)
                 || LockedCandidates<T>.LockedCandidatesRowOrColWithinBox(_sudokuBoard)
                 || HiddenSingle<T>.HiddenSingles(_sudokuBoard);
            didChange = didChange | changed;
            Validation.ValidateBoard<T>.Validate(
             _sudokuBoard.BoardGrid, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
        } while (changed && !_sudokuBoard.IsBoardSolved());
        return didChange;
    }

    /// <summary>
    /// this function solves the board for each time 
    /// and executes the hidden single heuristic with the update board.
    /// </summary>
    /// <returns></returns>
    private bool SolveForBoards()
    {
        bool didChange = false;
        bool changed = false;
        if (this._sudokuBoard.IsBoardSolved())
            return true;
        do
        {
            changed = _sudokuBoard.UpdateBoard()
                || HiddenSingle<T>.HiddenSingles(_sudokuBoard);
            didChange = didChange | changed;
            Validation.ValidateBoard<T>.Validate(
             _sudokuBoard.BoardGrid, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
        } while (changed && !_sudokuBoard.IsBoardSolved());
        return didChange;
    }



}