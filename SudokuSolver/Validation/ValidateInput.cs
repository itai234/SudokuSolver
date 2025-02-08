using System;
using System.Linq;

namespace SudokuSolver.Validation;

/// <summary>
/// This class validates the user's input for constructing a Sudoku board.
/// </summary>
public class ValidateInput<T>
{
    private readonly string _userInput;
    private readonly int _boardSize;
    private readonly int MAX_BOARD_SIZE = 25 * 25;

    /// <summary>
    /// Main constructor to initialize the input and board size.
    /// </summary>
    /// <param name="userInput">The input string representing the Sudoku board.</param>
    public ValidateInput(string userInput)
    {
        if(userInput.Length > MAX_BOARD_SIZE)
            throw new Exceptions.InvalidBoardSizeException("Input length is invalid. Cannot Represent a square Sudoku board.");
        _userInput = userInput ?? throw new ArgumentNullException("Input cannot be null.");
        double root = Math.Sqrt(_userInput.Length);
        if (root != (int)root || Math.Sqrt(root) % 1 != 0 )
            throw new Exceptions.InvalidBoardSizeException("Input length is invalid. Cannot Represent a square Sudoku board.");

        _boardSize = (int)root;
    }

    /// <summary>
    /// Validates that the input contains only the type of it's kind . 
    /// </summary>
    private void ValidateCharsInBoard()
    {

        if (string.IsNullOrEmpty(_userInput) || this._userInput.Any(c => !CanConvertToType(c.ToString())))
            throw new Exceptions.InvalidCharsInInputException("Input contains invalid characters for this board/empty string.");
    }
    /// <summary>
    /// the function checks if a character is a type of the Class --> T (integer for example) 
    /// </summary>
    /// <param name="letter">the input of the function is a char from the user's input , represented by a string </param>
    /// <returns></returns>
    private bool CanConvertToType(string letter)
    {
        try
        {
            Convert.ChangeType(letter[0]-'0', typeof(T));
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates that all characters in the input are within the allowed range of the sudoku Board.
    /// (for now it will be supporting integers, but it is flexible for changes if needed.)
    /// </summary>
    private void ValidateCharactersInRange()
    {
        foreach (char c in _userInput)
        {
            T val = (T)Convert.ChangeType(c.ToString()[0]-'0', typeof(T));
            if (val is int intValue && (intValue < 0 || intValue > _boardSize))
            {
                throw new Exceptions.InvalidCharactersRangeForBoardException("Input contains characters outside the allowed range.");
            }
        }
    }

    /// <summary>
    /// Runs all validation checks on the input.
    /// </summary>
    public void Validate()
    {
        ValidateCharsInBoard();
        ValidateCharactersInRange();
    }
}