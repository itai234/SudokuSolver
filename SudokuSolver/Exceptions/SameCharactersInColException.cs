using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

/// <summary>
/// represents a duplicate chars in the same col in the sudoku , it is not legal.
/// </summary> 
public class SameCharactersInColException : SudokuExceptions
{
    public SameCharactersInColException(string message) : base(message)
    {

    }
}