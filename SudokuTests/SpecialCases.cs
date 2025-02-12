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
public class SpecialCases : SudokuTestBase
{
    /// <summary>
    /// Returns all test cases for special cases as TestCaseData.
    /// </summary>
    public static IEnumerable<TestCaseData> SpecialCasesBoards
    {
        get
        {
            yield return new TestCaseData(
                "000005080000601043000000000010500000000106000300000005530000061000000004000000000",
                "The Board You Entered Is Invalid and Unsolvable."
            ).SetName("TestUnsolvable1");

            yield return new TestCaseData(
                "005300000800000020070010500400005300010070006003200009060500040000000700000003002",
                "The Board You Entered Is Invalid and Unsolvable."
            ).SetName("TestUnsolvable2");


            yield return new TestCaseData(
                "023000009400000100090030040200910004000007800900040002300090001060000000000500000",
                "Board is Unsolvable."
            ).SetName("TestUnsolvable3");



            yield return new TestCaseData(
                "100006080000700000090050000000560030300000000000003801500001060000020400802005010",
                "Board is Unsolvable."
            ).SetName("TestUnsolvable4");


            yield return new TestCaseData(
                "00530000080000002007001050040000530001007000600320000906050004000000070000000300",
                "Input length is invalid. Cannot Represent a square Sudoku board."
            ).SetName("TestInvalidInputLength1");

            yield return new TestCaseData(
                "0053000008000000200700105004000053000100700060032000090000000300",
                "Input length is invalid. Cannot Represent a square Sudoku board."
            ).SetName("TestInvalidInputLength2");

            yield return new TestCaseData(
                "",
                "Input contains invalid characters for this board/empty string."
            ).SetName("TestInvalidInputLength3");

            yield return new TestCaseData(
                "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
                "Input length is invalid. Cannot Represent a square Sudoku board."
            ).SetName("TestInvalidInputLength4");

            yield return new TestCaseData(
                "0000000000000000000000000000000000A0000000000000000000000000000000000000000000000",
                "Input contains characters outside the allowed range."
            ).SetName("TestInvalidCharactersInBoard");

            yield return new TestCaseData(
                "00000:000000000000000000000000000FA0000000000000000B0000000000000000;000000000000",
                "Input contains characters outside the allowed range."
            ).SetName("TestInvalidCharactersInBoard2");

            yield return new TestCaseData(
                "110000000000000000000000000000000000000000000000000000000000000000000000000000000",
                "Illegal Board. Cannot put same characters -- 1 -- in the same row."
            ).SetName("TestDuplicatesInRow1");

            yield return new TestCaseData(
                "100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000AA00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
                "Illegal Board. Cannot put same characters -- A -- in the same row."
            ).SetName("TestDuplicatesInRow2");

            yield return new TestCaseData(
                "100000000100000000000000000000000000000000000000000000000000000000000000000000000",
                "Illegal Board. Cannot put same characters -- 1 -- in the same column."
            ).SetName("TestDuplicatesInCol1");

            yield return new TestCaseData(
                ";0000000000000000000000000000000000000000000000000000000000000000000000000000000;0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
                "Illegal Board. Cannot put same characters -- ; -- in the same column."
            ).SetName("TestDuplicatesInCol2");

            yield return new TestCaseData(
                "100000000010000000000000000000000000000000000000000000000000000000000000000000000",
                "Illegal Board. Cannot put same characters -- 1 -- in the same box."
            ).SetName("TestDuplicatesInBox1");

            yield return new TestCaseData(
                ":00000000000000000000000000000000:000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000",
                "Illegal Board. Cannot put same characters -- : -- in the same box."
            ).SetName("TestDuplicatesInBox2");
        }
    }



    /// <summary>
    /// This test method will run once for each test case in SpecialCasesBoards.
    /// </summary>
    /// <param name="input">The sudoku board as a string.</param>
    /// <param name="expectedResult">The expected error message.</param>
    [Test, TestCaseSource(nameof(SpecialCasesBoards))]
    public void RunSpecialCaseTest(string input, string expectedResult)
    {
        SolveBoard(input, expectedResult);
    }

}
