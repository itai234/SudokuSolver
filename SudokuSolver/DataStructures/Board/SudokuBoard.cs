using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.DataStructures.Board;

public class SudokuBoard<T> : Board<T>
{
    public SudokuBoard(string input) : base(input)
    {

    }
}

