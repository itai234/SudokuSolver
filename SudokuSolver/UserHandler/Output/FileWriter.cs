using System;
using System.IO;

namespace SudokuSolver.UserHandler.Output;

/// <summary>
///  the class handles writing to a file the result of the board
/// </summary>
public static class FileWriter
{
    /// <summary>
    /// the creates a new text file and writes to it the result of board.
    /// it catches all the possible exceptions that can occur because of this action.
    /// </summary>
    /// <param name="path"> the path to the file entered by the user .</param>
    /// <param name="board"> a string representing either the board or an error like -> unsolvable board . </param>
    public static void WriteToFile(string path, string board)
    {
        try
        {
            string directoryPath = Path.GetDirectoryName(path);
            string filePath = Path.Combine(directoryPath, "Result.txt");
            using (var sw = new StreamWriter(filePath ,append: true))
            {
                sw.WriteLine("\n" + board + "\n");
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Access to the file is denied.");
            Console.ResetColor();   
        }
        catch (DirectoryNotFoundException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The directory was not found.");
            Console.ResetColor();
        }
        catch (IOException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("An I/O error occurred while writing the file.");
            Console.ResetColor();
        }
        catch (ArgumentException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The file path is invalid.");
            Console.ResetColor();
        }
        catch (NotSupportedException)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The provided file path is not supported.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }
}

