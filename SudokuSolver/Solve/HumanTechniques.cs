using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve;

public class HumanTechniques<T> : ISolving<T>
{
    private SudokuBoard<T> sudokuBoard;
    public void SetBoard(SudokuBoard<T> sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
    }
    public void Solve()
    {
        bool changed = false; 
        do
        {
            changed = LockedCandidatesBlockWithinRow();
        } while (changed && !sudokuBoard.IsBoardSolved());
    }

    public bool LockedCandidatesBlockWithinRow()
    {
        HashSet<T> AllPossibilities = sudokuBoard.CreatePossibilitySet();
        int boxSize = sudokuBoard.GetBoxSize();
        int boxIndex;
        Dictionary<int, List<int>> RowAndCols = new Dictionary<int, List<int>>();
        bool didChange = false;
        for (int row = 0; row < sudokuBoard.Size; row++)
        {
            foreach (T candidate in AllPossibilities)
            {
                RowAndCols.Clear();
                for (int col = 0; col < sudokuBoard.Size; col++)
                {
                    if (!sudokuBoard.board[row, col].IsPermanent() &&
                        sudokuBoard.board[row, col].GetPossibilities().Contains(candidate))
                    {
                        if (!RowAndCols.ContainsKey(row))
                        {
                            RowAndCols[row] = new List<int>();
                        }
                        RowAndCols[row].Add(col);
                    }
                }
                if (RowAndCols.ContainsKey(row) && RowAndCols[row].Count >= 1 && RowAndCols[row].Count <= boxSize)
                {
                    boxIndex = sudokuBoard.GetBoxIndex(row, RowAndCols[row][0]);
                    bool allInSameBox = RowAndCols[row].All(col => sudokuBoard.GetBoxIndex(row, col) == boxIndex);
                    if (allInSameBox)
                    {
                        List<(int row, int col)> cellsInBox = sudokuBoard.GetCellsInBox(boxIndex);
                        foreach (var cell in cellsInBox)
                            if (cell.row != row && !sudokuBoard.board[cell.row, cell.col].IsPermanent()
                                && sudokuBoard.board[cell.row, cell.col].GetPossibilities().Contains(candidate))
                            {
                                sudokuBoard.board[cell.row, cell.col].RemovePossibility(candidate);
                                didChange = true;   
                            }
                    }
                }
            }
        }
        return didChange;   
    }
}

