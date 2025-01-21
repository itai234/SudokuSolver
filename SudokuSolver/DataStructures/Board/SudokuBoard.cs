using SudokuSolver.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Utilities;
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
        ValidateBoard<T>.Validate(board, SudokuBoardUtilities.GameStateForValidation.BaseBoardInput);
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
        ValidateBoard<T>.Validate(board, SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
    }
    private void RemoveValueFromPossibilities(int row, int col, T value)
    {
        rows[row].Remove(value);
        cols[col].Remove(value);
        int boxIndex = GetBoxIndex(row, col);
        boxes[boxIndex].Remove(value);
        UpdateRowPossibilities(row, col, value);
        UpdateColPossibilities(row, col, value);
        UpdateBoxPossibilities(row, col, value);
    }
    private int GetBoxIndex(int row, int col)
    {
        return Math.Min((row / boxSize) * boxSize + (col / boxSize), Size - 1);
    }
    public void UpdateRowPossibilities(int row, int col, T value)
    {
        for (int j = 0; j < Size; j++)
        {
            if (!board[row, j].IsPermanent())
                board[row, j].RemovePossibility(value);
        }
    }
    public void UpdateColPossibilities(int row, int col, T value)
    {
        for (int j = 0; j < Size; j++)
        {
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
    public bool CanPlaceValue(int row, int col, T value)
    {
        return !rows[row].Contains(value) && !cols[col].Contains(value) &&
               !boxes[GetBoxIndex(row, col)].Contains(value) && !board[row, col].IsPermanent();
    }
    public void SetCellValue(int row, int col, T value)
    {
        board[row, col].SetValue(value);
        RemoveValueFromPossibilities(row, col, value);
    }
    public (int row, int col)? FindCellWithLeastPossibilities()
    {
        int min = Convert.ToInt32(MAX_VALUE);
        int? row = null;
        int? col = null;
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                if (board[i, j].GetPossibilities().Count() < min)
                {
                    min = board[i, j].GetPossibilities().Count();
                    row = i; 
                    col = j; 
                }
        return (row.HasValue && col.HasValue) ? (row.Value, col.Value) : null;
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

