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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome to the my sudoku Solver!\nPlease enter your board you want to solve as string: ");
            Console.ResetColor();
            try
            {
                string input = Console.ReadLine();
                input = input.Replace(" ", "");
                input = input.Replace('.', '0');
                Validation.ValidateInput<int> validator = new Validation.ValidateInput<int>(input);
                validator.Validate();
                SudokuBoard<int> board = new SudokuBoard<int>(input);
                board.DisplayBoard();
                SolverManager<int> solver = new SolverManager<int>(board);
                HumanTechniques<int> humanTec = new HumanTechniques<int>();
                ComputerTechniques<int> ComputerTec = new ComputerTechniques<int>();
                solver.AddTechnique(humanTec);
                solver.AddTechnique(ComputerTec);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                solver.SolveBoard();
                watch.Stop();
                var time = watch.ElapsedMilliseconds;
                Console.WriteLine($"Took {time} miliseconds ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                board.DisplayBoard();
                Console.ResetColor();
                Console.WriteLine("\n");
            }
            catch(EndOfStreamException ex)
            {
                Console.WriteLine("Try again.");
            }
            catch(ThreadInterruptedException ex)
            {
                Console.WriteLine("Try again.");
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

