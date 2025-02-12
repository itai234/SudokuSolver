using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SudokuSolver.Solve;
using SudokuSolver.DataStructures.Board;
using SudokuSolver.Exceptions;
using SudokuSolver.Solve.ComputerAlgorithm;
using SudokuSolver.Solve.HumanHeuristics;


namespace SudokuTests;

/// <summary>
/// Base class for the sudoku tests classes to inherit from.
/// </summary>
public abstract class SudokuTestBase
{
    protected HumanTechniques<int> _humanTechniques;
    protected ComputerTechniques<int> _computerTechniques;
    protected SudokuBoard<int> _sudokuBoard;
    protected SudokuSolver.Validation.ValidateInput<int> _validator;
    protected static readonly int _maxTimeForGrid = 1000;
    protected SolverManager<int> _solver;
    protected Stopwatch _stopwatch = new Stopwatch();

    [SetUp]
    public void Setup()
    {
    }

    /// <summary>
    /// Runs the solving algorithm for the given input,asserts the result and
    /// catches an exception if thrown by the custom exceptions.
    /// </summary>
    /// <param name="input">The sudoku board as a string.</param>
    /// <param name="expectedResult">The expected result or exception.</param>
    protected void SolveBoard(string input, string expectedResult)
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
            _stopwatch.Stop();
            if (_stopwatch.ElapsedMilliseconds > _maxTimeForGrid && _sudokuBoard.Size <= 16)
                Assert.Fail("Took Too Long");
            Assert.That(_sudokuBoard.BoardToString(), Is.EqualTo(expectedResult));
        }
        catch (Exception ex)
        {
            _stopwatch.Stop();
            if (_stopwatch.ElapsedMilliseconds > _maxTimeForGrid && _sudokuBoard.Size <= 16)
                Assert.Fail("Took Too Long");
            Assert.That(ex.Message, Is.EqualTo(expectedResult));
        }
    }
}

