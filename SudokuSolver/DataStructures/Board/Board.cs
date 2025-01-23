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
    public Cell<T>[,] board { get; protected set; }
    public int Size { get; }

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
        int boxSize = (int)Math.Sqrt(Size);
        string horizontalSeparator = new string('-', Size * 10 + boxSize - 1);

        for (int i = 0; i < Size; i++)
        {
            if (i % boxSize == 0 && i != 0)
                Console.WriteLine(horizontalSeparator);

            for (int j = 0; j < Size; j++)
            {
                if (j % boxSize == 0 && j != 0)
                    Console.Write(" | ");

                var cell = board[i, j];
                if (cell.IsPermanent())
                {
                    Console.Write($" {cell.GetValue()}         ");
                }
                else
                {
                    string possibilities = string.Join("", cell.GetPossibilities());
                    
                    Console.Write($"{possibilities.PadRight(9)}");
                }
            }
            Console.WriteLine();
        }
    }


}

