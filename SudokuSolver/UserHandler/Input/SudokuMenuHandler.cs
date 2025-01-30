using SudokuSolver.UserHandler.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.UserHandler.Input;

public static class SudokuMenuHandler
{
    //public static SudokuMenuHandler() { MenuUserChoice(); }

    public static void MenuUserChoice()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(ConsoleOutputUtilities.WELCOME_MESSAGE);
        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (string str in ConsoleOutputUtilities.MENU)
            Console.WriteLine(str);
        while(true)
        {
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ShowRules();
                    break;
                case "2":
                    SolveFromConsole();
                    break;
                case "3":
                    SolveFromFile();
                    break;
                case "4":
                    ExitApplication();
                    break;
                default:
                    Console.WriteLine(ConsoleOutputUtilities.INVALID_CHOICE_MESSAGE);
                    break;

            }

        }

    }
    private static void ShowRules()
    {
     
    }

    private static void SolveFromConsole()
    {
        ConsoleBoardInput consoleBoardInput = new ConsoleBoardInput();
    }

    private static void SolveFromFile()
    {
      
    }

    private static void ExitApplication()
    {
        Console.WriteLine(ConsoleOutputUtilities.EXIT_MESSAGE);
        Environment.Exit(0);  
    }
}

