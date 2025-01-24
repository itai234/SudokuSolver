using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

/// <summary>
/// represents Invalid Characters Range For Board Exception ->> when user enters a range that is not possibile
/// for example for 25 x 25 enters 30 for a cell.
/// </summary>
public class InvalidCharactersRangeForBoardException : SudokuExceptions
{
    public InvalidCharactersRangeForBoardException(string message) : base(message)
    {
    }
}

