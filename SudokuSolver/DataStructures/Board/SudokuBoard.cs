﻿using SudokuSolver.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.DataStructures.Board;


/// <summary>
/// this class is the main Sudoku Board class.
/// it will handle all the actions related to the sudoku board.
/// Its generic for flexibility and if future changes are needed, its will be easy to change.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SudokuBoard<T> : Board<T>
{
    private HashSet<T>[] rows;
    private HashSet<T>[] cols;
    private HashSet<T>[] boxes;
    private readonly int boxSize;


    public SudokuBoard(string input) : base(input)
    {
        ValidateBoard<T>.Validate(board); 
        rows = new HashSet<T>[Size];
        cols = new HashSet<T>[Size];
        boxes = new HashSet<T>[Size];
        boxSize = (int)Math.Sqrt(Size);

        for (int i = 0; i < Size; i++)
        {
            rows[i] = CreatePossibilitySet();
            cols[i] = CreatePossibilitySet();
            boxes[i] = CreatePossibilitySet();
        }
        UpdateBoard();

    }
    private HashSet<T> CreatePossibilitySet()
    {
        IEnumerable<T> range = GetRangeForInt(Convert.ToInt32(MIN_VALUE), Size);
        return new HashSet<T>(range); 
    }
    private void UpdateBoard()
    {
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                if (board[i, j].IsPermanent())
                    RemoveValueFromPossibilities(i, j, board[i, j].GetValue());
    }
    private void RemoveValueFromPossibilities(int row, int col, T value)
    {
        rows[row].Remove(value);
        cols[col].Remove(value);
        int boxIndex = GetBoxIndex(row, col);
        boxes[boxIndex].Remove(value);
        UpdateBoardPossibilities(row, col, value);
    }
    private int GetBoxIndex(int row, int col)
    {
        return (row / boxSize) * boxSize + (col / boxSize);
    }
    public void UpdateBoardPossibilities(int row, int col, T value)
    {
        UpdateRowAndColPossibilities(row, col, value);
        UpdateBoxPossibilities(row, col, value);
    }
    public void UpdateRowAndColPossibilities(int row, int col, T value)
    {
        for (int j = 0; j < Size; j++)
        {
            if (!board[row, j].IsPermanent())
                board[row, j].RemovePossibility(value);
            if (!board[j, col].IsPermanent())
                board[j, col].RemovePossibility(value);
        }
    }
    public void UpdateBoxPossibilities(int row, int col, T value)
    {
        int startRow = (row / boxSize) * boxSize;
        int startCol = (col / boxSize) * boxSize;

        for (int i = startRow; i < startRow + boxSize; i++)
            for (int j = startCol; j < startCol + boxSize; j++)
                if (!board[i, j].IsPermanent())
                    board[i, j].RemovePossibility(value);
    }

    public HashSet<T> GetRowPossibilities(int row)
    {
        return rows[row];
    }

    public HashSet<T> GetColPossibilities(int col)
    {
        return cols[col];
    }

    public HashSet<T> GetBoxPossibilities(int boxIndex)
    {
        return boxes[boxIndex];
    }

}

