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
                Validation.ValidateInput<int> validator = new Validation.ValidateInput<int>(input);
                validator.Validate();
                Console.WriteLine("Success");

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

