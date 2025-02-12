using System;

namespace SudokuSolver;
public class Program
{
    public static void Main(string[] args)
    {

        UserHandler.Game game = new UserHandler.Game();
        game.StartGame();
    }
}

