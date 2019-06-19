using SudokuSolver.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuSolver
    {
        private List<Point> SudokuMatrix = new List<Point>(81);
        private List<Area> Areas = new List<Area>(9);
        private readonly List<byte> PossibleNumbers = new List<byte>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public SudokuSolver(List<Point> matrix)
        {
            this.SudokuMatrix = matrix;
            this.Inititialize();
        }

        private void Inititialize()
        {
            var allAreaPositions = Enum.GetValues(typeof(AreaNumber)).Cast<AreaNumber>();

            for (int i = 0; i < 9; i++)
            {
                var areaPosition = allAreaPositions.Single(ap => ap == (AreaNumber)i + 1);
                var startPosition = GetStartPositionForArea(areaPosition);
                this.Areas.Add(new Area(areaPosition, startPosition));
            }

            Position GetStartPositionForArea(AreaNumber areaNumber)
            {
                switch (areaNumber
    )
                {
                    case AreaNumber.One:
                        return new Position(0, 0);
                    case AreaNumber.Two:
                        return new Position(0, 3);
                    case AreaNumber.Three:
                        return new Position(0, 6);
                    case AreaNumber.Four:
                        return new Position(3, 0);
                    case AreaNumber.Five:
                        return new Position(3, 3);
                    case AreaNumber.Six:
                        return new Position(3, 6);
                    case AreaNumber.Seven:
                        return new Position(6, 0);
                    case AreaNumber.Eight:
                        return new Position(6, 3);
                    case AreaNumber.Nine:
                        return new Position(6, 6);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private List<Point> GetAreaPoints(AreaNumber areaNumber)
        {
            var areaStartingPosition = this.Areas.Single(area => area.AreaPosition == areaNumber).StartPosition;

            return SudokuMatrix.Where(p => p.Position.X >= areaStartingPosition.X
                                                    && p.Position.X < areaStartingPosition.X + 3
                                                    && p.Position.Y >= areaStartingPosition.Y
                                                    && p.Position.Y < areaStartingPosition.Y + 3).ToList();
        }

        private List<Position> FindPositionsForNumber(byte number)
        {
            return this.SudokuMatrix.Where(p => p.Value == number)
                .Select(p => p.Position)
                .ToList();
        }

        private bool ScanAreaPoints(AreaNumber areaNumber)
        {
            var areaPoints = GetAreaPoints(areaNumber);
            var valuesInArea = areaPoints.Select(ap => ap.Value).ToList();

            var missingNumbers = PossibleNumbers.Where(p => !valuesInArea.Contains(p)).ToList();
            var anyNumberSolved = false;

            foreach (var number in missingNumbers)
            {
                var numberPositions = FindPositionsForNumber(number);

                var numberPositionsX = numberPositions.Select(p => p.X).ToList();
                var numberPositionsY = numberPositions.Select(p => p.Y).ToList();

                var freePosition = areaPoints.Where(a => a.Value == null
                                                      && !numberPositionsX.Contains(a.Position.X)
                                                      && !numberPositionsY.Contains(a.Position.Y)).ToList();

                if (freePosition.Count() == 1)
                {
                    freePosition[0].Value = number;
                    anyNumberSolved = true;
                }
            }

            return anyNumberSolved;
        }

        public byte FindMissingNumber(List<byte> list)
        {
            return PossibleNumbers.Single(p => !list.Contains(p));
        }

        public void ScanRows()
        {

                var emptyYPoints = SudokuMatrix.Where(p => p.Value == null).GroupBy(p => p.Position.Y);

                foreach (var groupOfYPoints in emptyYPoints)
                {
                    var emptyPointsHorizontaly = groupOfYPoints.Select(p => p).ToList();
                    var emptyPointsHorizontalyValues = groupOfYPoints.Select(p => p.Value).ToList();

                    var horizontalNumbers = SudokuMatrix.Where(p => p.Position.Y == groupOfYPoints.Key && p.Value != null).Select(p => p.Value.Value).ToList();

                    var missingNumbers = PossibleNumbers.Where(p => !horizontalNumbers.Contains(p)).ToList();

                    if (emptyPointsHorizontaly.Count == 1)
                    {
                        var horizontalySolvedPointsValues = SudokuMatrix.Where(p => p.Position.Y == groupOfYPoints.Key
                                                                               && p.Value != null)
                                                                       .Select(p => p.Value.Value)
                                                                       .ToList();

                        var missingNumber = FindMissingNumber(horizontalySolvedPointsValues);

                        emptyPointsHorizontaly.Single().Value = missingNumber;
                    }
                    else
                    {
                        foreach (var point in emptyPointsHorizontaly)
                        {
                            var verticalEmptyPointsForRowPoint = SudokuMatrix.Where(p => p.Position.X == point.Position.X
                                                                              && p.Value != null)
                                                                      .Select(p => p.Value.Value)
                                                                      .ToList();


                            if (verticalEmptyPointsForRowPoint.Where(p => missingNumbers.Contains(p)).Count() == emptyPointsHorizontaly.Count - 1)
                            {
                                point.Value = missingNumbers.Single(p => !verticalEmptyPointsForRowPoint.Contains(p));
                            }
                        }
                    }
                }           

                var emptyXPoints = SudokuMatrix.Where(p => p.Value == null).GroupBy(p => p.Position.X);

                foreach (var groupOfXPoints in emptyXPoints)
                {
                    var emptyPointsVerticaly = groupOfXPoints.Select(p => p).ToList();
                    var emptyPointsVerticalyValues = groupOfXPoints.Select(p => p.Value).ToList();

                    var verticalNumbers = SudokuMatrix.Where(p => p.Position.X == groupOfXPoints.Key && p.Value != null).Select(p => p.Value.Value).ToList();

                    var missingNumbers = PossibleNumbers.Where(p => !verticalNumbers.Contains(p)).ToList();

                    if (emptyPointsVerticaly.Count == 1)
                    {
                        var verticalySolvedPointsValues = SudokuMatrix.Where(p => p.Position.X == groupOfXPoints.Key
                                                                               && p.Value != null)
                                                                       .Select(p => p.Value.Value)
                                                                       .ToList();

                        var missingNumber = FindMissingNumber(verticalySolvedPointsValues);

                        emptyPointsVerticaly.Single().Value = missingNumber;
                    }
                    else
                    {
                        foreach (var point in emptyPointsVerticaly)
                        {
                            var verticalEmptyPointsForRowPoint = SudokuMatrix.Where(p => p.Position.Y == point.Position.Y
                                                                              && p.Value != null)
                                                                      .Select(p => p.Value.Value)
                                                                      .ToList();


                            if (verticalEmptyPointsForRowPoint.Where(p => missingNumbers.Contains(p)).Count() == emptyPointsVerticaly.Count - 1)
                            {
                                point.Value = missingNumbers.Single(p => !verticalEmptyPointsForRowPoint.Contains(p));
                            }
                        }
                    }                
            }
        }
        public List<Point> SolveSudoku()
        {
            while (Areas.Any(a => a.IsSolved == false))
            {
                List<Tuple<AreaNumber, bool>> areaSolvedPointPairs = new List<Tuple<AreaNumber, bool>>();

                foreach (var area in Areas.Where(a => a.IsSolved == false))
                {
                    var anyNumberSolved = ScanAreaPoints(area.AreaPosition);
                    areaSolvedPointPairs.Add(new Tuple<AreaNumber, bool>(area.AreaPosition, anyNumberSolved));
                }

                if (areaSolvedPointPairs.All(a => a.Item2 == false))
                {
                    ScanRows();
                }

                foreach (var area in Areas)
                {
                    var areaPoints = GetAreaPoints(area.AreaPosition);

                    if (areaPoints.All(p => p.Value != null))
                    {
                        area.IsSolved = true;
                    }
                } 
            }

            return this.SudokuMatrix;
        }
    }


    public class SudokuGenerator
    {
        private readonly List<byte> PossibleNumbers = new List<byte>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public List<Point> CreateSudokuBoard()
        {
            var board = new List<Point>();

            for (int i = 0; i < 81; i++)
            {
                board.Add(new Point(0, 0));
            }

            int counter = 0;

            for (byte y = 0; y < 9; y++)
            {
                for (byte x = 0; x < 9; x++)
                {
                    board[counter].Position.Y = y;
                    board[counter].Position.X = x;
                    board[counter].Value = null;

                    counter++;
                }
            }

            board.Single(p => p.Position.Y == 0 && p.Position.X == 1).Value = 3;
            board.Single(p => p.Position.Y == 0 && p.Position.X == 2).Value = 7;
            board.Single(p => p.Position.Y == 0 && p.Position.X == 3).Value = 6;

            board.Single(p => p.Position.Y == 1 && p.Position.X == 0).Value = 8;
            board.Single(p => p.Position.Y == 1 && p.Position.X == 4).Value = 3;
            board.Single(p => p.Position.Y == 1 && p.Position.X == 5).Value = 9;
            board.Single(p => p.Position.Y == 1 && p.Position.X == 6).Value = 7;
            board.Single(p => p.Position.Y == 1 && p.Position.X == 8).Value = 2;

            board.Single(p => p.Position.Y == 2 && p.Position.X == 0).Value = 4;
            board.Single(p => p.Position.Y == 2 && p.Position.X == 5).Value = 7;
            board.Single(p => p.Position.Y == 2 && p.Position.X == 8).Value = 6;

            board.Single(p => p.Position.Y == 3 && p.Position.X == 4).Value = 9;
            board.Single(p => p.Position.Y == 3 && p.Position.X == 5).Value = 3;
            board.Single(p => p.Position.Y == 3 && p.Position.X == 6).Value = 2;
            board.Single(p => p.Position.Y == 3 && p.Position.X == 7).Value = 6;

            board.Single(p => p.Position.Y == 4 && p.Position.X == 1).Value = 2;
            board.Single(p => p.Position.Y == 4 && p.Position.X == 2).Value = 5;
            board.Single(p => p.Position.Y == 4 && p.Position.X == 3).Value = 7;
            board.Single(p => p.Position.Y == 4 && p.Position.X == 7).Value = 8;

            board.Single(p => p.Position.Y == 5 && p.Position.X == 0).Value = 9;
            board.Single(p => p.Position.Y == 5 && p.Position.X == 1).Value = 7;
            board.Single(p => p.Position.Y == 5 && p.Position.X == 4).Value = 6;
            board.Single(p => p.Position.Y == 5 && p.Position.X == 6).Value = 4;

            board.Single(p => p.Position.Y == 6 && p.Position.X == 0).Value = 2;
            board.Single(p => p.Position.Y == 6 && p.Position.X == 2).Value = 9;
            board.Single(p => p.Position.Y == 6 && p.Position.X == 4).Value = 4;

            board.Single(p => p.Position.Y == 7 && p.Position.X == 2).Value = 4;
            board.Single(p => p.Position.Y == 7 && p.Position.X == 5).Value = 6;
            board.Single(p => p.Position.Y == 7 && p.Position.X == 8).Value = 8;

            board.Single(p => p.Position.Y == 8 && p.Position.X == 3).Value = 8;
            board.Single(p => p.Position.Y == 8 && p.Position.X == 7).Value = 4;
            board.Single(p => p.Position.Y == 8 && p.Position.X == 8).Value = 9;

            return board;
        }
    }
}