using SudokuSolver.UserHandler.Input;
using SudokuSolver.UserHandler.Output;
using System;
using System.IO;
using System.Threading;

namespace SudokuSolver.UserHandler;

/// <summary>
/// this is the main class.
/// it will redirect the user to the menu.
/// </summary>
public class Game
{
    /// <summary>
    /// starts the game and gives the player his choices.
    /// </summary>
    public void StartGame()
    {
        Console.CancelKeyPress += (sender, cancel) =>
        {
            Console.WriteLine(ConsoleOutputUtilities.EXIT_MESSAGE);
        };
        Utilities.SudokuBoardUtilities.EngineTrick();

        while (true)
        {
            try
            {
                SudokuMenuHandler.ShowMenu();
                string choice = Console.ReadLine();
                bool exit = SudokuMenuHandler.HandleChoice(choice);
                if (exit) break;
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("Try again.");
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("Try again.");
            }
            catch (IOException)
            {
                Console.WriteLine("Try again.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Input too long");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

