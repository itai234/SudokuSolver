using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

public class UnsolvableBoardException : SudokuExceptions
{
    public UnsolvableBoardException(string message) : base(message)
    {

    }
}
