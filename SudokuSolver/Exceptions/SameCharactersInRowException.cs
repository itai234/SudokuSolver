using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

/// <summary>
/// represents a duplicate chars in the same row in the sudoku , it is not legal.
/// </summary>
public class SameCharactersInRowException : SudokuExceptions
{
    public SameCharactersInRowException(string message) : base(message)
    {

    }
}
