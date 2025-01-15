using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;


public class InvalidBoardSizeException : SudokuExceptions
{
    public InvalidBoardSizeException(string message) : base(message)
    {
    }
}

