using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve;

public class SolverManager<T>
{

    private SudokuBoard<T> sudokuBoard;
    private List<ISolving<T>> solvingTechniques;

    public SolverManager(SudokuBoard<T> sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
        solvingTechniques = new List<ISolving<T>>();
    }
    public void AddTechnique(ISolving<T> technique)
    {
        technique.SetBoard(sudokuBoard);
        solvingTechniques.Add(technique);
    }
    public void SolveBoard()
    {
        foreach (var technique in solvingTechniques)
        {
            technique.Solve();
            if (this.sudokuBoard.IsBoardSolved())
            {
                Console.WriteLine("Solved!");
                break;
            }
        }
    }
}

