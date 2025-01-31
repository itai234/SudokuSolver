using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

/// <summary>
///  this exception is thrown when the user enters an invalid path location
///  as input for the board.
/// </summary>
public class InvalidFilePathException : SudokuExceptions
{
    public InvalidFilePathException(string message) : base(message)
    {

    }
}

