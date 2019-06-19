using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var sudokuGenerator = new SudokuGenerator();
            var sudokuBoard = sudokuGenerator.CreateSudokuBoard();
            var sudokuSolver = new SudokuSolver(sudokuBoard);

            var solvedBoard = sudokuSolver.SolveSudoku();

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                   var value = solvedBoard.Single(p => p.Position.Y == y && p.Position.X == x).Value;

                    if (value != null)
                    {
                        Console.Write($" | {value} | ");
                    }
                    else
                    {
                        Console.Write($" |   | ");
                    }
                }

                Console.WriteLine("---------------------------------------------------");
            }

            Console.Read();
        }
    }
}
