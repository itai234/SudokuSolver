using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

/// <summary>
/// represents a duplicate chars in the same box in the sudoku , it is not legal.
/// </summary>
public class SameCharactersInBoxException: SudokuExceptions
{
    public SameCharactersInBoxException(string message) : base(message)
    {

    }
}

