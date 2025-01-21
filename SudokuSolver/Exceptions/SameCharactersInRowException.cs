using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

public class SameCharactersInRowException : SudokuExceptions
{
    public SameCharactersInRowException(string message) : base(message)
    {

    }
}
