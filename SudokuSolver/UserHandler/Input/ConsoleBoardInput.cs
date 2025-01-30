using Microsoft.VisualStudio.TestPlatform.Utilities;
using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using SudokuSolver.Solve;
using SudokuSolver.UserHandler.Output;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.UserHandler.Input;


public class ConsoleBoardInput : IReader
{
    private SudokuBoard<int> board;
    private SolverManager<int> solver;
    private Stopwatch stopwatch = new Stopwatch();

    public ConsoleBoardInput() { ReadInput(); } 
    public void ReadInput()
    {
        Console.WriteLine(ConsoleOutputUtilities.ENTER_BOARD_MESSAGE);
        string input = Console.ReadLine();
        input = input.Replace(" ", "");
        input = input.Replace('.', '0');
        ValidateInput(input);
    }

    public void ValidateInput(string input)
    {
        try
        {
            Validation.ValidateInput<int> validator = new Validation.ValidateInput<int>(input);
            validator.Validate();
            this.board = new SudokuBoard<int>(input);
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
        AddTechniques();    
 
    }
    public void AddTechniques()
    {
        this.solver = new SolverManager<int>(board);
        HumanTechniques<int> humanTec = new HumanTechniques<int>();
        ComputerTechniques<int> ComputerTec = new ComputerTechniques<int>();
        solver.AddTechnique(humanTec);
        solver.AddTechnique(ComputerTec);
        solve();
    }

    public void solve()
    {
        try
        {
            stopwatch.Reset();
            stopwatch.Start();
            bool Solved = solver.SolveBoard();
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("\n Solved The Board!\n");
            Console.WriteLine($"Took {time} miliseconds ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            board.DisplayBoard();
            Console.ResetColor();
            Console.WriteLine("\n");
        }
        catch (Exception ex) 
        when (ex is UnsolvableBoardException) 
        {
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.WriteLine($"Took {time} miliseconds. ");
            Console.ResetColor();
        }
        SudokuMenuHandler.MenuUserChoice();
    }
}

