

namespace SudokuSolver.UserHandler.Output;

/// <summary>
/// class of utilities for the connsole .
/// </summary>
public static class ConsoleOutputUtilities
{
    public static readonly string WELCOME_MESSAGE =
        "\t\t\t\t <-=== Welcome To Itai's Sudoku Solver ===->\n";

    public static readonly string[] MENU = {
            "1. Sudoku Rules.",
            "2. Solve Sudoku With Input From Console.",
            "3. Solve Sudoku With Input From File.",
            "4. Exit."
        };

    public static readonly string EXIT_MESSAGE =
        "Thank You For Using my Sudoku Solver. Goodbye, And See You Next Time.";

    public static readonly string INVALID_CHOICE_MESSAGE = "Invalid choice, please try again.";
    public static readonly string ERROR_READING_FILE_MESSAGE = "Error reading the file.";
    public static readonly string ERROR_WRITING_TO_FILE_MESSAGE = "Error writing to the file.";
    public static readonly string SOLVED_SUDOKU_MESSAGE = "The Board Is Solved!";
    public static readonly string ENTER_FILE_PATH_MESSAGE = "Enter the file path to load the Sudoku board:";
    public static readonly string ENTER_BOARD_MESSAGE = "Please Enter Your Board:";
    public static readonly string WANT_TO_SOLVE_MESSAGE = "Do You Wish To Solve The Board? If Yes, type \"Yes\"";
    public static readonly string BEFORE_BOARD_DISPLAY_MESSAGE = "Your Sudoku board:";

    public static readonly string TITLE_MESSAGE = "\r\n  _____ _        _ _        _____           _       _             _____       _                \r\n |_   _| |      (_| )      / ____|         | |     | |           / ____|     | |               \r\n   | | | |_ __ _ _|/ ___  | (___  _   _  __| | ___ | | ___   _  | (___   ___ | |_   _____ _ __ \r\n   | | | __/ _` | | / __|  \\___ \\| | | |/ _` |/ _ \\| |/ / | | |  \\___ \\ / _ \\| \\ \\ / / _ \\ '__|\r\n  _| |_| || (_| | | \\__ \\  ____) | |_| | (_| | (_) |   <| |_| |  ____) | (_) | |\\ V /  __/ |   \r\n |_____|\\__\\__,_|_| |___/ |_____/ \\__,_|\\__,_|\\___/|_|\\_\\\\__,_| |_____/ \\___/|_| \\_/ \\___|_|   \r\n                                                                                               \r\n                                                                                               \r\n";


}

