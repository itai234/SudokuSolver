using SudokuSolver.DataStructures.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Solve.HumanSolving;

public static class NakedSets<T>
{
    /// <summary>
    /// main function to call when wanting to summon the naked sets checking.
    /// the function checks from 2 pairs, to the board size -2 (included) .
    /// </summary>
    /// <returns> return true if changes were made .</returns>
    public static bool ApplyNakedSets(int minNum, int maxNum, SudokuBoard<T> grid)
    {
        bool didChange = false;
        for (int SetSize = minNum; SetSize < maxNum; SetSize++)
            didChange |= NakedSet(SetSize, grid);
        return didChange;
    }
    /// <summary>
    /// this function loops through the board size, and for each location , it will call the
    /// main function to find the sets -> it sends her each iteration, for each location in the board size 
    /// ( from 1 to the size of the board ) , all the rows,cols, and boxes cells.
    /// </summary>
    /// <param name="SetSize"> the size of the set to find (pairs, triples) ... </param>
    /// <returns> returns true if changes in the board were made. </returns>
    private static bool NakedSet(int SetSize, SudokuBoard<T> grid)
    {
        bool didChange = false;
        for (int location = 0; location < grid.Size; location++)
        {

            if (NakedSetFind(SolvingUtilities.SolvingUtilities<T>.GetRowCells(location, grid), SetSize, grid))
                didChange = true;
            if (NakedSetFind(SolvingUtilities.SolvingUtilities<T>.GetColumnCells(location, grid), SetSize, grid))
                didChange = true;
            if (NakedSetFind(SolvingUtilities.SolvingUtilities<T>.GetBoxCells(location, grid), SetSize, grid))
                didChange = true;
        }
        return didChange;
    }

    /// <summary>
    /// This is the main function to find the naked sets.
    /// The function takes an input a list of cells in a row/col/box and the set size (pairs, triples).
    /// The function generates all the potential Sets of the row/col/box cells.
    /// And for each Set it checks whether it is a valid set for naked sets ->
    /// (A set of cells in which each possibility is unique to that set, meaning that no other cells in the same ->
    /// row, column, or box can contain those possibilities.)
    /// If a valid set is found , All the possibilities from this set will be removed from the other Cells 
    /// int the row/col/box.
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="setSize"></param>
    /// <returns></returns>
    private static bool NakedSetFind(List<Cell<T>> cells, int setSize, SudokuBoard<T> grid)
    {
        bool didChange = false;
        List<List<Cell<T>>> potentialSets = GetCombinations(cells, setSize);

        foreach (List<Cell<T>> set in potentialSets)
        {
            HashSet<T> nakedCandidates = new HashSet<T>();

            foreach (Cell<T> cell in set)
                foreach (T possibility in cell.GetPossibilities())
                    nakedCandidates.Add(possibility);

            if (nakedCandidates.Count() == setSize)
                foreach (Cell<T> cell in cells)
                    if (!cell.IsPermanent() && !set.Contains(cell))
                        foreach (T candidate in nakedCandidates)
                            if (cell.RemovePossibility(candidate))
                                didChange = true;
        }
        return didChange;
    }


    /// <summary>
    ///The function receives a list of Cells representing a row/col/box , 
    ///and a setSize , the function will recursively generate all the sets of cells that are possibile
    /// and return them.
    /// </summary>
    /// <param name="Cells"></param>
    /// <param name="SetSize"></param>
    /// <returns></returns>
    private static List<List<Cell<T>>> GetCombinations(List<Cell<T>> Cells, int SetSize)
    {
        List<List<Cell<T>>> combinations = new List<List<Cell<T>>>();

        if (SetSize == 0)
        {
            combinations.Add(new List<Cell<T>>());
            return combinations;
        }

        for (int i = 0; i <= Cells.Count - SetSize; i++)
        {
            //recursive call to find all the possibile combinations of the setsize -1 , from the remaining cells --> Cells.Skip(i + 1)
            List<List<Cell<T>>> remainingCombinations = GetCombinations(Cells.Skip(i + 1).ToList(), SetSize - 1);

            foreach (List<Cell<T>> combination in remainingCombinations)
            {
                List<Cell<T>> result = new List<Cell<T>> { Cells[i] };
                result.AddRange(combination);
                combinations.Add(result);
            }
        }
        return combinations;
    }
}

