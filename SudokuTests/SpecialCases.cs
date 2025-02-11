using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SudokuSolver.Solve;
using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using System.Diagnostics;
using SudokuSolver.Solve.ComputerAlgorithm;
using SudokuSolver.Solve.HumanHeuristics;

namespace SudokuTests;
/// <summary>
/// this class will check special cases 
/// like unsolvable boards , small boards and invalid input (duplicates or illegal size) .
/// </summary>
[TestFixture]
public class SpecialCases
{
    private HumanTechniques<int> _humanTechniques;
    private ComputerTechniques<int> _computerTechniques;
    private SudokuBoard<int> _sudokuBoard;
    private SudokuSolver.Validation.ValidateInput<int> _validator;
    private static readonly int _maxTimeForGrid = 1000;
    private SolverManager<int> _solver;
    private Stopwatch _stopwatch = new Stopwatch();

    [SetUp]
    public void Setup()
    {

    }


    private void solve(string input, string expectedResult)
    {


        try
        {
            _validator = new SudokuSolver.Validation.ValidateInput<int>(input);
            _validator.Validate();
            _sudokuBoard = new SudokuBoard<int>(input);
            _humanTechniques = new HumanTechniques<int>();
            _computerTechniques = new ComputerTechniques<int>();
            _solver = new SolverManager<int>(_sudokuBoard);
            _solver.AddTechnique(_humanTechniques);
            _solver.AddTechnique(_computerTechniques);
            _stopwatch.Reset();
            _stopwatch.Start();
            _solver.SolveBoard();
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            if (_stopwatch.ElapsedMilliseconds > _maxTimeForGrid && _sudokuBoard.Size <= 16)
                Assert.Fail("Took Too Long");
            Assert.That(ex.Message, Is.EqualTo(expectedResult));
        }
    }


    // unsolvable boards:



    [Test]
    public void TestUnsolvable1()
    {
        string input = "000005080000601043000000000010500000000106000300000005530000061000000004000000000";
        string result = "The Board You Entered Is Invalid and Unsolvable.";
        solve(input, result);
    }


    [Test]
    public void TestUnsolvable2()
    {
        string input = "005300000800000020070010500400005300010070006003200009060500040000000700000003002";
        string result = "The Board You Entered Is Invalid and Unsolvable.";
        solve(input, result);
    }



    // invalid length input: 


    [Test]
    public void TestInvalidInputLength1()
    {
        string input = "00530000080000002007001050040000530001007000600320000906050004000000070000000300";
        string result = "Input length is invalid. Cannot Represent a square Sudoku board.";
        solve(input, result);
    }

    [Test]
    public void TestInvalidInputLength2()
    {
        string input = "0053000008000000200700105004000053000100700060032000090000000300";
        string result = "Input length is invalid. Cannot Represent a square Sudoku board.";
        solve(input, result);
    }

    [Test]
    public void TestInvalidInputLength3()
    {
        string input = "";
        string result = "Input contains invalid characters for this board/empty string.";
        solve(input, result);
    }
    [Test]
    public void TestInvalidInputLength4()
    {
        string input = "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        string result = "Input length is invalid. Cannot Represent a square Sudoku board.";
        solve(input, result);
    }



    // illegal characters in the board 


    [Test]
    public void TestInvalidCharactersInBoard()
    {
        string input = "0000000000000000000000000000000000A0000000000000000000000000000000000000000000000";
        string result = "Input contains characters outside the allowed range.";
        solve(input, result);
    }

    [Test]
    public void TestInvalidCharactersInBoard2()
    {
        string input = "00000:000000000000000000000000000FA0000000000000000B0000000000000000;000000000000";
        string result = "Input contains characters outside the allowed range.";
        solve(input, result);
    }


    // duplicates in numbers:

    [Test]
    public void TestDuplicatesInRow1()
    {
        string input = "110000000000000000000000000000000000000000000000000000000000000000000000000000000";
        string result = "Illegal Board. Cannot put same characters -- 1 -- in the same row.";
        solve(input, result);
    }


    [Test]
    public void TestDuplicatesInRow2()
    {
        string input = "100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000AA00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        string result = "Illegal Board. Cannot put same characters -- 17 -- in the same row.";
        solve(input, result);
    }



    [Test]
    public void TestDuplicatesInCol1()
    {
        string input = "100000000100000000000000000000000000000000000000000000000000000000000000000000000";
        string result = "Illegal Board. Cannot put same characters -- 1 -- in the same column.";
        solve(input, result);
    }

    [Test]
    public void TestDuplicatesInCol2()
    {
        string input = ";0000000000000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        string result = "Illegal Board. Cannot put same characters -- 11 -- in the same column.";
        solve(input, result);
    }



    [Test]
    public void TestDuplicatesInBox1()
    {
        string input = "100000000010000000000000000000000000000000000000000000000000000000000000000000000";
        string result = "Illegal Board. Cannot put same characters -- 1 -- in the same box.";
        solve(input, result);
    }

    [Test]
    public void TestDuplicatesInBox2()
    {
        string input = ":00000000000000000000000000000000:000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        string result = "Illegal Board. Cannot put same characters -- 10 -- in the same box.";
        solve(input, result);
    }




}
