using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;


/// <summary>
/// represenmts invalid board size exception -- > when user enters a illegal board size.
/// </summary>
public class InvalidBoardSizeException : SudokuExceptions
{
    public InvalidBoardSizeException(string message) : base(message)
    {
    }
}

