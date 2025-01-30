using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.UserHandler.Input;

public interface IReader
{
    public void ReadInput();  
    public void ValidateInput(string input);
}

