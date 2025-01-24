using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

/// <summary>
/// represents a Unsolvable board , that cannot be solved.
/// </summary>
public class UnsolvableBoardException : SudokuExceptions
{
    public UnsolvableBoardException(string message) : base(message)
    {

    }
}
