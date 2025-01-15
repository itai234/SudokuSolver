using System;
using System.Linq;

namespace SudokuSolver.ValidateInput;

/// <summary>
/// This class validates the user's input for constructing a Sudoku board.
/// </summary>
public class ValidateInput
{
    private readonly string _userInput;
    private readonly int _boardSize;

    /// <summary>
    /// Main constructor to initialize the input and board size.
    /// </summary>
    /// <param name="userInput">The input string representing the Sudoku board.</param>
    public ValidateInput(string userInput)
    {
        _userInput = userInput ?? throw new ArgumentNullException("Input cannot be null.");
        double root = Math.Sqrt(_userInput.Length);
        if (root != (int)root)
            throw new Exceptions.InvalidBoardSizeException("Input length is invalid. Cannot Represent a square Sudoku board.");
        _boardSize = (int)root;
    }

    /// <summary>
    /// Validates that the input contains only numbers.
    /// </summary>
    private void ValidateContainsOnlyDigits()
    {
        if (!_userInput.All(char.IsDigit))
            throw new Exceptions.InvalidCharsInInputException("Input contains non numbers.");
    }

    /// <summary>
    /// Validates that all numbers in the input are within the allowed range of the sudoku Board.
    /// </summary>
    private void ValidateNumbersInRange()
    {
        if (_userInput.Any(c => int.Parse(c.ToString()) < 0 || int.Parse(c.ToString()) > _boardSize))
            throw new Exceptions.InvalidNumbersInBoardException("Input contains numbers outside the allowed range.");
    }

    /// <summary>
    /// Runs all validation checks on the input.
    /// </summary>
    public void Validate()
    {
        ValidateContainsOnlyDigits();
        ValidateNumbersInRange();
    }
}
