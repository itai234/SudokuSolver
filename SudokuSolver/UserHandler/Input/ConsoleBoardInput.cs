using SudokuSolver.UserHandler.Output;
using System;

namespace SudokuSolver.UserHandler.Input;

/// <summary>
/// this class handles the console board input.
/// </summary>
public class ConsoleBoardInput : InputReader
{
    public ConsoleBoardInput()
    {
        ReadInput();
    }

    /// <summary>
    /// reads input from the user and after it it will summon the methods from the one he inherits from.
    /// which is the general Input reader that handles validation of the input and solving the board.
    /// </summary>
    public override void ReadInput()
    { 
        try
        {
            Console.WriteLine(ConsoleOutputUtilities.ENTER_BOARD_MESSAGE);
            string input = Console.ReadLine();
            input = input.Replace(" ", "");
            Console.WriteLine(ConsoleOutputUtilities.WANT_TO_SOLVE_MESSAGE);
            string answer = Console.ReadLine();
            answer = answer.ToLower();
            switch (answer)
            {
                case "yes":
                    try
                    {
                        ValidateInput(input);
                        AddTechniques();
                        Solve();
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input");
                        Console.ResetColor();
                    }
                    break;
                default:
                    break;

            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input");
            Console.ResetColor();   
        }
    }
}

