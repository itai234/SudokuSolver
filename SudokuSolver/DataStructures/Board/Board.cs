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
        for (int row = 0; row < Size; row++)
            for (int col = 0; col < Size; col++)
                SetIntValueForCell(input, row, col, range);

    }

    /// <summary>
    /// the function will decide wether the cell has a value that is permenant or not.
    /// and according to this will put in the cell either all his options or one permanant option 
    /// if the char is 0 it will be range of ints from min value (usually 1) to the max value (board size) 
    /// if its a number then the cell will be defined as "Permanent"
    /// </summary>
    /// <param name="input"> the string from the user's input</param>
    /// <param name="row">represents the row that the cell is in it</param>
    /// <param name="col"> represents the column that the cell is in it </param>
    /// <param name="range"> the range of the numbers flexible to the board's size.</param>
    private void SetIntValueForCell(string input, int row, int col, IEnumerable<T> range)
    {
        char currentChar = input[row * Size + col];
        int number = currentChar - '0';
        if (number == 0)
            board[row, col] = new Cell<T>(range, row, col);
        else
            board[row, col] = new Cell<T>((T)Convert.ChangeType(number, typeof(T)), row, col);

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
    /// the function will return a string of the board 
    /// and afterwards when you print it out it will be organized and cool.
    /// </summary>
    /// <returns> string representing the board. </returns>
    public string DisplayBoard()
    {
        var sb = new StringBuilder();
        var rowLabels = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var colLabels = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int boxSize = (int)Math.Sqrt(Size);

        int cellWidth = (int)Math.Floor(Math.Log10(Size) + 1);
        if (cellWidth < 1) cellWidth = 1;
        int lineSegmentWidth = cellWidth + 2;

        sb.Append("   ");
        for (int c = 0; c < Size; c++)
        {
            sb.Append(" ");
            string label = colLabels[c].ToString().PadLeft(cellWidth+1 );
            sb.Append(label);
            sb.Append(" ");
            if ((c + 1) % boxSize == 0 && c < Size - 1) sb.Append(" ");
        }
        sb.AppendLine();

        sb.Append("   ");
        sb.AppendLine(BuildLine(true, false, lineSegmentWidth));

        for (int r = 0; r < Size; r++)
        {
            sb.Append(" ");
            sb.Append(rowLabels[r]);
            sb.Append(" ");
            for (int c = 0; c < Size; c++)
            {
                if (c % boxSize == 0) sb.Append("║");
                else sb.Append("│");

                var val = board[r, c].IsPermanent() ? board[r, c].GetValue().ToString() : "0";
                val = val.PadLeft(cellWidth, ' ');
                sb.Append($" {val} ");
            }
            sb.Append("║");
            sb.AppendLine();

            sb.Append("   ");
            if (r < Size - 1)
            {
                if ((r + 1) % boxSize == 0)
                {
                    sb.AppendLine(BuildLine(false, false, lineSegmentWidth, thick: true));
                }
                else
                {
                    sb.AppendLine(BuildLine(false, false, lineSegmentWidth));
                }
            }
            else
            {
                sb.AppendLine(BuildLine(false, true, lineSegmentWidth));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// helper function to build a line in the display of the board.
    /// gets all the info about how should the line be built.
    /// </summary>
    /// <param name="top"></param>
    /// <param name="bottom"></param>
    /// <param name="segmentWidth"></param>
    /// <param name="thick"></param>
    /// <returns></returns>
    string BuildLine(bool top, bool bottom, int segmentWidth, bool thick = false)
    {
        var sb = new StringBuilder();
        int boxSize = (int)Math.Sqrt(Size);

        char left = top ? '╔' : (bottom ? '╚' : (thick ? '╠' : '├'));
        char mid = top ? '╦' : (bottom ? '╩' : (thick ? '╬' : '┼'));
        char right = top ? '╗' : (bottom ? '╝' : (thick ? '╣' : '┤'));
        char thin = '─';
        char bold = '═';

        sb.Append(left);
        for (int c = 0; c < Size; c++)
        {
            char lineChar = thick ? bold : thin;
            sb.Append(new string(lineChar, segmentWidth));
            if (c < Size - 1)
            {
                if ((c + 1) % boxSize == 0)
                {
                    sb.Append(thick ? mid : '┼');
                }
                else
                {
                    sb.Append(lineChar);
                }
            }
        }
        sb.Append(right);

        return sb.ToString();
    }

    public string BoardToString()
    {
        string result = "";
        for (int row = 0; row < Size; row++)
        {
            for(int col = 0; col < Size; col++)
            {
                result += board[row, col].IsPermanent() ? board[row, col].GetValue() : 0;
            }
        }
        return result;  
    }



}

