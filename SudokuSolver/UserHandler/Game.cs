using SudokuSolver.DataStructures.Board;
using SudokuSolver.Solve;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.UserHandler;

/// <summary>
/// this class will be the main class of the sudoku.
/// </summary>
public class Game
{
    public void StartGame()
    {
        while (true)
        {
            Console.WriteLine("Welcome to the my sudoku Solver!\n please enter you board: ");
            try
            {
                string input = Console.ReadLine();
                input = input.Replace(" ", "");

                Validation.ValidateInput<int> validator = new Validation.ValidateInput<int>(input);
                validator.Validate();
                SudokuBoard<int> board = new SudokuBoard<int>(input);
                board.DisplayBoard();
                SolverManager<int> solver = new SolverManager<int>(board);
                HumanTechniques<int> humanTec = new HumanTechniques<int>();
                solver.AddTechnique(humanTec);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                solver.SolveBoard();
                watch.Stop();
                var time = watch.ElapsedMilliseconds;
                Console.WriteLine("\n\n\n\n");
                Console.WriteLine($"Took {time} miliseconds ");
                board.DisplayBoard();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }
                
    }

}

