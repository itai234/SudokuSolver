using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve;

/// <summary>
/// this class manages all the solving techniques and calls them.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SolverManager<T>
{

    private SudokuBoard<T> sudokuBoard;
    private List<ISolving<T>> solvingTechniques;
    /// <summary>
    /// the constructor gets the sudoku board class instance and inserts it to  the class property,
    /// it also creates a list of the solving techniques using the interface.
    /// </summary>
    /// <param name="sudokuBoard"></param>
    public SolverManager(SudokuBoard<T> sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
        solvingTechniques = new List<ISolving<T>>();
    }
    /// <summary>
    /// adds a technique to the list of the techniques in this class.
    /// and initialize for this technique it's board.
    /// </summary>
    /// <param name="technique"></param>
    public void AddTechnique(ISolving<T> technique)
    {
        technique.SetBoard(sudokuBoard);
        solvingTechniques.Add(technique);
    }
    /// <summary>
    /// main function to call for all the solving techniques and solve the board
    /// if the board is solved it will break and return.
    /// </summary>
    public bool SolveBoard()
    {
        foreach (var technique in solvingTechniques)
        {
            technique.Solve();
            if (this.sudokuBoard.IsBoardSolved())
            {
                return true;
               
            }
        }
        return false;
    }
}

