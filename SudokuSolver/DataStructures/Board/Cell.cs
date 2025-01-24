using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.DataStructures.Board;


/// <summary>
/// this class represents each cell of the board. 
/// It is generic and flexible.
/// In this class you will have all the actions you need to interact and change the Cells in the 
/// Board.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Cell<T>
{
    private HashSet<T> _possibilities;
    private bool _isPermanent;
    private int row;
    private int col;    

    /// <summary>
    /// this is one of the constructors , it will get all the possibilities in range and 
    /// insert it to the possibilities property.
    /// also , it will declare the cell to be not permanent. 
    /// this constructor will be called for cells that the value inputed for them is 0 (for ints) .
    /// </summary>
    /// <param name="possibilities"> range of the options</param>
    public Cell(IEnumerable<T> possibilities, int row , int col )
    {
        _possibilities = possibilities.ToHashSet<T>();
        _isPermanent = false;
        this.row = row;
        this.col = col; 
    }
    /// <summary>
    ///  this is the second constructor , it will get one value
    /// and insert it to the possibilities property.
    /// also , it will declare the cell to be permanent. 
    /// this constructor will be called for cells that the value inputed for them is not 0 (for ints) .
    /// </summary>
    /// <param name="value">value inputed from the user (char from the string input)</param>
    public Cell(T value, int row , int col )
    {
        _possibilities = new HashSet<T>();
        _ = _possibilities.Add(value);
        _isPermanent = true;
        this.row = row;
        this.col = col;
    }

    /// <summary>
    /// if a cell is to be certain with a value , it will clear the possibilities, declare it with permanent, and add it to the 
    /// only possibility
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(T value)
    {
        _isPermanent = true;
        _possibilities.Clear(); 
        _possibilities.Add(value);
    }
    /// <summary>
    ///  returns if the cell is Permanent or not.
    /// </summary>
    /// <returns></returns>
    public bool IsPermanent()
    {
        return _isPermanent;
    }

    /// <summary>
    /// this will remove a possibility from a cell, and return if it is updated,
    /// if the cell is permanent it wont do anything
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public bool RemovePossibility(T option)
    {
        bool updated = false;
        if(!_isPermanent)
        {
            if (_possibilities.Remove(option)) updated = true;
            if (_possibilities.Count == 1)
                _isPermanent = true;
        }    
        return updated;
    }


    public void SetPossibilities(IEnumerable<T> possibilities)
    {
        _possibilities.Clear();
        _possibilities = possibilities.ToHashSet();
        _isPermanent = false;
    }

    /// <summary>
    /// returns Hashset of the possibilities of the cell.
    /// </summary>
    /// <returns></returns>
    public HashSet<T> GetPossibilities()
    {
        HashSet<T> result = new HashSet<T>(_possibilities);
        return result;
    }
    /// <summary>
    /// if the cell is permanent the function will returns it's value.
    /// else it will return 0 for ints.
    /// </summary>
    /// <returns></returns>
    public T? GetValue()
    {
        if (_isPermanent)
            return _possibilities.First();
        return default;
    }

    public int GetRow()
    {
        return this.row;
    }
    public int GetCol()
    {
        return this.col;
    }


}

