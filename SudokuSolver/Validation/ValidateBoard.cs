﻿using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;
using static SudokuSolver.Utilities.SudokuBoardUtilities;

namespace SudokuSolver.Validation;

public static class ValidateBoard<T>
{
    public static void Validate(Cell<T>[,] board, GameStateForValidation state )
    {
        ValidateBoardValues(board,state);
    }

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

    private static void CheckForDuplicatesInRow(HashSet<T>[] rows, int row, T value, GameStateForValidation state)
    {
        if (rows[row].Contains(value))
            DecideExceptionForDuplicatedInRow(value, state);
        rows[row].Add(value);
    }

    private static void CheckForDuplicatesInColumn(HashSet<T>[] cols, int col, T value, GameStateForValidation state)
    {
        if (cols[col].Contains(value))
            DecideExceptionForDuplicatedInCol(value, state);
        cols[col].Add(value);
    }

    private static void CheckForDuplicatesInBox(HashSet<T>[] boxes, int row, int col, T value, GameStateForValidation state)
    {
        int boxSize = (int)Math.Sqrt(boxes.Length);
        int boxIndex = Math.Min((row / boxSize) * boxSize + (col / boxSize), boxes.Length - 1);
        if (boxes[boxIndex].Contains(value))
            DecideExceptionForDuplicatedInBox(value,state);
        boxes[boxIndex].Add(value);
    }
    
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

