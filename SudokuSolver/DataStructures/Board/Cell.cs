using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.DataStructures.Board;

public class Cell<T> 
{ 
    private HashSet<T> _possibilities{ get; set; }
    private bool _isPermanent = false;

    public Cell(IEnumerable<T> possibilities)
    {
        _possibilities = new HashSet<T>(possibilities); 
    }

    public bool IsPermanent()
    {
        return _possibilities.Count == 1 && !_possibilities.Contains(default(T));
    }

    public void Removepossibility(T option)
    {
        _possibilities.Remove(option);  
    }
}

