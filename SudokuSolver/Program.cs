using System;

namespace SudokuSolver;
public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            UserHandler.Game game = new UserHandler.Game();
            game.StartGame();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

