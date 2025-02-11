using SudokuSolver.UserHandler.Input;
using SudokuSolver.UserHandler.Output;
using System;
using System.IO;
using System.Threading;
using System.Xml.Linq;

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
        Console.ForegroundColor = ConsoleColor.Cyan;   
        // this part just prints the title welcome message in the center of the screen
        string[] lines = ConsoleOutputUtilities.TITLE_MESSAGE.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        int consoleWidth = Console.WindowWidth;

        foreach (var line in lines)
        {
            int leftPadding = Math.Max((consoleWidth - line.Length) / 2, 0);
            Console.WriteLine(new string(' ', leftPadding) + line);
        }
        Console.ResetColor();   
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

