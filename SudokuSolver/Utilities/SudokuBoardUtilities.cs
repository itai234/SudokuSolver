using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Utilities;

/// <summary>
/// the function represents utilities for the sudoku that can be static.
/// </summary>
public static class SudokuBoardUtilities
{
    public enum GameStateForValidation
    {
        BaseBoardInput,
        BaseBoardWithPossibilitiesFixed
    };

}


