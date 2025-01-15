using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;


/// <summary>
/// this class will represent the custom exceptions in the sudoku game,
/// its the general exception for the game , and other specific exceptions 
/// will inherit from this class.
/// </summary>
public class SudokuExceptions : Exception
{
    public SudokuExceptions(string message) : base(message)
    {

    }
}
