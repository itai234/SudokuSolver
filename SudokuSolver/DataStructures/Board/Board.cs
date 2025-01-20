using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.DataStructures.Board;


public class Board<T>
{
    protected Cell<T>[,] board { get; set; }
    protected int Size { get; }

    protected readonly T MIN_VALUE;
    protected readonly T MAX_VALUE;

    public Board(string input)
    {
        Size = (int)Math.Sqrt(input.Length);
        board = new Cell<T>[Size, Size];
        MIN_VALUE = (T)Convert.ChangeType(1, typeof(T));
        MAX_VALUE = (T)Convert.ChangeType(Size, typeof(T));
        initializeBoardForIntegers(input);
    }

    private void initializeBoardForIntegers(string input)
    {
        IEnumerable<T> range = GetRangeForInt(Convert.ToInt32(MIN_VALUE), Size);
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                SetIntValueForCell(input, i, j,range);
    }

    private void SetIntValueForCell(string input, int i, int j,IEnumerable<T> range )
    {
        char currentChar = input[i * Size + j];
        int number = int.Parse(currentChar.ToString());
        if (number == 0)      
            board[i, j] = new Cell<T>(range);
        else
            board[i, j] = new Cell<T>((T)Convert.ChangeType(number, typeof(T)));
    }
    protected IEnumerable<T> GetRangeForInt(int start, int end)
    {
        return Enumerable.Range(start, end).Cast<T>();
    }
    public void DisplayBoard()
    {
        // the display is temporary for debug.
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Console.Write(board[i, j].IsPermanent() ?  $"{board[i, j].GetValue()} " : "0 " );
            }
            Console.WriteLine();
        }
    }
}

