using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

public class InvalidCharactersRangeForBoardException : SudokuExceptions
{
    public InvalidCharactersRangeForBoardException(string message) : base(message)
    {
    }
}

