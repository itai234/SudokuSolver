using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve;

public interface ISolving<T>
{
    public void Solve();
    public void SetBoard(SudokuBoard<T> sudokuBoard);

}

