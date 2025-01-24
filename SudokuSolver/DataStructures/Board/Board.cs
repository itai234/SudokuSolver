using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.DataStructures.Board;

/// <summary>
/// this is the main class that represent a general board with possibilities, and from him the sudoku board
/// will inherit from and use his functions. 
/// each element in the board is a cell and has a class for it (the cell includes his possibilities, and functions to handle them)
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Board<T>
{
    public Cell<T>[,] board { get; protected set; }
    public int Size { get; }

    protected readonly T MIN_VALUE;
    protected readonly T MAX_VALUE;

    /// <summary>
    /// the constructor of the class to build the board.
    /// </summary>
    /// <param name="input"> represents the string that the user inputs to represent the board.</param>
    public Board(string input)
    {
        Size = (int)Math.Sqrt(input.Length);
        board = new Cell<T>[Size, Size];
        MIN_VALUE = (T)Convert.ChangeType(1, typeof(T));
        MAX_VALUE = (T)Convert.ChangeType(Size, typeof(T));
        initializeBoardForIntegers(input);
    }

    /// <summary>
    /// this will initialize the board with range of options for each cell ( each one is a range of ints)
    /// </summary>
    /// <param name="input">the string of the user's input </param>
    private void initializeBoardForIntegers(string input)
    {
        IEnumerable<T> range = GetRangeForInt(Convert.ToInt32(MIN_VALUE), Size);
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                SetIntValueForCell(input, i, j,range);

    }

    /// <summary>
    /// the function will decide wether the cell has a value that is permenant or not.
    /// and according to this will put in the cell either all his options or one permanant option 
    /// if the char is 0 it will be range of ints from min value (usually 1) to the max value (board size) 
    /// if its a number then the cell will be defined as "Permanent"
    /// </summary>
    /// <param name="input"> the string from the user's input</param>
    /// <param name="i">represents the row that the cell is in it</param>
    /// <param name="j"> represents the column that the cell is in it </param>
    /// <param name="range"> the range of the numbers flexible to the board's size.</param>
    private void SetIntValueForCell(string input, int i, int j,IEnumerable<T> range )
    {
        char currentChar = input[i * Size + j];
        int number = int.Parse(currentChar.ToString());
        if (number == 0)      
            board[i, j] = new Cell<T>(range,i,j);
        else
            board[i, j] = new Cell<T>((T)Convert.ChangeType(number, typeof(T)),i,j);
        
    }
    /// <summary>
    /// gets the range for the int board.
    /// </summary>
    /// <param name="start">start of range --> (1) </param>
    /// <param name="end"> end of range --> (Board Size)</param>
    /// <returns></returns>
    protected IEnumerable<T> GetRangeForInt(int start, int end)
    {
        return Enumerable.Range(start, end).Cast<T>();
    }
    /// <summary>
    /// the function will print out the board but this display is temporay for now . 
    /// </summary>
    public void DisplayBoard()
    {
        int boxSize = (int)Math.Sqrt(Size);
        string horizontalSeparator = new string('-', Size*3  + boxSize - 1);

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
                    Console.Write($"{cell.GetValue()} ");
                }
                else
                {
                    Console.Write("0 ");
                }
            }
            Console.WriteLine();
        }
    }


}

