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

    /// <summary>
    /// base constructor, gets as input the string of the user input , 
    /// call the board class to initialize the board , check if the board is valid , creates hashsets to represent
    /// each row col and boxes options , and updates the board (eliminate possibilities)
    /// </summary>
    /// <param name="input"></param>
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
    /// <summary>
    /// creates a possibilties  hashset for the cells and return it .
    /// </summary>
    /// <returns></returns>
    public HashSet<T> CreatePossibilitySet()
    {
        IEnumerable<T> range = GetRangeForInt(Convert.ToInt32(MIN_VALUE), Size);
        return new HashSet<T>(range);
    }
    /// <summary>
    /// this function loops through each cell in the board , and if the cell is permanent , it will remove the possibilities 
    /// of this value at its box, col , and row 
    /// after it, the function will check if the board is valid.
    /// and returns true if changes were made or false if not.
    /// </summary>
    public bool UpdateBoard()
    {
        bool didchange = false;
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                if (board[i, j].IsPermanent())
                    didchange = didchange | RemoveValueFromPossibilities(i, j, board[i, j].GetValue());

        ValidateBoard<T>.Validate(board, SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
        return didchange;
    }
 
    /// <summary>
    /// the function removes the possibility of a certain value from the hashsets of the rows cols and boxes,
    /// that this value is in , and updates all the rows cols and boxes.
    /// the function returns true if changes were made.
    /// </summary>
    /// <param name="row"> the row that this values is in</param>
    /// <param name="col"> the col that this value is in</param>
    /// <param name="value"> the value that this cell represents.</param>
    /// <returns></returns>
    public bool RemoveValueFromPossibilities(int row, int col, T value)
    {
        bool didchange = false;
        if (rows[row].Remove(value)) didchange = true;
        if (cols[col].Remove(value)) didchange = true;

        int boxIndex = GetBoxIndex(row, col);
        if (boxes[boxIndex].Remove(value)) didchange = true;

        if (UpdateRowPossibilities(row, value)) didchange = true;
        if (UpdateColPossibilities( col, value)) didchange = true;
        if (UpdateBoxPossibilities(row, col, value)) didchange = true;

        return didchange;
    }
    /// <summary>
    /// the function returns the box index of a certain cell.
    /// </summary>
    /// <param name="row"> represents the cell's row</param>
    /// <param name="col"> represents the cell's col</param>
    /// <returns></returns>
    public int GetBoxIndex(int row, int col)
    {
        return Math.Min((row / boxSize) * boxSize + (col / boxSize), Size - 1);
    }
    /// <summary>
    /// updates a certain row possibilities, with a value that is permanent in this row
    /// </summary>
    /// <param name="row"> represents the row num</param>
    /// <param name="value"> represents the value that is permenant in this row</param>
    /// <returns></returns>
    public bool UpdateRowPossibilities(int row, T value)
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
    /// <summary>
    /// updates a certain column possibilities, with a value that is permanent in this column
    /// </summary>
    /// <param name="col">represents the col to be updated.</param>
    /// <param name="value">  represents the value that is permenant in this column</param>
    /// <returns></returns>
    public bool UpdateColPossibilities(int col, T value)
    {
        bool changed = false;
        for (int j = 0; j < Size; j++)
        {
            if (!board[j, col].IsPermanent())
               if( board[j, col].RemovePossibility(value)) changed = true;
        }
        return changed;
    }
    /// <summary>
    ///  updates a certain box possibilities, with a value that is permanent in this box
    /// </summary>
    /// <param name="row">row in the box </param>
    /// <param name="col"> col in the box</param>
    /// <param name="value"> a value that is permanent in this box</param>
    /// <returns></returns>
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
    /// <summary>
    /// the function checks if a value can be put in a certain location in the board
    /// </summary>
    /// <param name="row">the row of the value</param>
    /// <param name="col"> the column of the value</param>
    /// <param name="value"> the value that wanted to be inserted to the element</param>
    /// <returns></returns>
    public bool CanPlaceValue(int row, int col, T value)
    {
        return rows[row].Contains(value) && cols[col].Contains(value) &&
               boxes[GetBoxIndex(row, col)].Contains(value) && !board[row, col].IsPermanent();
    }
    /// <summary>
    /// sets a cell value and updates the possibilites in his row, col, and box
    /// </summary>
    /// <param name="row"> row of the value</param>
    /// <param name="col"> col of the value</param>
    /// <param name="value"> the value to set.</param>
    public void SetCellValue(int row, int col, T value)
    {
        board[row, col].SetValue(value);
        RemoveValueFromPossibilities(row, col, value);
    }
    /// <summary>
    /// the function searches through out the board for the cell with the least options in it 
    /// and returns it , if nothing was found it returns null.
    /// </summary>
    /// <returns></returns>
    public (int row, int col)? FindCellWithLeastPossibilities()
    {
        int min = Convert.ToInt32(MAX_VALUE)+1;
        int? row = null;
        int? col = null;
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                if (!board[i,j].IsPermanent() &&board[i, j].GetPossibilities().Count() < min)
                {
                    min = board[i, j].GetPossibilities().Count();
                    row = i; 
                    col = j; 
                }
        return (row.HasValue && col.HasValue) ? (row.Value, col.Value) : null;
    }
    /// <summary>
    ///  returns true if all the cells in the boards are permanent.
    /// </summary>
    /// <returns></returns>
    public bool IsBoardSolved()
    {
        for (int i = 0; i < Size; i++)
            for (int j = 0 ; j < Size; j++)
                if (!board[i,j].IsPermanent()) return false;
        return true;    
    }

    /// <summary>
    /// returns the row Possibilities
    /// </summary>
    /// <param name="row"> a certain row</param>
    /// <returns></returns>
    public HashSet<T> GetRowPossibilities(int row)
    {
        return rows[row];
    }
    /// <summary>
    /// returns a col possibilities
    /// </summary>
    /// <param name="col"> a certain col</param>
    /// <returns></returns>
    public HashSet<T> GetColPossibilities(int col)
    {
        return cols[col];
    }
    /// <summary>
    /// return a box possibilities
    /// </summary>
    /// <param name="boxIndex"> a  certain box</param>
    /// <returns></returns>
    public HashSet<T> GetBoxPossibilities(int boxIndex)
    {
        return boxes[boxIndex];
    }
    /// <summary>
    ///  return the box size in a board.
    /// </summary>
    /// <returns></returns>
    public int GetBoxSize()
    {
        return this.boxSize;
    }
    /// <summary>
    ///  returns list of all the cells in a box 
    /// </summary>
    /// <param name="boxIndex"> a certain box index.</param>
    /// <returns></returns>
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

    /// <summary>
    /// the function saves the board's state in a dictionary that it's key is an location in the board
    /// and its value is the possiblities that location had . 
    /// </summary>
    /// <returns> returns the dictionary containing the possibilities of the board cells that are not permanent.</returns>
    public Dictionary<(int row, int col), HashSet<T>> SaveBoardState()
    {
        Dictionary<(int row, int col), HashSet<T>> stateForBoard = new Dictionary<(int row, int col), HashSet<T>>();
        for (int row = 0; row< Size; row++ )
        {
            for(int col = 0; col < Size; col++)
            {
                if(!board[row, col].IsPermanent())
                {
                    stateForBoard[(row,col)] = board[row, col].GetPossibilities(); 
                }
            }
        }
        return stateForBoard;
    }
    public HashSet<T>[] SaveRowsState()
    {
        return rows.Select(set => new HashSet<T>(set)).ToArray();
    }
    public HashSet<T>[] SaveColsState()
    {
        return cols.Select(set => new HashSet<T>(set)).ToArray();
    }

    public HashSet<T>[] SaveBoxesState()
    {
        return boxes.Select(set => new HashSet<T>(set)).ToArray();
    }
    /// <summary>
    /// the function restores the board state , with the state dictionary that it has as input ,
    /// it restores each cell in it.
    /// </summary>
    /// <param name="state"> a certain board state represented by dictionary of location,possibilities.</param>
    public void RestoreBoardState(Dictionary<(int row, int col), HashSet<T>> state)
    {
        foreach (var element in state)
        {
            var (row, col) = element.Key;
            HashSet<T> possibilities = element.Value;
            board[row, col].SetPossibilities(possibilities);
        }
    }
    public void RestorePropertiesState(HashSet<T>[] rows, HashSet<T>[] cols, HashSet<T>[] boxes)
    {
        for (int i = 0; i < this.rows.Length; i++)
        {
            this.rows[i] = new HashSet<T>(rows[i]);
        }

        for (int i = 0; i < this.cols.Length; i++)
        {
            this.cols[i] = new HashSet<T>(cols[i]);
        }

        for (int i = 0; i < this.boxes.Length; i++)
        {
            this.boxes[i] = new HashSet<T>(boxes[i]);
        }
    }

}

