using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve;

public class ComputerTechniques<T> : ISolving<T>
{
    private SudokuBoard<T> sudokuBoard;

    public void SetBoard(SudokuBoard<T> sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard; 
    }

    public void Solve()
    {
        throw new NotImplementedException();
    }
}

