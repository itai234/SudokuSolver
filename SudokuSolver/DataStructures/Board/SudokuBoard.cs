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
    public HashSet<T>[] Rows { get; }
    public HashSet<T>[] Cols { get; }
    public HashSet<T>[] Boxes { get; }

    private readonly int boxSize;

    /// <summary>
    /// base constructor, gets as input the string of the user input , 
    /// call the board class to initialize the board , check if the board is valid , creates hashsets to represent
    /// each row col and boxes options , and updates the board (eliminate possibilities)
    /// </summary>
    /// <param name="input"></param>
    public SudokuBoard(string input) : base(input)
    {
        ValidateBoard<T>.Validate(BoardGrid, SudokuBoardUtilities.GameStateForValidation.BaseBoardInput);
        Rows = new HashSet<T>[Size];
        Cols = new HashSet<T>[Size];
        Boxes = new HashSet<T>[Size];
        boxSize = (int)Math.Sqrt(Size);

        for (int index = 0; index < Size; index++)
        {
            Rows[index] = CreatePossibilitySet();
            Cols[index] = CreatePossibilitySet();
            Boxes[index] = CreatePossibilitySet();
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
        for (int row = 0; row < Size; row++)
            for (int col = 0; col < Size; col++)
                if (BoardGrid[row, col].IsPermanent())
                    didchange = didchange | RemoveValueFromPossibilities(row, col, BoardGrid[row, col].GetValue());

        ValidateBoard<T>.Validate(BoardGrid, SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
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
        if (Rows[row].Remove(value)) didchange = true;
        if (Cols[col].Remove(value)) didchange = true;

        int boxIndex = GetBoxIndex(row, col);
        if (Boxes[boxIndex].Remove(value)) didchange = true;

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
        for (int col = 0; col < Size; col++)
        {
            if (!BoardGrid[row, col].IsPermanent())
                if(BoardGrid[row, col].RemovePossibility(value)) 
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
        for (int row = 0; row < Size; row++)
        {
            if (!BoardGrid[row, col].IsPermanent())
               if( BoardGrid[row, col].RemovePossibility(value)) changed = true;
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

        for (int Row = startRow; Row < startRow + boxSize; Row++)
            for (int Col = startCol; Col < startCol + boxSize; Col++)
                if (!BoardGrid[Row, Col].IsPermanent())
                    if (BoardGrid[Row, Col].RemovePossibility(value)) changed = true ;
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
        return Rows[row].Contains(value) && Cols[col].Contains(value) &&
               Boxes[GetBoxIndex(row, col)].Contains(value) && !BoardGrid[row, col].IsPermanent();
    }
    /// <summary>
    /// sets a cell value and updates the possibilites in his row, col, and box
    /// </summary>
    /// <param name="row"> row of the value</param>
    /// <param name="col"> col of the value</param>
    /// <param name="value"> the value to set.</param>
    public void SetCellValue(int row, int col, T value)
    {
        BoardGrid[row, col].SetValue(value);
        RemoveValueFromPossibilities(row, col, value);
    }
    /// <summary>
    /// the function searches through out the board for the cell with the least options in it 
    /// and returns it , if nothing was found it returns null.
    /// it the function finds more than 1 cell with the same least possibilities it will make a 
    /// decision which cell to choose according to his Degree in the board.
    /// </summary>
    /// <returns></returns>
    public (int row, int col)? FindCellWithLeastPossibilities()
    {

        List<(int row, int col)> LeastPossibilitiesCellsList = new List<(int row, int col)>();
        GetLeastPossibilities(ref  LeastPossibilitiesCellsList);

        if (!LeastPossibilitiesCellsList.Any())
        {
            return null;  
        }
        if (LeastPossibilitiesCellsList.Count > 1)
        {
            int maxDegree = -1;
            List<(int row, int col)> finalCellsList = new List<(int row, int col)>();

            foreach ((int row, int col) in LeastPossibilitiesCellsList)
            {
                int degree = GetDegreeOfCell(row, col);
                if (degree > maxDegree)
                {
                    maxDegree = degree;
                    finalCellsList.Clear(); 
                    finalCellsList.Add((row, col));
                }
                else if (degree == maxDegree)
                {
                    finalCellsList.Add((row, col)); 
                }
            }
            return finalCellsList.First();
        }
        return LeastPossibilitiesCellsList.First() ;
    }
    
    /// <summary>
    /// Gets as an input the empty list of tuples , and add to it the cells that have the least possibilities
    /// </summary>
    /// <param name="LeastPossibilitiesCellsList"> empty list </param>
    public void GetLeastPossibilities(ref List<(int row, int col)> LeastPossibilitiesCellsList)
    {
        int min = Convert.ToInt32(MAX_VALUE) + 1;

        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                if (!BoardGrid[row, col].IsPermanent())
                {
                    int possibilitiesCount = BoardGrid[row, col].GetPossibilities().Count();

                    if (possibilitiesCount < min)
                    {
                        LeastPossibilitiesCellsList.Clear();
                        min = possibilitiesCount;
                        LeastPossibilitiesCellsList.Add((row, col));
                    }
                    else if (possibilitiesCount == min)
                    {
                        LeastPossibilitiesCellsList.Add((row, col));
                    }
                }
            }
        }
    }
    /// <summary>
    /// returns the degree of a certain cell, meaning , returns the number of permanent elements 
    /// there are in his row ,col or box. this is for the backtracking to work faster.
    /// </summary>
    /// <param name="row">the row of the cell</param>
    /// <param name="col"> the column of the cell</param>
    /// <returns></returns>
    private int GetDegreeOfCell(int row, int col)
    {
        int degree = 0;
        degree += Size - Rows[row].Count();
        degree += Size - Cols[col].Count();
        degree += Size - Boxes[GetBoxIndex(row,col)].Count();
        return degree;
    }

    /// <summary>
    ///  returns true if all the cells in the boards are permanent.
    /// </summary>
    /// <returns></returns>
    public bool IsBoardSolved()
    {
        for (int row = 0; row < Size; row++)
            for (int col = 0 ; col < Size; col++)
                if (!BoardGrid[row,col].IsPermanent()) return false;
        return true;    
    }

    /// <summary>
    /// returns the row Possibilities
    /// </summary>
    /// <param name="row"> a certain row</param>
    /// <returns></returns>
    public HashSet<T> GetRowPossibilities(int row)
    {
        return Rows[row];
    }
    /// <summary>
    /// returns a col possibilities
    /// </summary>
    /// <param name="col"> a certain col</param>
    /// <returns></returns>
    public HashSet<T> GetColPossibilities(int col)
    {
        return Cols[col];
    }
    /// <summary>
    /// return a box possibilities
    /// </summary>
    /// <param name="boxIndex"> a  certain box</param>
    /// <returns></returns>
    public HashSet<T> GetBoxPossibilities(int boxIndex)
    {
        return Boxes[boxIndex];
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
    /// the function saves the boards state with a copy of all the cells, and returns the matrix of the 
    /// cells.
    ///  </summary>
    /// <returns> returns the dictionary containing the possibilities of the board cells that are not permanent.</returns>
    public Cell<T>[,] SaveBoardState()
    {
        Cell<T>[,] stateForBoard = new Cell<T>[Size,Size];
        for (int row = 0; row< Size; row++ )
        {
            for(int col = 0; col < Size; col++)
            {
                stateForBoard[row, col] = new Cell<T>(this.BoardGrid[row, col].GetPossibilities(),row,col);
            }
        }
        return stateForBoard;
    }
    public HashSet<T>[] SaveRowsState()
    {
        return Rows.Select(set => new HashSet<T>(set)).ToArray();
    }
    public HashSet<T>[] SaveColsState()
    {
        return Cols.Select(set => new HashSet<T>(set)).ToArray();
    }

    public HashSet<T>[] SaveBoxesState()
    {
        return Boxes.Select(set => new HashSet<T>(set)).ToArray();
    }
    /// <summary>
    /// the function restores the board state , with the state cells matrix  that it has as input ,
    /// it restores each cell in it.
    /// </summary>
    /// <param name="state"> a certain board state represented by dictionary of location,possibilities.</param>
    public void RestoreBoardState(Cell<T>[,] state)
    {
        for(int row = 0; row < Size; row++)
        {
            for (int col = 0;col < Size; col++)
            {
                this.BoardGrid[row, col].SetPossibilities(state[row, col].GetPossibilities());
            }
        }
    }
    /// <summary>
    /// retores all the values back to their old ones, this function is called from the backtracking 
    /// when a value that has been chosen leads to a dead path.
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="cols"></param>
    /// <param name="boxes"></param>
    public void RestorePropertiesState(HashSet<T>[] rows, HashSet<T>[] cols, HashSet<T>[] boxes)
    {
        for (int index = 0; index < this.Rows.Length; index++)
        {
            this.Rows[index] = new HashSet<T>(rows[index]);
        }

        for (int index = 0; index < this.Cols.Length; index++)
        {
            this.Cols[index] = new HashSet<T>(cols[index]);
        }

        for (int index = 0; index < this.Boxes.Length; index++)
        {
            this.Boxes[index] = new HashSet<T>(boxes[index]);
        }
    }

    /// <summary>
    /// this function returns a list of cells indexes in the board that are on the row/col/box of the cell that the function gets.
    /// </summary>
    /// <param name="row"> the cell's row </param>
    /// <param name="col"> the cell's col </param>
    /// <returns></returns>
    public List<(int row, int col)> GetCellsBesidesItself(int row, int col)
    {
        List<(int row, int col)> Cells = new List<(int row , int col)>();

        for (int Col = 0; Col < Size; Col++)
            if (Col != col) Cells.Add((row, Col));

        for (int Row = 0; Row < Size; Row++)
            if (Row != row) Cells.Add((Row, col));

        foreach ((int rowIn, int colIn) in GetCellsInBox(GetBoxIndex(row, col)))
            if (rowIn != row || colIn != col)
                Cells.Add((rowIn, colIn));

        return Cells;
    }

}

