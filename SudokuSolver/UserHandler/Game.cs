using SudokuSolver.DataStructures.Board;
using SudokuSolver.Solve;
using SudokuSolver.UserHandler.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.UserHandler;

/// <summary>
/// this class will be the main class of the sudoku.
/// </summary>
public class Game
{
    public void StartGame()
    {
        Utilities.SudokuBoardUtilities.EngineTrick();

        while (true)
        {
            try
            {
                SudokuMenuHandler.MenuUserChoice();
            }
            catch(EndOfStreamException ex)
            {
                Console.WriteLine("Try again.");
            }
            catch(ThreadInterruptedException ex)
            {
                Console.WriteLine("Try again.");
            }
            catch(IOException ex)
            {
                Console.WriteLine("Try again.");
            }

            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Input too long");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected Exception Occured");
            }

        }
                
    }

}

