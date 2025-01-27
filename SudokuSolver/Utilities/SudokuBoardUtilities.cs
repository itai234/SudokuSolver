using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using SudokuSolver.Solve;
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

    /// <summary>
    /// this function is a sort of a compiler "Heater" for the start up.
    /// it basically worms up the compiler and the data structers, and improves the 
    /// running time overall of the sudoku solving algorithem for the user. 
    /// cons: when the user fire up the project there will be a delay for about 0.5
    /// seconds , but after it the solving is faster.
    /// </summary>
    public static void EngineTrick()
    {
        for(int times = 0; times< 3; times++)
        {
            string input = "6..3.2....5.....1..........7.26............543.........8.15........4.2........7..";
            input = input.Replace(" ", "");
            input = input.Replace('.', '0');
            SudokuBoard<int> board = new SudokuBoard<int>(input);
            SolverManager<int> solver = new SolverManager<int>(board);
            HumanTechniques<int> humanTec = new HumanTechniques<int>();
            ComputerTechniques<int> ComputerTec = new ComputerTechniques<int>();
            solver.AddTechnique(humanTec);
            solver.AddTechnique(ComputerTec);
            solver.SolveBoard();
        }
    }

}


