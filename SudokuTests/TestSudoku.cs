using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SudokuSolver.Solve;
using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;

namespace SudokuTests;

[TestFixture]
public class Tests
{
    private HumanTechniques<int> _HumanTechniques;
    private ComputerTechniques<int> _ComputerTechniques;
    private SudokuBoard<int> _SudokuBoard;
    private List<string> SudokusBoards;
    private static int maxTimeForGrid = 1000 ;

    [SetUp]
    public void Setup()
    {
        SudokusBoards = new List<string>();
        string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string filePath = Path.Combine(executingDirectory, "HardSudokus.txt");

        try
        {
            SudokusBoards.AddRange(File.ReadAllLines(filePath));
        }
        catch (Exception ex)
        {
            Assert.Fail($"failed to read the file: {ex.Message}");
        }
    }

    [Test]
    public void TestSolveLegalSudokus()
    {
        long totalTime = 0;  
        int solvedBoardsCount = 0;  
        foreach (string grid in SudokusBoards)
        {
            if (string.IsNullOrWhiteSpace(grid)) continue;

            string input = grid.Replace(" ", "").Replace('.', '0');
            SudokuSolver.Validation.ValidateInput<int> validator = new SudokuSolver.Validation.ValidateInput<int>(input);

            try
            {
                validator.Validate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Validation failed for grid: {grid}, error: {ex.Message}");
                continue;
            }

            _SudokuBoard = new SudokuBoard<int>(input);
            _HumanTechniques = new HumanTechniques<int>();
            _ComputerTechniques = new ComputerTechniques<int>();

            var solver = new SolverManager<int>(_SudokuBoard);
            solver.AddTechnique(_HumanTechniques);
            solver.AddTechnique(_ComputerTechniques);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            bool solved = false;

            try
            {
                solved = solver.SolveBoard();
            }
            catch (Exception ex)
            {
                Assert.Fail($"exception: {ex.Message}\n for board : {grid}");
                Console.WriteLine( $"exception: {ex.Message}\n for board : {grid}");
            }
            watch.Stop();
            var time = watch.ElapsedMilliseconds;
            totalTime += time;  
            if (solved)
            {
                solvedBoardsCount++; 
            }
            if (time > maxTimeForGrid)
            {
                Assert.Fail($"solving the grid took too long: {time} milliseconds.");
            }
            Console.WriteLine($"solved grid in {time} milliseconds.");

            Assert.That(solved, Is.True, "Board was not solved successfully.");
            Assert.That(_SudokuBoard.IsBoardSolved(), Is.True, "Board is not fully solved.");
        }
        if (solvedBoardsCount > 0)
        {
            long avgTime = totalTime / solvedBoardsCount;  
            Console.WriteLine($"Average Solving time for the boards: {avgTime} milliseconds.");
        }
    }
}
