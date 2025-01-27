using SudokuSolver.DataStructures.Board;
using SudokuSolver.Solve;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
        Stopwatch stopwatch = new Stopwatch();  
        Utilities.SudokuBoardUtilities.EngineTrick();
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
            catch(EndOfStreamException ex)
            {
                Console.WriteLine("Try again.");
            }
            catch(ThreadInterruptedException ex)
            {
                Console.WriteLine("Try again.");
            }
            catch(IOException ex)
            {
                Console.WriteLine("Try again.");
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine("Out of memory");

            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Input too long");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var time = stopwatch.ElapsedMilliseconds;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Took {time} miliseconds. ");
                Console.ResetColor();
            }

        }
                
    }

}

