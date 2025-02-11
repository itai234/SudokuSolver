using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using SudokuSolver.Solve;
using SudokuSolver.Solve.ComputerAlgorithm;
//using SudokuSolver.Solve.HumanHeuristics;
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
        string[] boards = { "000000008003000400090020060000079000000061200060502070008000500010000020405000003"
        ,"0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"
        ,"023000009400000100090030040200910004000007800900040002300090001060000000000500000"};
        for(int times = 0; times< 3; times++)
        {
            try
            {
                string input = boards[times];
                SudokuBoard<int> board = new SudokuBoard<int>(input);
                SolverManager<int> solver = new SolverManager<int>(board);
                HumanTechniques<int> humanTec = new HumanTechniques<int>();
                ComputerTechniques<int> ComputerTec = new ComputerTechniques<int>();
                solver.AddTechnique(humanTec);
                solver.AddTechnique(ComputerTec);
                solver.SolveBoard();
            }
            catch (Exception e)
            {
            }
           
        }
    }

}


