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
    private int boxSize;
    HashSet<T> possibilities;

    public void SetBoard(SudokuBoard<T> sudokuBoard)
    {
        this.sudokuBoard = sudokuBoard;
        boxSize = sudokuBoard.GetBoxSize();
        possibilities = sudokuBoard.CreatePossibilitySet();
    }

    public void Solve()
    {
        
        bool changed = false;
        if (this.sudokuBoard.IsBoardSolved())
            return;
        do
        {
            changed = sudokuBoard.UpdateBoardAfterTechnique()
                       || LockedCandidatesBlockWithinRowOrCol()
                       || LockedCandidatesRowOrColWithinBox();
        } while (changed && !sudokuBoard.IsBoardSolved());

          Validation.ValidateBoard<T>.Validate(
          sudokuBoard.board, Utilities.SudokuBoardUtilities.GameStateForValidation.BaseBoardWithPossibilitiesFixed);
    }

    public bool LockedCandidatesBlockWithinRowOrCol()
    {
        HashSet<T> allPossibilities = sudokuBoard.CreatePossibilitySet();
        bool didChangeRow = false;
        bool didChangeCol = false;

        for (int row = 0; row < sudokuBoard.Size; row++)
        {
            foreach (T candidate in allPossibilities)
            {
                LockedCandidatesBlockWithinRowOrColHelperFunction(row, candidate, ref didChangeRow, ref didChangeCol);
            }
        }
        return didChangeRow || didChangeCol;
    }

    public void LockedCandidatesBlockWithinRowOrColHelperFunction(int row, T candidate, ref bool didChangeRow, ref bool didChangeCol)
    {
        Dictionary<int, List<int>> RowAndCols = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> ColsAndRows = new Dictionary<int, List<int>>();

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

            if (!sudokuBoard.board[col, row].IsPermanent() &&
                sudokuBoard.board[col, row].GetPossibilities().Contains(candidate))
            {
                if (!ColsAndRows.ContainsKey(row))
                {
                    ColsAndRows[row] = new List<int>();
                }
                ColsAndRows[row].Add(col);
            }
        }

        LockedCandidatesBlockWithinRowHelperFunctionCondition(RowAndCols, row, candidate, ref didChangeRow);
        LockedCandidatesBlockWithinColHelperFunctionCondition(ColsAndRows, row, candidate, ref didChangeCol);
    }

    public void LockedCandidatesBlockWithinRowHelperFunctionCondition(Dictionary<int, List<int>> RowAndCols, int row, T candidate, ref bool didChangeRow)
    {
        if (RowAndCols.ContainsKey(row) && RowAndCols[row].Count >= 1 && RowAndCols[row].Count <= boxSize)
        {
            int boxIndex = sudokuBoard.GetBoxIndex(row, RowAndCols[row][0]);

            bool allInSameBox = RowAndCols[row].All(col => sudokuBoard.GetBoxIndex(row, col) == boxIndex);

            if (allInSameBox)
            {
                List<(int row, int col)> cellsInBox = sudokuBoard.GetCellsInBox(boxIndex);

                foreach (var cell in cellsInBox)
                {
                    if (cell.row != row && !sudokuBoard.board[cell.row, cell.col].IsPermanent() &&
                        sudokuBoard.board[cell.row, cell.col].GetPossibilities().Contains(candidate))
                    {
                        if (!sudokuBoard.board[row, cell.col].GetPossibilities().Contains(candidate))
                        {
                            RemoveCellPossibilityAndUpdate(cell.row, cell.col, candidate);
                            didChangeRow = true;
                        }
                    }
                }
            }
        }
    }

    public void LockedCandidatesBlockWithinColHelperFunctionCondition(Dictionary<int, List<int>> ColsAndRows, int col, T candidate, ref bool didChangeCol)
    {
        if (ColsAndRows.ContainsKey(col) && ColsAndRows[col].Count >= 1 && ColsAndRows[col].Count <= boxSize && sudokuBoard.cols[col].Contains(candidate))
        {
            int boxIndex = sudokuBoard.GetBoxIndex(ColsAndRows[col][0], col);
            bool allInSameBox = ColsAndRows[col].All(row => sudokuBoard.GetBoxIndex(row, col) == boxIndex);

            if (allInSameBox)
            {
                List<(int row, int col)> cellsInBox = sudokuBoard.GetCellsInBox(boxIndex);
                foreach (var cell in cellsInBox)
                {
                    if (cell.col != col && !sudokuBoard.board[cell.row, cell.col].IsPermanent() &&
                        sudokuBoard.board[cell.row, cell.col].GetPossibilities().Contains(candidate))
                    {
                        if (!sudokuBoard.board[cell.row, col].GetPossibilities().Contains(candidate))
                        {
                            RemoveCellPossibilityAndUpdate(cell.row, cell.col, candidate);
                            didChangeCol = true;
                        }
                    }
                }
            }
        }
    }

    public void RemoveCellPossibilityAndUpdate(int row, int col, T value)
    {
        if (sudokuBoard.board[row, col].GetPossibilities().Contains(value))
        {

            if (!sudokuBoard.board[row, col].RemovePossibility(value)) return;
            if (sudokuBoard.board[row, col].IsPermanent())
            {
                sudokuBoard.SetCellValue(row, col, sudokuBoard.board[row, col].GetValue());
            }
        }
    }
    public bool LockedCandidatesRowOrColWithinBox()
    {
        bool didChange = false;

        for (int boxIndex = 0; boxIndex < sudokuBoard.Size; boxIndex++)
        {
            foreach (T candidate in possibilities)
            {
                List<(int row, int col)> candidateCells = new List<(int row, int col)>();
                foreach ((int row, int col) in sudokuBoard.GetCellsInBox(boxIndex))
                {
                    if (!sudokuBoard.board[row, col].IsPermanent() &&
                        sudokuBoard.board[row, col].GetPossibilities().Contains(candidate))
                    {
                        candidateCells.Add((row, col));
                    }
                }

                if (candidateCells.Count < 2)
                    continue;
                bool rowChange = LockedCandidatesRowWithinBox(boxIndex, candidate, candidateCells);
                bool colChange = LockedCandidatesColWithinBox(boxIndex, candidate, candidateCells);
                didChange |= (rowChange || colChange);
            }
        }

        return didChange;
    }

    public bool LockedCandidatesRowWithinBox(int boxIndex, T candidate, List<(int row, int col)> candidateCells)
    {
        bool didChange = false;
        var groupedByRow = candidateCells.GroupBy(cell => cell.row).ToList();
        if (groupedByRow.Count == 1)
        {
            int lockedRow = groupedByRow[0].Key;
            for (int col = 0; col < sudokuBoard.Size; col++)
            {
                if (sudokuBoard.GetBoxIndex(lockedRow, col) != boxIndex)
                {
                    Cell<T> cell = sudokuBoard.board[lockedRow, col];
                    if (!cell.IsPermanent() && cell.GetPossibilities().Contains(candidate))
                    {
                        RemoveCellPossibilityAndUpdate(lockedRow, col, candidate);
                        didChange = true;

                    }
                }
            }
        }

        return didChange;
    }

    public bool LockedCandidatesColWithinBox(int boxIndex, T candidate, List<(int row, int col)> candidateCells)
    {
        bool didChange = false;
        var groupedByCol = candidateCells.GroupBy(cell => cell.col).ToList();
        if (groupedByCol.Count == 1)
        {
            int lockedCol = groupedByCol[0].Key;
            for (int row = 0; row < sudokuBoard.Size; row++)
            {
                if (sudokuBoard.GetBoxIndex(row, lockedCol) != boxIndex)
                {
                    Cell<T> cell = sudokuBoard.board[row, lockedCol];
                    if (!cell.IsPermanent() && cell.GetPossibilities().Contains(candidate))
                    {
                        RemoveCellPossibilityAndUpdate(row, lockedCol, candidate);
                        didChange = true;
                    }
                }
            }
        }
        return didChange;
    }




}
