using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

/// <summary>
/// represents invalid characters that cannot represent a board in the input
/// </summary>
public class InvalidCharsInInputException : SudokuExceptions
{
    public InvalidCharsInInputException(string message) : base(message)
    {
    }
}

