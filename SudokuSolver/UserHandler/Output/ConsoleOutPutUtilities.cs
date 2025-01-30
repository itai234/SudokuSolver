using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.UserHandler.Output;

public static class ConsoleOutputUtilities
{
    public static readonly string WELCOME_MESSAGE = "\t\t\t\t <-=== Welcome To Itai's Sudoku Solver ===->        \n";

    public static readonly string[] MENU = {
        "1. Sudoku Rules.",
        "2. Solve Sudoku With Input From Console.",
        "3. Solve Sudoku With Input From File.",
        "4. Exit."
    };

    public static readonly string EXIT_MESSAGE = "Thank You For Using my Sudoku Solver. Goodbye, And See You Next Time.";

    public static readonly string INVALID_CHOICE_MESSAGE = "Invalid choice, please try again.";

    public static readonly string ERROR_READING_FILE_MESSAGE = "Error reading the file. Please check the file format or path.";

    public static readonly string ERROR_WRITING_TO_FILE_MESSAGE = "Error writing to the file.";

    public static readonly string SOLVED_SUDOKU_MESSAGE = "The Board Is Solved!";

    public static readonly string ENTER_FILE_PATH_MESSAGE = "Enter the file path to load the Sudoku board:";

    public static readonly string ENTER_BOARD_MESSAGE = "Please Enter Your Board:";

    public static readonly string BEFORE_BOARD_DISPLAY_MESSAGE = "Your Sudoku board:";

    public static readonly string WANT_TO_SOLVE_MESSAGE = "Do You Wish To Solve The Board? If Yes, type \"Yes\"";
    //ill do here the rules later..
}

