using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using SudokuSolver.Solve;
using SudokuSolver.Solve.ComputerAlgorithm;
using SudokuSolver.Solve.HumanHeuristics;
using SudokuSolver.UserHandler.Output;
using System;
using System.Diagnostics;

namespace SudokuSolver.UserHandler.Input;

/// <summary>
/// this is the main class that the file and console input classes
/// will inherit from , it is abstract , and contains all the functions 
/// and actions to solve a board.
/// </summary>
public abstract class InputReader
{
    protected SudokuBoard<int> _board;
    protected SolverManager<int> _solver;
    protected Stopwatch stopwatch = new Stopwatch();
    protected string[] _usersInput;

    /// <summary>
    /// this function represents reading the input from the user, and it can be either by the path 
    /// or direct input through the cli, so its abstract and the implementation depends on the class
    /// that inherits from this class.
    /// </summary>
    public abstract void ReadInput();

    /// <summary>
    /// the function receives an input representing the board ,
    /// and the function will validate the input if it is valid for a sudoku board.
    /// </summary>
    /// <param name="input"> the input is the string represeting the board.</param>
    public void ValidateInput(string input)
    {
        try
        {
            var validator = new Validation.ValidateInput<int>(input);
            validator.Validate();
            _board = new SudokuBoard<int>(input);
        }
        catch (Exception ex)
        when (ex is InvalidBoardSizeException
            || ex is InvalidCharactersRangeForBoardException
            || ex is InvalidCharsInInputException
            || ex is SameCharactersInBoxException
            || ex is SameCharactersInColException
            || ex is SameCharactersInRowException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }
    }
    /// <summary>
    /// the function adds all the solving techniques to the solver manager.
    /// </summary>
    public void AddTechniques()
    {
        _solver = new SolverManager<int>(_board);
        var humanTec = new HumanTechniques<int>();
        var computerTec = new ComputerTechniques<int>();
        _solver.AddTechnique(humanTec);
        _solver.AddTechnique(computerTec);
    }

    /// <summary>
    /// main function to solve the board.
    /// </summary>
    public void Solve()
    {
        try
        {
            stopwatch.Reset();
            stopwatch.Start();
            bool solved = _solver.SolveBoard();
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine();
            Console.WriteLine("Solved The Board!");
            Console.WriteLine($"Took {time} miliseconds");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(_board.DisplayBoard());
            //Console.WriteLine("\n" + _board.BoardToString());
            Console.ResetColor();
            Console.WriteLine();
        }
        catch (UnsolvableBoardException ex)
        {
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine($"Took {time} miliseconds.");
            Console.ResetColor();
        }
    }
}

