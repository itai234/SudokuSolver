using SudokuSolver.DataStructures.Board;
using SudokuSolver.UserHandler.Input;
using SudokuSolver.UserHandler.Output;
using System;
using System.Reflection;

namespace SudokuSolver.UserHandler;

/// <summary>
/// this class handles the Menu in the cli.
/// </summary>
public static class SudokuMenuHandler
{
    static readonly int BoxWidth = 60;

    /// <summary>
    /// this function receives the menu from the console utilities and sents it to another function 
    /// that handles the box and to make it look good. ( just to look good ) 
    /// </summary>
    public static void ShowMenu()
    {
        var lines = new string[]
        {
                ConsoleOutputUtilities.WELCOME_MESSAGE.Trim(),
                "",
                ConsoleOutputUtilities.MENU[0],
                ConsoleOutputUtilities.MENU[1],
                ConsoleOutputUtilities.MENU[2],
                ConsoleOutputUtilities.MENU[3],
        };

        PrintColoredBox(lines, BoxWidth);
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// this function receives a user's choice and
    /// handles his choice accordingly. 
    /// either show the rules or input a board or exit.
    /// </summary>
    /// <param name="choice"> the choice of the user</param>
    /// <returns> it returns false as long as the user stays , but if he wants to leave it will return true and the program
    /// ends. </returns>
    public static bool HandleChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                ShowRules();
                return false;
            case "2":
                new ConsoleBoardInput();
                return false;
            case "3":
                new FileBoardInput();
                return false;
            case "4":
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(ConsoleOutputUtilities.EXIT_MESSAGE);
                Console.ResetColor();
                return true;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ConsoleOutputUtilities.INVALID_CHOICE_MESSAGE);
                Console.ResetColor();
                return false;
        }
    }

    public static void ShowRules()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("╔════════════════════════════════════════════════╗");
        Console.WriteLine("║                 S U D O K U   R U L E S        ║");
        Console.WriteLine("╚════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkYellow;


        string projectDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));
        string filePath = Path.Combine(projectDirectory, "SudokuRules.txt");
        var text = File.ReadAllText(filePath);
        Console.WriteLine(text);

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine();
        Console.WriteLine("\nGood Luck And Have Fun With My Sudoku Solver! :)\n\n");
        Console.ResetColor();
    }

    /// <summary>
    /// this function just prints out the box surrounding the menu
    /// </summary>
    /// <param name="lines"> the lines of the menu </param>
    /// <param name="width"> a width of the box .</param>
    public static void PrintColoredBox(string[] lines, int width)
    {
        int totalWidth = Console.WindowWidth;
        int leftPad = Math.Max((totalWidth - width) / 2, 0);

        PrintBorder('╔', '═', '╗', width, leftPad, ConsoleColor.Yellow);
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == 0)
            {
                CenterText(lines[i], width, leftPad, ConsoleColor.Green);
            }
            else if (string.IsNullOrWhiteSpace(lines[i]))
            {
                CenterText(lines[i], width, leftPad, ConsoleColor.White);
            }
            else if (lines[i].StartsWith("1.") || lines[i].StartsWith("2.") ||
                     lines[i].StartsWith("3.") || lines[i].StartsWith("4."))
            {
                CenterText(lines[i], width, leftPad, ConsoleColor.Cyan);
            }
            else
            {
                CenterText(lines[i], width, leftPad, ConsoleColor.White);
            }
        }
        PrintBorder('╚', '═', '╝', width, leftPad, ConsoleColor.Yellow);
    }


    public static void PrintBorder(char left, char fill, char right,
                            int width, int leftPad, ConsoleColor borderColor)
    {
        Console.ForegroundColor = borderColor;
        Console.Write(new string(' ', leftPad));
        Console.Write(left);
        for (int i = 0; i < width - 2; i++)
        {
            Console.Write(fill);
        }
        Console.WriteLine(right);
        Console.ResetColor();
    }

    public static void CenterText(string text, int width,
                           int leftPad, ConsoleColor textColor)
    {
        Console.ForegroundColor = textColor;

        int innerWidth = width - 2;
        if (text.Length > innerWidth) text = text.Substring(0, innerWidth);
        int space = Math.Max((innerWidth - text.Length) / 2, 0);

        Console.Write(new string(' ', leftPad));
        Console.Write("║");
        Console.Write(new string(' ', space));
        Console.Write(text);
        Console.Write(new string(' ', innerWidth - space - text.Length));
        Console.WriteLine("║");
        Console.ResetColor();
    }
}

