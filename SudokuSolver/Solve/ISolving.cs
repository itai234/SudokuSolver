using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve;

/// <summary>
///  the interface that will represent the general solving techniques that 
///  the human and computer solving techniques will inherit from
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISolving<T>
{
    public bool Solve();
    public void SetBoard(SudokuBoard<T> sudokuBoard);

}

