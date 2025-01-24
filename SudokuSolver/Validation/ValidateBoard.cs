using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using static SudokuSolver.Utilities.SudokuBoardUtilities;

namespace SudokuSolver.Validation;

/// <summary>
///  the function validates the board and if it is not valid it will throw an exception.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ValidateBoard<T>
{
    /// <summary>
    ///  main constructor that receives the board and the game state ( for deciding which exceptions to throw)
    /// </summary>
    /// <param name="board"> the board of the game </param>
    /// <param name="state"> the game state (before starting or after starting the solving)</param>
    public static void Validate(Cell<T>[,] board, GameStateForValidation state )
    {
        ValidateBoardValues(board,state);
    }

    /// <summary>
    /// main function to validate the board , it loops through the board and for each cell checks if it is valid.
    /// </summary>
    /// <param name="board"> board of the sudoku </param>
    /// <param name="state"> game state </param>
    private static void ValidateBoardValues(Cell<T>[,] board, GameStateForValidation state)
    {
        HashSet<T>[] rows = new HashSet<T>[board.GetLength(0)];
        HashSet<T>[] cols = new HashSet<T>[board.GetLength(0)];
        HashSet<T>[] boxes = new HashSet<T>[board.GetLength(0)];

        for (int i = 0; i < board.GetLength(0); i++)
        {
            rows[i] = new HashSet<T>();
            cols[i] = new HashSet<T>();
            boxes[i] = new HashSet<T>();
        }

        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {


                if (board[row, col].IsPermanent())
                {
                    T value = board[row, col].GetValue();
                    CheckForDuplicatesInRow(rows, row, value,state);
                    CheckForDuplicatesInColumn(cols, col, value, state);
                    CheckForDuplicatesInBox(boxes, row, col, value, state);
                }
            }
        }
    }

    /// <summary>
    /// checks for duplicate in a cetain row.
    /// </summary>
    /// <param name="rows">hashset that reprsents the a certain row values</param>
    /// <param name="row"> the row to check</param>
    /// <param name="value"> the value to check if it is duplicated.</param>
    /// <param name="state"> the game state to decide exception </param>
    private static void CheckForDuplicatesInRow(HashSet<T>[] rows, int row, T value, GameStateForValidation state)
    {
        if (rows[row].Contains(value))
            DecideExceptionForDuplicatedInRow(value, state);
        rows[row].Add(value);
    }

    /// <summary>
    /// checks for duplicate in a cetain column.
    /// </summary>
    /// <param name="cols">hashset that reprsents the a certain col values</param>
    /// <param name="col"> the column to check</param>
    /// <param name="value"> the value to check if it is duplicated.</param>
    /// <param name="state"> the game state to decide exception </param>
    private static void CheckForDuplicatesInColumn(HashSet<T>[] cols, int col, T value, GameStateForValidation state)
    {
        if (cols[col].Contains(value))
            DecideExceptionForDuplicatedInCol(value, state);
        cols[col].Add(value);
    }
    /// <summary>
    /// checks for duplicate in a cetain box.
    /// </summary>
    /// <param name="boxes">hashset that reprsents the a certain box values</param>
    /// <param name="row"> the row to check</param>
    /// <param name="col"> the column to check</param>
    /// <param name="value"> the value to check if it is duplicated.</param>
    /// <param name="state"> the game state to decide exception </param>
    private static void CheckForDuplicatesInBox(HashSet<T>[] boxes, int row, int col, T value, GameStateForValidation state)
    {
        int boxSize = (int)Math.Sqrt(boxes.Length);
        int boxIndex = Math.Min((row / boxSize) * boxSize + (col / boxSize), boxes.Length - 1);
        if (boxes[boxIndex].Contains(value))
            DecideExceptionForDuplicatedInBox(value,state);
        boxes[boxIndex].Add(value);
    }
    
    /// <summary>
    /// decides which exception to throw if there are duplicates.
    /// </summary>
    /// <param name="value"> the value that is duplicated.</param>
    /// <param name="state"> the game state </param>
    /// <exception cref="SameCharactersInBoxException"> if the same characters are in the same box at the begining</exception>
    /// <exception cref="UnsolvableBoardException"> if the board is unsolvable </exception>
    private static void DecideExceptionForDuplicatedInBox(T value,GameStateForValidation state)
    {
        switch (state)
        {
            case GameStateForValidation.BaseBoardInput:
                throw new SameCharactersInBoxException($"Illegal Board. Cannot put same characters -- {value} -- in the same box.");
            case GameStateForValidation.BaseBoardWithPossibilitiesFixed:
                throw new UnsolvableBoardException($"The Board You Entered Is Invalid and Unsolvable.");
        }
    }
    /// <summary>
    /// decides which exception to throw if there are duplicates.
    /// </summary>
    /// <param name="value"> the value that is duplicated.</param>
    /// <param name="state"> the game state </param>
    /// <exception cref="SameCharactersInRowException"> if the same characters are in the same Row at the begining</exception>
    /// <exception cref="UnsolvableBoardException"> if the board is unsolvable </exception>
    private static void DecideExceptionForDuplicatedInRow(T value,GameStateForValidation state)
    {
        switch (state)
        {
            case GameStateForValidation.BaseBoardInput:
                throw new SameCharactersInRowException($"Illegal Board. Cannot put same characters -- {value} -- in the same row.");
            case GameStateForValidation.BaseBoardWithPossibilitiesFixed:
                throw new UnsolvableBoardException($"The Board You Entered Is Invalid and Unsolvable.");
        }
    }
    /// <summary>
    /// decides which exception to throw if there are duplicates.
    /// </summary>
    /// <param name="value"> the value that is duplicated.</param>
    /// <param name="state"> the game state </param>
    /// <exception cref="SameCharactersInColException"> if the same characters are in the same Col at the begining</exception>
    /// <exception cref="UnsolvableBoardException"> if the board is unsolvable </exception>
    private static void DecideExceptionForDuplicatedInCol(T value ,GameStateForValidation state)
    {
        switch (state)
        {
            case GameStateForValidation.BaseBoardInput:
                throw new SameCharactersInColException($"Illegal Board. Cannot put same characters -- {value} -- in the same column.");
            case GameStateForValidation.BaseBoardWithPossibilitiesFixed:
                throw new UnsolvableBoardException($"The Board You Entered Is Invalid and Unsolvable.");
        }

    }
}

