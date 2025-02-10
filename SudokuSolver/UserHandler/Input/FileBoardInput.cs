using Microsoft.Testing.Platform.Extensions.Messages;
using SudokuSolver.Exceptions;
using SudokuSolver.UserHandler.Output;
using System;

namespace SudokuSolver.UserHandler.Input;


/// <summary>
///  this class handles the file board input type of way.
/// </summary>
public class FileBoardInput : InputReader
{
    string path;

    public FileBoardInput()
    {
        ReadInput();
    }

    /// <summary>
    /// the function will get the path to the file from the user.
    /// the file will contain the sudoku board as a long string .
    /// if the file and the string inside it is valid, 
    /// it will solve the board and write the result also to a file.
    /// </summary>
    public override void ReadInput()
    {
        Console.WriteLine(ConsoleOutputUtilities.ENTER_FILE_PATH_MESSAGE);
        path = Console.ReadLine();
        try
        {
            _usersInput = ReadFile();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();
            return;
        }

        
        try
        {
            for (int index = 0; index < _usersInput.Length; index++)
            {
                ValidateInput(_usersInput[index]);
                AddTechniques();
                Solve();
                if (_board.IsBoardSolved())
                    FileWriter.WriteToFile(path, _board.BoardToString());
                else
                    FileWriter.WriteToFile(path, "The board is Unsolvable");
            }
            
        }
        catch
        {
        }
    }

    /// <summary>
    /// the function is responsibly to read the file and look for exceptions
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidFilePathException"> custom exception if the file is invalid. </exception>
    string[] ReadFile()
    {
        if (!path.EndsWith(".txt"))
            throw new InvalidFilePathException("The file path is invalid.");

        try
        {
            return File.ReadAllLines(path);
                 
        }
        catch (FileNotFoundException)
        {
            throw new InvalidFilePathException("File not found.");
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidFilePathException("Unauthorized access.");
        }
        catch (DirectoryNotFoundException)
        {
            throw new InvalidFilePathException("Directory not found.");
        }
        catch (PathTooLongException)
        {
            throw new InvalidFilePathException("Path is too long.");
        }
        catch (IOException ex)
        {
            throw new InvalidFilePathException($"I/O error: {ex.Message}");
        }
    }
}

