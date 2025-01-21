using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using System;
using System.Collections.Generic;

namespace SudokuSolver.Validation;

public static class ValidateBoard<T>
{
    public static void Validate(Cell<T>[,] board)
    {
        ValidateBoardValues(board);
    }

    private static void ValidateBoardValues(Cell<T>[,] board)
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
                    CheckForDuplicatesInRow(rows, row, value);
                    CheckForDuplicatesInColumn(cols, col, value);
                    CheckForDuplicatesInBox(boxes, row, col, value);
                }
            }
        }
    }

    private static void CheckForDuplicatesInRow(HashSet<T>[] rows, int row, T value)
    {
        if (rows[row].Contains(value))
            throw new SameCharactersInRowException($"Illegal Board. Cannot put same characters -- {value} -- in the same row.");
        rows[row].Add(value);
    }

    private static void CheckForDuplicatesInColumn(HashSet<T>[] cols, int col, T value)
    {
        if (cols[col].Contains(value))
            throw new SameCharactersInColException($"Illegal Board. Cannot put same characters -- {value} -- in the same column.");
        cols[col].Add(value);
    }

    private static void CheckForDuplicatesInBox(HashSet<T>[] boxes, int row, int col, T value)
    {
        int boxSize = (int)Math.Sqrt(boxes.Length);
        int boxIndex = Math.Min((row / boxSize) * boxSize + (col / boxSize), boxes.Length - 1);
        if (boxes[boxIndex].Contains(value))
            throw new SameCharactersInBoxException($"Illegal Board. Cannot put same characters -- {value} -- in the same box.");
        boxes[boxIndex].Add(value);
    }
}

