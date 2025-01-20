using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.DataStructures.Board;

public class Board<T>
{

    protected Cell<T>[,] board { get; set; }
    protected int Size { get;  }        
    public Board(string input)
    {
        Size = (int)Math.Sqrt(input.Length);
        board = new Cell<T>[Size, Size];
        initializeBoard(input);
    }

    private void initializeBoard(string input)
    {
        IEnumerable<T> range = GetRangeForInt(1, Size);   
        for (int i = 0; i < Size; i++)
        {
            for(int j = 0; j < Size; j++)
            {
                board[i, j] = new Cell<T>(range);
            }
        }
    }
    private IEnumerable<T>  GetRangeForInt(int start, int end)
    {
        return Enumerable.Range(start, end).Cast<T>();
    }
    public void displayBoard()
    {
       
    }

 
}

