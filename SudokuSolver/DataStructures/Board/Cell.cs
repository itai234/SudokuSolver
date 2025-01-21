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

    public Cell(IEnumerable<T> possibilities)
    {
        _possibilities = possibilities.ToHashSet<T>();
        _isPermanent = false;
    }
    public Cell(T value)
    {
        _possibilities = new HashSet<T>();
        _possibilities.Add(value);  
        _isPermanent = true; 
    }

    public void SetValue(T value)
    {
        _isPermanent = true;
        _possibilities.Clear(); 
        _possibilities.Add(value);
    }
    public bool IsPermanent()
    {
        return _isPermanent || (_possibilities.Count == 1 && !_possibilities.Contains(default(T)));
    }

    public void RemovePossibility(T option)
    {
        if(!_isPermanent) 
            _possibilities.Remove(option);
        if (_possibilities.Count == 1)
            _isPermanent = true;
    }

    public HashSet<T> GetPossibilities()
    {
        return _possibilities;
    }
    public T? GetValue()
    {
        if (_isPermanent)
            return _possibilities.First();
        return default;
    }


}

