using SudokuSolver.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.Utilities;
using System.Reflection.Metadata.Ecma335;
namespace SudokuSolver.DataStructures.Board;


/// <summary>
/// this class is the main Sudoku Board class.
/// it will handle all the actions related to the sudoku board.
/// Its generic for flexibility and if future changes are needed, its will be easy to change.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SudokuBoard<T> : Board<T>
{
    public HashSet<T>[] rows { get; }
    public HashSet<T>[] cols { get; }
    public HashSet<T>[] boxes { get; }

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
    public HashSet<T> CreatePossibilitySet()
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
    public bool UpdateBoardAfterTechnique() 
    {
        bool didchange = false;
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                if (board[i, j].IsPermanent())
                    didchange = didchange |  RemoveValueFromPossibilities(i, j, board[i, j].GetValue());
        return didchange;
    }
    public bool RemoveValueFromPossibilities(int row, int col, T value)
    {
        bool didchange = false;
        if (rows[row].Remove(value)) didchange = true;
        if (cols[col].Remove(value)) didchange = true;

        int boxIndex = GetBoxIndex(row, col);
        if (boxes[boxIndex].Remove(value)) didchange = true;

        if (UpdateRowPossibilities(row, col, value)) didchange = true;
        if (UpdateColPossibilities(row, col, value)) didchange = true;
        if (UpdateBoxPossibilities(row, col, value)) didchange = true;

        return didchange;
    }
    public int GetBoxIndex(int row, int col)
    {
        return Math.Min((row / boxSize) * boxSize + (col / boxSize), Size - 1);
    }
    public bool UpdateRowPossibilities(int row, int col, T value)
    {
        bool changed = false;
        for (int j = 0; j < Size; j++)
        {
            if (!board[row, j].IsPermanent())
                if(board[row, j].RemovePossibility(value)) 
                    changed = true;
        }
        return changed;
    }
    public bool UpdateColPossibilities(int row, int col, T value)
    {
        bool changed = false;
        for (int j = 0; j < Size; j++)
        {
            if (!board[j, col].IsPermanent())
               if( board[j, col].RemovePossibility(value)) changed = true;
        }
        return changed;
    }
    public bool UpdateBoxPossibilities(int row, int col, T value)
    {
        bool changed = false;

        int startRow = (row / boxSize) * boxSize;
        int startCol = (col / boxSize) * boxSize;

        for (int i = startRow; i < startRow + boxSize; i++)
            for (int j = startCol; j < startCol + boxSize; j++)
                if (!board[i, j].IsPermanent())
                    if (board[i, j].RemovePossibility(value)) changed = true ;
        return changed;
    }
    public bool CanPlaceValue(int row, int col, T value)
    {
        return rows[row].Contains(value) && cols[col].Contains(value) &&
               boxes[GetBoxIndex(row, col)].Contains(value) && !board[row, col].IsPermanent();
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
    public bool IsBoardSolved()
    {
        for (int i = 0; i < Size; i++)
            for (int j = 0 ; j < Size; j++)
                if (!board[i,j].IsPermanent()) return false;
        return true;    
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
    public int GetBoxSize()
    {
        return this.boxSize;
    }
    public List<(int row, int col)> GetCellsInBox(int boxIndex)
    {
        List<(int row, int col)> cells = new List<(int row, int col)>();
        int startRow = (boxIndex / boxSize) * boxSize;
        int startCol = (boxIndex % boxSize) * boxSize; 

        for (int row = startRow; row < startRow + boxSize; row++)
        {
            for (int col = startCol; col < startCol + boxSize; col++)
            {
                cells.Add((row, col));
            }
        }
        return cells;
    }
}

