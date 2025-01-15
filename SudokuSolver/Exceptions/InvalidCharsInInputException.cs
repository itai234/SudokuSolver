using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

public class InvalidCharsInInputException : SudokuExceptions
{
    public InvalidCharsInInputException(string message) : base(message)
    {
    }
}

