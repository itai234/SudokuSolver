using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Exceptions;

public class SameCharactersInBoxException: SudokuExceptions
{
    public SameCharactersInBoxException(string message) : base(message)
    {

    }
}

